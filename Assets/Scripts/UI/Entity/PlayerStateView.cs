using System.ComponentModel;
using Data;
using UI.Entity.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Entity
{
    /// <summary>
    /// Hp, Mp, Fp 등 플레이어 상태 View
    /// </summary>
    public class PlayerStateView : UIEntity
    {
        // Hp, Mp, Fp 창

        [SerializeField] private Image currentHpBar;
        [SerializeField] private Image maxHpBar;
        [SerializeField] private Image currentMpBar;
        [SerializeField] private Image maxMpBar;
        [SerializeField] private Image currentFpBar;
        [SerializeField] private Image maxFpBar;
        
        private void Awake()
        {
            var playerDataViewModel = DataManager.instance.playerDataViewModel;
            playerDataViewModel.PropertyChanged += UpdateStateView;
        }

        protected override void UpdateView()
        {
            UpdateStateView(null, null);
        }

        private void UpdateStateView(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;
            
            var playerDataViewModel = DataManager.instance.playerDataViewModel;
            
            // Max값에 따라 Max 크기 변경
            
            currentHpBar.fillAmount = (float)playerDataViewModel.HealthPoint / playerDataViewModel.MaxHealthPoint;
            currentMpBar.fillAmount = (float)playerDataViewModel.ManaPoint / playerDataViewModel.MaxManaPoint;
            currentFpBar.fillAmount = (float)playerDataViewModel.StaminaPoint / playerDataViewModel.MaxStaminaPoint;
        }
    }
}