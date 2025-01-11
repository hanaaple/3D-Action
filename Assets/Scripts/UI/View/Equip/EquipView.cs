using System;
using System.ComponentModel;
using Data.Item.Base;
using Manager;
using Player;
using TMPro;
using UI.Base;
using UI.Selectable.Container;
using UI.Selectable.Container.Item;
using UI.Selectable.Slot;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.View.Equip
{
    /// <summary>
    /// 장비창 View
    /// 각 Slot에 대한 Event와 Input을 제어한다.
    /// </summary>
    public class EquipView : UIContainerEntity
    {
        [SerializeField] private SelectableSlotContainer selectableSlotContainer;
        
        [SerializeField] private TMP_Text slotNameText;
        [SerializeField] private TMP_Text itemNameText;

        [SerializeField] private GameObject describeViewPanel;

        [FormerlySerializedAs("equipChangeView")] [Space(10)] [SerializeField] private EquipChangeInventoryView equipChangeInventoryView;

        [Space(10)] [SerializeField] private SelectableEquipSlot[] leftWeaponSlots;
        [SerializeField] private SelectableEquipSlot[] rightWeaponSlots;

        [SerializeField] private SelectableEquipSlot[] armorSlots;

        [SerializeField] private SelectableEquipSlot[] accessorySlots;
        [SerializeField] private SelectableEquipSlot[] toolSlots;

        private SelectableEquipSlot helmetSlot => armorSlots[0];
        private SelectableEquipSlot breastPlateSlot => armorSlots[1];
        private SelectableEquipSlot leggingsSlot => armorSlots[2];
        private SelectableEquipSlot shoesSlot => armorSlots[3];

        protected override SelectableSlotContainer GetCurrentContainer()
        {
            return selectableSlotContainer;
        }

        protected override void Awake()
        {
            base.Awake();
            
            // 각 버튼을 연결 및 Container의 Index Count를 넣어준다.
            equipChangeInventoryView.Initialize();
            equipChangeInventoryView.SetContainerLength(EquipSlotType.LeftWeapon, leftWeaponSlots.Length);
            equipChangeInventoryView.SetContainerLength(EquipSlotType.RightWeapon, rightWeaponSlots.Length);
            equipChangeInventoryView.SetContainerLength(EquipSlotType.Helmet, 1);
            equipChangeInventoryView.SetContainerLength(EquipSlotType.BreastPlate, 1);
            equipChangeInventoryView.SetContainerLength(EquipSlotType.Leggings, 1);
            equipChangeInventoryView.SetContainerLength(EquipSlotType.Shoes, 1);
            equipChangeInventoryView.SetContainerLength(EquipSlotType.Accessory, accessorySlots.Length);
            equipChangeInventoryView.SetContainerLength(EquipSlotType.Tool, toolSlots.Length);

            for (var index = 0; index < leftWeaponSlots.Length; index++)
            {
                var weaponSlot = leftWeaponSlots[index];
                weaponSlot.Initialize("왼손 무기", index, EquipSlotType.LeftWeapon);
                AddOnClick(weaponSlot, EquipContainerType.Weapon);
            }

            for (var index = 0; index < rightWeaponSlots.Length; index++)
            {
                var weaponSlot = rightWeaponSlots[index];
                weaponSlot.Initialize("오른손 무기", index, EquipSlotType.RightWeapon);
                AddOnClick(weaponSlot, EquipContainerType.Weapon);
            }

            AddOnClick(helmetSlot, EquipContainerType.Helmet);
            AddOnClick(breastPlateSlot, EquipContainerType.BreastPlate);
            AddOnClick(leggingsSlot, EquipContainerType.Leggings);
            AddOnClick(shoesSlot, EquipContainerType.Shoes);

            helmetSlot.Initialize("헬멧", 0, EquipSlotType.Helmet);
            breastPlateSlot.Initialize("흉갑", 0, EquipSlotType.BreastPlate);
            leggingsSlot.Initialize("각반", 0, EquipSlotType.Leggings);
            shoesSlot.Initialize("신발", 0, EquipSlotType.Shoes);

            for (var index = 0; index < accessorySlots.Length; index++)
            {
                var accessorySlot = accessorySlots[index];
                accessorySlot.Initialize("악세사리", index, EquipSlotType.Accessory);
                AddOnClick(accessorySlot, EquipContainerType.Accessory);
            }

            for (var index = 0; index < toolSlots.Length; index++)
            {
                var toolSlot = toolSlots[index];
                toolSlot.Initialize("사용 아이템", index, EquipSlotType.Tool);
                AddOnClick(toolSlot, EquipContainerType.Tool);
            }
        }

        private void OnEnable()
        {
            //Debug.LogWarning("On Enable Equip View");
            
            PlaySceneManager.instance.BindPlayerData(ViewModelType.Equip, UpdateSlotData);
            PrimitiveUIManager.instance.selectedUiViewModel.PropertyChanged += UpdateBySelectedSlotData;

            // 이거 이전에 Select가 되는건가
            
            UpdateSlotData(null, null);
            UpdateBySelectedSlotData(null, null);
        }

        private void OnDisable()
        {
            PlaySceneManager.instance.UnBindPlayerData(ViewModelType.Equip, UpdateSlotData);
            PrimitiveUIManager.instance.selectedUiViewModel.PropertyChanged -= UpdateBySelectedSlotData;
        }

        public override void OpenOrLoad()
        {
            base.OpenOrLoad();

            describeViewPanel.SetActive(true);
        }

        public override void Close(bool isSelectClear = true)
        {
            base.Close(isSelectClear);

            describeViewPanel.SetActive(false);
        }

        private void UpdateSlotData(object s, PropertyChangedEventArgs e)
        {
            var equipViewModel = PlaySceneManager.instance.playerDataManager.equipViewModel;

            for (var index = 0; index < equipViewModel.leftWeapons.Length; index++)
            {
                var leftWeapon = equipViewModel.leftWeapons[index];
                leftWeaponSlots[index].SetItem(leftWeapon);
            }

            for (var index = 0; index < equipViewModel.rightWeapons.Length; index++)
            {
                var rightWeapon = equipViewModel.rightWeapons[index];
                rightWeaponSlots[index].SetItem(rightWeapon);
            }

            helmetSlot.SetItem(equipViewModel.helmet);
            breastPlateSlot.SetItem(equipViewModel.breastplate);
            leggingsSlot.SetItem(equipViewModel.leggings);
            shoesSlot.SetItem(equipViewModel.shoes);

            for (var index = 0; index < equipViewModel.accessories.Count; index++)
            {
                var accessory = equipViewModel.accessories[index];
                accessorySlots[index].SetItem(accessory);
            }

            for (var index = 0; index < equipViewModel.tools.Count; index++)
            {
                var tool = equipViewModel.tools[index];
                toolSlots[index].SetItem(tool);
            }
        }

        private void UpdateBySelectedSlotData(object sender, PropertyChangedEventArgs e)
        {
            //Debug.LogWarning("UpdateBySelectedSlotData");
            
            var itemSlot = PrimitiveUIManager.instance.selectedUiViewModel.selectedItemSlot;

            var equipSlot = itemSlot as SelectableEquipSlot;
            
            if (equipSlot == null) return;

            if (slotNameText)
                slotNameText.text = equipSlot.GetItemName();

            if (itemNameText)
            {
                var item = equipSlot.GetItem();
                if (item.IsBare())
                {
                    itemNameText.text = "-";
                    // TODO
                    // 맨손이든 몸이든 뭐든 해야됨.
                }
                else if (item.IsNullOrEmpty())
                {
                    itemNameText.text = "-";
                }
                else
                {
                    itemNameText.text = item.GetItemDisplayName();
                }
            }
        }

        private void AddOnClick(SelectableEquipSlot slot, EquipContainerType containerType)
        {
            slot.AddListener(() =>
            {
                // slot.itemSlotIndex가 Indexing할떄마다 Update 되야되는데
                equipChangeInventoryView.SetTarget(containerType, slot.containerSlotIndex);
                equipChangeInventoryView.Push();
            });
        }

        public override void Travel(Action<BaseUIEntity> action)
        {
            base.Travel(action);
            equipChangeInventoryView.Travel(action);
        }
    }
}