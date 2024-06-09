using System.ComponentModel;
using Data;
using Data.PlayItem;
using TMPro;
using UI.Entity.Base;
using UnityEngine;

namespace UI.View.PlayView
{
    /// <summary>
    /// 장착 중인 무기, 도구 View
    /// </summary>
    public class PlaySlotView : UIContainerEntity
    {
        [SerializeField] private PlayItemSlot rightHandSlot;
        [SerializeField] private PlayItemSlot leftHandSlot;
        [SerializeField] private PlayItemSlot toolSlots;
        [SerializeField] private TMP_Text itemDescText;

        private void Awake()
        {
            var equipViewModel = DataManager.instance.playerEquipViewModel;

            equipViewModel.PropertyChanged += UpdateUiView;
        }

        protected override void UpdateView()
        {
            UpdateUiView(null, null);
        }

        // equippedItemData 기반으로 Update Equipped Slot
        private void UpdateUiView(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;

            var equipViewModel = DataManager.instance.playerEquipViewModel;

            // 현재 WeaponIndex의 아이템을 보여준다. 아이템이 Null(맨 손)인 경우 빈 슬롯을 보여준다.
            var rightWeapon = equipViewModel.GetCurrentRightWeapon();
            var leftWeapon = equipViewModel.GetCurrentLeftWeapon();

            rightHandSlot.DisplaySlotUI(rightWeapon?.GetItemData());
            leftHandSlot.DisplaySlotUI(leftWeapon?.GetItemData());

            // 현재 ToolIndex부터 시작해서 1 ~ 3개의 Tool을 보여준다.
            toolSlots.DisplaySlotUI(equipViewModel.GetToolData(toolSlots.GetCount()));

            var tool = equipViewModel.GetCurrentTool();

            itemDescText.text = !tool.IsNullOrEmpty() ? tool.GetItemName() : "";
        }

        public override void OnRightArrow()
        {
            rightHandSlot.BlinkSlot();
        }

        public override void OnLeftArrow()
        {
            leftHandSlot.BlinkSlot();
        }

        public override void OnDownArrow()
        {
            toolSlots.BlinkSlot();

            var equipViewModel = DataManager.instance.playerEquipViewModel;
            equipViewModel.SetToolIndexNext();
        }
    }
}