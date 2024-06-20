using Volt;

namespace SurvivalGame
{
    public class PlayerUI : Script
    {
        public Entity PlayerEntity;
        public Entity CameraEntity;
        public Entity HealthBarEntity;
        public Entity StaminaBarEntity;

        private Player PlayerScript;

        float HealthPercentage = 1;
        float HealthBarStartScale = 0;

        float StaminaPercentage = 1;
        float StaminaBarStartScale = 0;

        private void OnCreate()
        {
            HealthBarStartScale = HealthBarEntity.localScale.x;

            StaminaBarStartScale = StaminaBarEntity.localScale.x;

            if (PlayerEntity != null)
            {
                PlayerScript = PlayerEntity.GetScript<Player>();
            }
        }

        private void OnUpdate(float deltaTime)
        {
            if (CameraEntity != null)
            {
                entity.position = CameraEntity.position;
                entity.rotation = CameraEntity.rotation;
            }

            if(PlayerScript != null)
            {
                SetStaminaPercent(PlayerScript.GetStaminaPercentage());
                SetHealthPercent(PlayerScript.GetHealthPercentage());
            }            
        }

        private void SetStaminaPercent(float percentage)
        {
            StaminaPercentage = percentage;
            UpdateStaminaBar();
        }

        private void SetHealthPercent(float percentage)
        {
            HealthPercentage = percentage;
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            HealthBarEntity.localScale = new Vector3(HealthBarStartScale * HealthPercentage, HealthBarEntity.localScale.y, HealthBarEntity.localScale.z);
        }
        private void UpdateStaminaBar()
        {
            StaminaBarEntity.localScale = new Vector3(StaminaBarStartScale * StaminaPercentage, StaminaBarEntity.localScale.y, StaminaBarEntity.localScale.z);
        }


    }
}
