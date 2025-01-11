using System.ComponentModel;
using Manager;
using Player;
using TMPro;
using UI.Base;
using UnityEngine;

namespace UI.View
{
    /// <summary>
    /// Status View
    /// </summary>
    public class StatusView : BaseUIEntity
    {
        [Header("레벨")]
        public TMP_Text level;
        public TMP_Text experiencePoint;
        public TMP_Text levelUpExp;

        [Header("스테이터스")]
        public TMP_Text constitution;
        public TMP_Text spirit;
        public TMP_Text strength;
        public TMP_Text stamina;

        [Header("캐릭터 데이터")]
        public TMP_Text attack;
        public TMP_Text defense;
        public TMP_Text healthPoint;
        public TMP_Text maxHealthPoint;
        public TMP_Text manaPoint;
        public TMP_Text maxManaPoint;
        public TMP_Text maxStaminaPoint;
        public TMP_Text equipWeight;
        public TMP_Text maxEquipWeight;

        protected void OnEnable()
        {
            PlaySceneManager.instance.BindPlayerData(ViewModelType.Status, UpdateStatusView);
            PlaySceneManager.instance.BindPlayerData(ViewModelType.CharacterData, UpdatePlayerDataView);

            UpdatePlayerDataView(null, null);
            UpdateStatusView(null, null);
        }

        private void OnDisable()
        {
            PlaySceneManager.instance.UnBindPlayerData(ViewModelType.Status, UpdateStatusView);
            PlaySceneManager.instance.UnBindPlayerData(ViewModelType.CharacterData, UpdatePlayerDataView);
        }

        private void UpdatePlayerDataView(object sender, PropertyChangedEventArgs e)
        {
            var playerData = PlaySceneManager.instance.playerDataManager.playerDataViewModel;
            
            if (attack != null) attack.text = playerData.Attack.ToString();
            if (defense != null) defense.text = playerData.Defense.ToString();
            
            if (healthPoint != null) healthPoint.text = playerData.HealthPoint.ToString();
            if (maxHealthPoint != null) maxHealthPoint.text = playerData.MaxHealthPoint.ToString();

            if (manaPoint != null) manaPoint.text = playerData.ManaPoint.ToString();
            if (maxManaPoint != null) maxManaPoint.text = playerData.MaxManaPoint.ToString();
            if (maxStaminaPoint != null) maxStaminaPoint.text = playerData.MaxStaminaPoint.ToString();
            if (equipWeight != null) equipWeight.text = playerData.EquipWeight.ToString();
            if (maxEquipWeight != null) maxEquipWeight.text = playerData.MaxEquipWeight.ToString();
        }

        private void UpdateStatusView(object sender, PropertyChangedEventArgs e)
        {
            var statusViewModel = PlaySceneManager.instance.playerDataManager.statusViewModel;

            if (level) level.text = statusViewModel.Level.ToString();
            if (experiencePoint) experiencePoint.text = statusViewModel.ExperiencePoint.ToString();

            if (levelUpExp)
                levelUpExp.text = statusViewModel.IsLevelUpPossible()
                    ? statusViewModel.RequiredExperiencePoint.ToString()
                    : "-";

            if (constitution) constitution.text = statusViewModel.Vitality.ToString();
            if (spirit) spirit.text = statusViewModel.Spirit.ToString();
            if (strength) strength.text = statusViewModel.Strength.ToString();
            if (stamina) stamina.text = statusViewModel.Endurance.ToString();
        }
    }
}