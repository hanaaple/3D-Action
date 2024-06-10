using System.ComponentModel;
using Data;
using TMPro;
using UI.Entity.Base;

namespace UI.Entity
{
    /// <summary>
    /// Display Player Data
    /// </summary>
    public class PlayerDataView : UIEntity
    {
        public TMP_Text attack;
        public TMP_Text defense;
        public TMP_Text healthPoint;
        public TMP_Text manaPoint;
        public TMP_Text staminaPoint;
        public TMP_Text equipWeight;

        public TMP_Text maxHealthPoint;
        public TMP_Text maxManaPoint;
        public TMP_Text maxStaminaPoint;
        public TMP_Text maxEquipWeight;

        private void Awake()
        {
            var playerDataViewModel = DataManager.instance.playerDataViewModel;
            playerDataViewModel.PropertyChanged += UpdatePlayerDataView;
        }

        protected override void UpdateView()
        {
            UpdatePlayerDataView(null, null);
        }

        private void UpdatePlayerDataView(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;

            var playerDataViewModel = DataManager.instance.playerDataViewModel;
            if (attack != null) attack.text = playerDataViewModel.Attack.ToString();
            if (defense != null) defense.text = playerDataViewModel.Defense.ToString();
            if (healthPoint != null) healthPoint.text = playerDataViewModel.HealthPoint.ToString();

            if (manaPoint != null) manaPoint.text = playerDataViewModel.ManaPoint.ToString();
            if (staminaPoint != null) staminaPoint.text = playerDataViewModel.StaminaPoint.ToString();
            if (equipWeight != null) equipWeight.text = playerDataViewModel.EquipWeight.ToString();
            if (maxHealthPoint != null) maxHealthPoint.text = playerDataViewModel.MaxHealthPoint.ToString();
            if (maxManaPoint != null) maxManaPoint.text = playerDataViewModel.MaxManaPoint.ToString();
            if (maxStaminaPoint != null) maxStaminaPoint.text = playerDataViewModel.MaxStaminaPoint.ToString();
            if (maxEquipWeight != null) maxEquipWeight.text = playerDataViewModel.MaxEquipWeight.ToString();
        }
    }
}