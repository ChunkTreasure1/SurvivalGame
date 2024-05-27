using Volt;

namespace SurvivalGame
{
    class Player : Script
    {
        public Entity CameraEntity;
        public float Speed = 100f;

        private CharacterControllerComponent m_characterController;

        private void OnCreate()
        {
            if (!entity.HasComponent<CharacterControllerComponent>())
            {
                Log.Error("Player must have a character controller component!");
            }

            m_characterController = entity.GetComponent<CharacterControllerComponent>();
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

            m_characterController.Move(resultDir * Speed * deltaTime);
        }
    }
}
