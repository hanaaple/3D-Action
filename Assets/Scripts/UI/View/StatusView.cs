using System.ComponentModel;
using Data;
using TMPro;
using UI.Entity.Base;
using UnityEngine;

namespace UI.View
{
    /// <summary>
    /// Status View
    /// </summary>
    public class StatusView : UIContainerEntity
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
        
        private void Awake()
        {
            var statusViewModel = DataManager.instance.playerStatusViewModel;
            statusViewModel.PropertyChanged += UpdateStatusView;
            
            var playerDataViewModel = DataManager.instance.playerDataViewModel;
            playerDataViewModel.PropertyChanged += UpdatePlayerDataView;
        }

        protected override void UpdateView()
        {
            UpdatePlayerDataView(null, null);
            UpdateStatusView(null, null);
        }

        private void UpdatePlayerDataView(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;
            
            var playerDataViewModel = DataManager.instance.playerDataViewModel;
            if (attack != null) attack.text = playerDataViewModel.Attack.ToString();
            if (defense != null) defense.text = playerDataViewModel.Defense.ToString();
            
            if (healthPoint != null) healthPoint.text = playerDataViewModel.HealthPoint.ToString();
            if (maxHealthPoint != null) maxHealthPoint.text = playerDataViewModel.MaxHealthPoint.ToString();

            if (manaPoint != null) manaPoint.text = playerDataViewModel.ManaPoint.ToString();
            if (maxManaPoint != null) maxManaPoint.text = playerDataViewModel.MaxManaPoint.ToString();
            if (maxStaminaPoint != null) maxStaminaPoint.text = playerDataViewModel.MaxStaminaPoint.ToString();
            if (equipWeight != null) equipWeight.text = playerDataViewModel.EquipWeight.ToString();
            if (maxEquipWeight != null) maxEquipWeight.text = playerDataViewModel.MaxEquipWeight.ToString();
        }

        private void UpdateStatusView(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;
            
            var statusViewModel = DataManager.instance.playerStatusViewModel;

            if (level) level.text = statusViewModel.Level.ToString();
            if (experiencePoint) experiencePoint.text = statusViewModel.ExperiencePoint.ToString();

            if (levelUpExp)
                levelUpExp.text = statusViewModel.IsLevelUpPossible()
                    ? statusViewModel.RequiredExperiencePoint.ToString()
                    : "-";

            if (constitution) constitution.text = statusViewModel.Constitution.ToString();
            if (spirit) spirit.text = statusViewModel.Spirit.ToString();
            if (strength) strength.text = statusViewModel.Strength.ToString();
            if (stamina) stamina.text = statusViewModel.Stamina.ToString();
        }
    }
}