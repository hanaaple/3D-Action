using System.ComponentModel;
using Manager;
using Player;
using TMPro;
using UnityEngine;

namespace UI.View.Entity
{
    public class CharacterDataView : MonoBehaviour
    {
        [Header("레벨")] public TMP_Text level;
        public TMP_Text experiencePoint;

        [Header("스테이터스")] public TMP_Text vitality;
        public TMP_Text spirit;

        public TMP_Text endurance;
        public TMP_Text strength;
        public TMP_Text workmanship;
        public TMP_Text intellect;

        [Header("캐릭터 데이터")] public TMP_Text healthPoint;
        public TMP_Text maxHealthPoint;

        public TMP_Text manaPoint;
        public TMP_Text maxManaPoint;

        public TMP_Text maxStaminaPoint;

        public TMP_Text equipWeight;
        public TMP_Text maxEquipWeight;
        public TMP_Text weightState; // 가벼움, 보통, 무거움 등


        private void OnEnable()
        {
            PlaySceneManager.instance.BindPlayerData(ViewModelType.Status, UpdateStatusView);
            PlaySceneManager.instance.BindPlayerData(ViewModelType.CharacterData, UpdatePlayerDataView);

            UpdateStatusView(null, null);
            UpdatePlayerDataView(null, null);
        }

        private void OnDisable()
        {
            PlaySceneManager.instance.UnBindPlayerData(ViewModelType.Status, UpdateStatusView);
            PlaySceneManager.instance.UnBindPlayerData(ViewModelType.CharacterData, UpdatePlayerDataView);
        }


        private void UpdatePlayerDataView(object sender, PropertyChangedEventArgs e)
        {
            var playerDataViewModel = PlaySceneManager.instance.playerDataManager.playerDataViewModel;

            if (healthPoint) healthPoint.text = playerDataViewModel.HealthPoint.ToString();
            if (maxHealthPoint) maxHealthPoint.text = playerDataViewModel.MaxHealthPoint.ToString();

            if (manaPoint) manaPoint.text = playerDataViewModel.ManaPoint.ToString();
            if (maxManaPoint) maxManaPoint.text = playerDataViewModel.MaxManaPoint.ToString();

            if (maxStaminaPoint) maxStaminaPoint.text = playerDataViewModel.MaxStaminaPoint.ToString();

            if (equipWeight) equipWeight.text = playerDataViewModel.EquipWeight.ToString();
            if (maxEquipWeight) maxEquipWeight.text = playerDataViewModel.MaxEquipWeight.ToString();

            var weightRatio = playerDataViewModel.EquipWeight / playerDataViewModel.MaxEquipWeight;
            var weightText = weightRatio switch
            {
                < 0.7f => "가벼움 (임시)",
                < 1f => "보통 (임시)",
                _ => "무거움 (임시)"
            };
            if(weightState) weightState.text = weightText;
        }

        private void UpdateStatusView(object sender, PropertyChangedEventArgs e)
        {
            // 레벨, 경험치 포인트
            // 생명력, 정신력, 지구력
            // 근력, 기량, 지력

            var statusViewModel = PlaySceneManager.instance.playerDataManager.statusViewModel;

            if (level) level.text = statusViewModel.Level.ToString();
            if (experiencePoint) experiencePoint.text = statusViewModel.ExperiencePoint.ToString();

            if (vitality) vitality.text = statusViewModel.Vitality.ToString();
            if (spirit) spirit.text = statusViewModel.Spirit.ToString();
            if (endurance) endurance.text = statusViewModel.Endurance.ToString();

            if (strength) strength.text = statusViewModel.Strength.ToString();
            if (workmanship) workmanship.text = statusViewModel.Workmanship.ToString();
            if (intellect) intellect.text = statusViewModel.Intellect.ToString();
        }
    }
}