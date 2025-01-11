using System.ComponentModel;
using Manager;
using Player;
using TMPro;
using UnityEngine;

namespace UI.Entity
{
    /// <summary>
    /// Display Player Data
    /// </summary>
    public class PlayerDataView : MonoBehaviour
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

        private void OnEnable()
        {
            PlaySceneManager.instance.BindPlayerData(ViewModelType.CharacterData, UpdateUI);
            UpdateUI(null, null);
        }
        
        private void OnDisable()
        {
            PlaySceneManager.instance.UnBindPlayerData(ViewModelType.CharacterData, UpdateUI);
        }

        private void UpdateUI(object sender, PropertyChangedEventArgs e)
        {
            var playerDataViewModel = PlaySceneManager.instance.playerDataManager.playerDataViewModel;
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