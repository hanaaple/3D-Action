using System.ComponentModel;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using View.Slot;

namespace View
{
    /// <summary>
    /// 장착 중인 무기, 도구 View
    /// </summary>
    public class PlaySlotView : MonoBehaviour
    {
        [SerializeField] private ItemSlot rightHandSlot;
        [SerializeField] private ItemSlot leftHandSlot;
        [SerializeField] private ItemSlot toolSlots;
        [SerializeField] private TMP_Text itemDescText;

        private void Start()
        {
            var equipViewModel = DataManager.instance.equipViewModel;

            equipViewModel.PropertyChanged += UpdateSlotUI;

            UpdateSlotUI(null, null);
        }

        // equippedItemData 기반으로 Update Equipped Slot
        private void UpdateSlotUI(object sender, PropertyChangedEventArgs e)
        {
            var equipViewModel = DataManager.instance.equipViewModel;

            // 현재 WeaponIndex의 아이템을 보여준다. 아이템이 Null(맨 손)인 경우 빈 슬롯을 보여준다.
            var rightWeapon = equipViewModel.GetCurrentRightWeapon();
            var leftWeapon = equipViewModel.GetCurrentLeftWeapon();
            
            rightHandSlot.DisplaySlotUI(rightWeapon?.weaponData);
            leftHandSlot.DisplaySlotUI(leftWeapon?.weaponData);

            // 현재 ToolIndex부터 시작해서 1 ~ 3개의 Tool을 보여준다.
            toolSlots.DisplaySlotUI(equipViewModel.GetTools(toolSlots.GetCount()));

            var tool = equipViewModel.GetCurrentTool();
            
            itemDescText.text = tool != null ? tool.GetItemName() : "";
        }

#if ENABLE_INPUT_SYSTEM
        public void OnEsc(InputValue inputValue)
        {
            // MainMenuOpen
        }

        public void OnChangeRightWeapon(InputValue inputValue)
        {
            // Just Blink Left Slot
            rightHandSlot.BlinkSlot();
        }

        public void OnChangeLeftWeapon(InputValue inputValue)
        {
            // Just Blink Right Slot
            leftHandSlot.BlinkSlot();
        }

        public void OnChangeItem(InputValue inputValue)
        {
            var equipViewModel = DataManager.instance.equipViewModel;
            // TryChangeToolIndex
            // Just Blink Item Slot

            toolSlots.BlinkSlot();
            equipViewModel.SetToolIndexNext();
        }
#endif
    }
}