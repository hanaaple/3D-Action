using System.ComponentModel;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    /// <summary>
    /// Hp, Mp, Fp 등 플레이어 상태 View
    /// </summary>
    public class PlayerStatusView : MonoBehaviour
    {
        // Hp, Mp, Fp 창

        [SerializeField] private Image currentHpBar;
        [SerializeField] private Image maxHpBar;
        [SerializeField] private Image currentMpBar;
        [SerializeField] private Image maxMpBar;
        [SerializeField] private Image currentFpBar;
        [SerializeField] private Image maxFpBar;
        
        private void Start()
        {
            var playerDataViewModel = DataManager.instance.playerDataViewModel;
            playerDataViewModel.PropertyChanged += UpdateUI;
        }

        private void UpdateUI(object sender, PropertyChangedEventArgs e)
        {
            var playerDataViewModel = DataManager.instance.playerDataViewModel;
            
            // Max값에 따라 Max 크기 변경
            
            currentHpBar.fillAmount = (float)playerDataViewModel.HealthPoint / playerDataViewModel.MaxHealthPoint;
            currentMpBar.fillAmount = (float)playerDataViewModel.ManaPoint / playerDataViewModel.MaxManaPoint;
            currentFpBar.fillAmount = (float)playerDataViewModel.StaminaPoint / playerDataViewModel.MaxStaminaPoint;
        }
    }
}