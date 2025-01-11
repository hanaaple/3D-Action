using System.ComponentModel;
using System.Linq;
using Data.Item.Base;
using Data.Play;
using Manager;
using Player;
using TMPro;
using UI.Base;
using UnityEngine;

namespace UI.View.Play
{
    /// <summary>
    /// 장착 중인 무기, 도구 View
    /// </summary>
    public class PlaySlotView : BaseUIEntity
    {
        [SerializeField] private PlayItemSlot rightHandSlot;
        [SerializeField] private PlayItemSlot leftHandSlot;
        [SerializeField] private PlayItemSlot toolSlots;
        
        [SerializeField] private TMP_Text toolDescText;

        private UIInput _uiInput;
        
        public void Awake()
        {
            // 이건 Input이 가능하면서 동시에 실행되도 되는거임.
            
            _uiInput = new UIInput
            {
                LeftArrow = () => leftHandSlot.BlinkSlot(),
                RightArrow = () => rightHandSlot.BlinkSlot(),
                DownArrow = () =>
                {
                    toolSlots.BlinkSlot();
                    PlaySceneManager.instance.playerDataManager.equipViewModel.MoveToNextToolIndex();
                },
                IsRightArrowActive = () => gameObject.activeSelf,
                IsLeftArrowActive = () => gameObject.activeSelf,
                IsDownArrowActive = () => gameObject.activeSelf,
                IsLeftArrowInterrupt = false,
                IsRightArrowInterrupt = false,
            };
        }

        protected override UIInput GetUIInput()
        {
            return _uiInput;
        }

        protected void OnEnable()
        {
            PlaySceneManager.instance.BindPlayerData(ViewModelType.Equip, UpdateUI);
            UpdateUI(null, null);
        }
        
        private void OnDisable()
        {
            PlaySceneManager.instance.UnBindPlayerData(ViewModelType.Equip, UpdateUI);
        }

        // equippedItemData 기반으로 Update Equipped Slot
        private void UpdateUI(object sender, PropertyChangedEventArgs e)
        {
            var playerDataManager = PlaySceneManager.instance.playerDataManager;
            if (playerDataManager == null) return;
            
            var equipViewModel = PlaySceneManager.instance.playerDataManager.equipViewModel;

            var rightWeapon = equipViewModel.GetCurrentWeapon(WeaponEquipType.Right);
            var leftWeapon = equipViewModel.GetCurrentWeapon(WeaponEquipType.Left);
            var tool = equipViewModel.GetCurrentTool();

            var rightWeaponIcon = rightWeapon?.GetItemData().slotSprite;
            var leftWeaponIcon = leftWeapon?.GetItemData().slotSprite;
            var toolIcons = equipViewModel.GetEquippedToolData(toolSlots.GetCount()).Select(item => item.slotSprite);
            
            rightHandSlot.DisplaySlotUI(rightWeaponIcon);
            leftHandSlot.DisplaySlotUI(leftWeaponIcon);
            toolSlots.DisplaySlotUI(toolIcons);
            
            toolDescText.text = tool.IsNullOrEmpty() ? "" : tool.GetItemDisplayName();
        }
    }
}