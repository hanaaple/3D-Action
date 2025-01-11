using System;
using System.ComponentModel;
using Data.Play;
using Manager;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Play
{
    public class PlayerStatusBar : MonoBehaviour
    {
        public Image hpBar;

        // public Image mpBar;
        public Image staminaBar;
        public Image poiseBar;

        public RectTransform hpBarRect;
        public RectTransform staminaBarRect;
        public RectTransform poiseBarRect;

        public float hpBarWeight;
        public float staminaBarWeight;
        public float poiseBarWeight;

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
            var playerDataManager = PlaySceneManager.instance.playerDataManager;
            if (playerDataManager == null) return;
            
            var playerDataViewModel = playerDataManager.playerDataViewModel;
            hpBar.fillAmount = playerDataViewModel.HealthPoint / playerDataViewModel.MaxHealthPoint;
            staminaBar.fillAmount = playerDataViewModel.StaminaPoint / playerDataViewModel.MaxStaminaPoint;
            // mpBar.fillAmount = playerDataViewModel.HealthPoint / (float)playerDataViewModel.MaxHealthPoint;
            poiseBar.fillAmount = playerDataViewModel.PoiseHealthPoint / playerDataViewModel.MaxPoiseHealthPoint;

            Debug.Log($"{playerDataViewModel.HealthPoint} / {playerDataViewModel.MaxHealthPoint}  " +
                      $"{playerDataViewModel.StaminaPoint} / {playerDataViewModel.MaxStaminaPoint}  " +
                      $"{playerDataViewModel.PoiseHealthPoint} / {playerDataViewModel.MaxPoiseHealthPoint}");

            hpBarRect.sizeDelta = new Vector2(playerDataViewModel.MaxHealthPoint * hpBarWeight, hpBarRect.sizeDelta.y);
            staminaBarRect.sizeDelta = new Vector2(playerDataViewModel.MaxStaminaPoint * staminaBarWeight,
                staminaBarRect.sizeDelta.y);
            poiseBarRect.sizeDelta = new Vector2(playerDataViewModel.MaxPoiseHealthPoint * poiseBarWeight,
                poiseBarRect.sizeDelta.y);
        }
    }
}