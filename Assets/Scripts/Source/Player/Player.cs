using Volt;

namespace SurvivalGame
{
    public class Player : Script
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

        private void OnCreate()
        {
        }

        private void OnUpdate(float deltaTime)
        {
        }
    }
}
