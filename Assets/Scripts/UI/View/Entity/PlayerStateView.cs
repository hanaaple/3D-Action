using System.ComponentModel;
using Data;
using Manager;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Entity
{
    /// <summary>
    /// Hp, Mp, Fp 등 플레이어 상태 View
    /// </summary>
    public class PlayerStateView : MonoBehaviour
    {
        [SerializeField] private Image currentHpBar;
        [SerializeField] private Image maxHpBar;
        [SerializeField] private Image currentMpBar;
        [SerializeField] private Image maxMpBar;
        [SerializeField] private Image currentFpBar;
        [SerializeField] private Image maxFpBar;
        
        // ViewModel or Presenter에 접근
        
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

            // Max값에 따라 Max 크기 변경

            currentHpBar.fillAmount = Mathf.Lerp(currentHpBar.fillAmount,
                playerDataViewModel.HealthPoint / playerDataViewModel.MaxHealthPoint, Time.deltaTime);
            currentMpBar.fillAmount = Mathf.Lerp(currentMpBar.fillAmount,
                playerDataViewModel.ManaPoint / playerDataViewModel.MaxManaPoint, Time.deltaTime);
            currentFpBar.fillAmount = Mathf.Lerp(currentFpBar.fillAmount,
                playerDataViewModel.StaminaPoint / playerDataViewModel.MaxStaminaPoint, Time.deltaTime);
        }
    }
}