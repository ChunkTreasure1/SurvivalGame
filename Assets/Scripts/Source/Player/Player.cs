using Volt;

namespace SurvivalGame
{
    class Player : Script
    {
        public Entity CameraEntity;
        
        public float MaxStamina = 100f;
        public float StaminaDecreasePerSecond = 5;
        public float StaminaIncreasePerSecond = 3;
        public float MinSprintStaminaAmount = 10;

        public float Speed = 100f;
        public float SprintSpeed = 150f;
        public float JumpForce = 100f;

        public float MaxItemGrabLength = 200f;

        private CharacterControllerComponent m_characterController;
        private bool m_disableSprinting = false;
        private float m_currentStaminaAmount = 0f;

        private void OnCreate()
        {
            if (!entity.HasComponent<CharacterControllerComponent>())
            {
                Log.Error("Player must have a character controller component!");
            }

            m_characterController = entity.GetComponent<CharacterControllerComponent>();
            m_currentStaminaAmount = MaxStamina;
        }

        private void OnUpdate(float deltaTime)
        {
            if (CameraEntity == null) 
            {
                Log.Error("Camera Entity must not be null!");
                return;
            }

            Vector3 forward = CameraEntity.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = CameraEntity.right;
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

            if (Input.IsKeyPressed(KeyCode.Space))
            {
                m_characterController.Jump(JumpForce);
            }

            bool isSprinting = Input.IsKeyDown(KeyCode.Left_Shift) && !m_disableSprinting;

            if (Input.IsKeyReleased(KeyCode.Left_Shift))
            {
                m_disableSprinting = false;
            }

            if (isSprinting && m_currentStaminaAmount < MinSprintStaminaAmount)
            {
                isSprinting = false;
                m_disableSprinting = true;
            }

            if (isSprinting)
            {
                m_currentStaminaAmount -= StaminaDecreasePerSecond * deltaTime;
                if (m_currentStaminaAmount < 0f)
                {
                    m_currentStaminaAmount = 0f;
                }
            }
            else
            {
                m_currentStaminaAmount += StaminaIncreasePerSecond * deltaTime;
                if (m_currentStaminaAmount > MaxStamina)
                {
                    m_currentStaminaAmount = MaxStamina;
                }
            }

            m_characterController.Move(resultDir * (isSprinting ? SprintSpeed : Speed) * deltaTime);

            RaycastHit hit;
            if (Physics.Raycast(CameraEntity.position, CameraEntity.forward, out hit, MaxItemGrabLength, 2))
            {
                if (Input.IsKeyPressed(KeyCode.E))
                {
                    Log.Trace("Item picked up!");
                }
            }
        }

        public float GetStaminaPercentage()
        {
            return m_currentStaminaAmount / MaxStamina;
        }

        public float GetHealthPercentage()
        {
            return 1f;
        }
    }
}
