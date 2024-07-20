using Volt;

namespace SurvivalGame
{
    public class PlayerController : Script
    {
        public float MouseSensitivity = 0.15f;

        private Player m_player = null;
        private CharacterControllerComponent m_characterController;

        private bool m_disableSprinting = false;
        private float m_currentStaminaAmount = 0f;

        private Vector2 m_lastMousePosition = Vector2.Zero;
        private Vector3 m_currentRotation = Vector3.Zero;

        private void OnCreate()
        {
            if (!entity.HasScript<Player>())
            {
                Log.Error("Player must have Player script!");
            }

            if (!entity.HasComponent<CharacterControllerComponent>())
            {
                Log.Error("Player must have a character controller component!");
            }

            m_characterController = entity.GetComponent<CharacterControllerComponent>();
            m_player = entity.GetScript<Player>();

            m_currentStaminaAmount = m_player.MaxStamina;
            m_lastMousePosition = Input.GetMousePosition();
        }

        private void OnUpdate(float deltaTime)
        {
            if (m_player == null) 
            {
                Log.Error("Player must not be null!");
                return;
            }

            if (m_player.CameraEntity == null)
            {
                Log.Error("Camera Entity must not be null!");
                return;
            }

            HandleMouseMovement(deltaTime);
            HandleMovement(deltaTime);
            HandleActions(deltaTime);
        }

        private void HandleMovement(float deltaTime)
        {
            Vector3 forward = m_player.CameraEntity.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = m_player.CameraEntity.right;
            right.y = 0f;
            right.Normalize();

            Vector3 resultDir = Vector3.Zero;

            if (Input.IsKeyDown(KeyCode.W))
            {
                resultDir += forward;
            }

            if (Input.IsKeyDown(KeyCode.S))
            {
                resultDir -= forward;
            }

            if (Input.IsKeyDown(KeyCode.A))
            {
                resultDir -= right;
            }

            if (Input.IsKeyDown(KeyCode.D))
            {
                resultDir += right;
            }

            if (Input.IsKeyPressed(KeyCode.Space) && m_characterController.isGrounded)
            {
                m_characterController.Jump(m_player.JumpForce);
            }

            bool isSprinting = Input.IsKeyDown(KeyCode.Left_Shift) && !m_disableSprinting;

            if (Input.IsKeyReleased(KeyCode.Left_Shift))
            {
                m_disableSprinting = false;
            }

            if (isSprinting && m_currentStaminaAmount < m_player.MinSprintStaminaAmount)
            {
                isSprinting = false;
                m_disableSprinting = true;
            }

            if (isSprinting)
            {
                m_currentStaminaAmount -= m_player.StaminaDecreasePerSecond * deltaTime;
                if (m_currentStaminaAmount < 0f)
                {
                    m_currentStaminaAmount = 0f;
                }
            }
            else
            {
                m_currentStaminaAmount += m_player.StaminaIncreasePerSecond * deltaTime;
                if (m_currentStaminaAmount > m_player.MaxStamina)
                {
                    m_currentStaminaAmount = m_player.MaxStamina;
                }
            }

            m_characterController.Move(resultDir * (isSprinting ? m_player.SprintSpeed : m_player.Speed) * deltaTime);
        }

        private void HandleActions(float deltaTime)
        {
            RaycastHit hit;
            if (Physics.Raycast(m_player.CameraEntity.position, m_player.CameraEntity.forward, out hit, m_player.MaxItemGrabLength, 2))
            {
                if (Input.IsKeyPressed(KeyCode.E))
                {
                    Log.Trace("Item picked up!");
                }
            }
        }

        private void HandleMouseMovement(float deltaTime)
        {
            Entity camEntity = m_player.CameraEntity;

            Vector2 mouseDelta = Input.GetMousePosition() - m_lastMousePosition;
            m_lastMousePosition = Input.GetMousePosition();

            float yawSign = camEntity.up.y < 0f ? -1f : 1f;
            float yawDelta = yawSign * mouseDelta.x * MouseSensitivity;
            float pitchDelta = mouseDelta.y * MouseSensitivity;

            m_currentRotation += new Vector3(Mathf.Radians(pitchDelta), Mathf.Radians(yawDelta), 0f);
            m_currentRotation.x = Mathf.Clamp(m_currentRotation.x, -1.35f, 1.35f);

            entity.rotation = new Quaternion(new Vector3(0f, m_currentRotation.y, 0f));
            m_player.CameraEntity.localRotation = new Quaternion(new Vector3(m_currentRotation.x, 0f, 0f));
        }
    }
}
