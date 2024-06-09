using System;
using System.ComponentModel;
using Data;
using Data.PlayItem;
using TMPro;
using UI.Entity.Base;
using UI.Entity.Selectable.Slot;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Equip
{
    public class EquipView : UIContainerEntity
    {
        [SerializeField] private TMP_Text slotNameText;
        [SerializeField] private TMP_Text itemNameText;

        [SerializeField] private GameObject describeViewPanel;
        [SerializeField] private GameObject characterDataViewPanel;
        
        [SerializeField] private SelectableItemSlot[] leftWeaponSlots;
        [SerializeField] private SelectableItemSlot[] rightWeaponSlots;
        
        [SerializeField] private SelectableItemSlot helmetSlot;
        [SerializeField] private SelectableItemSlot breastPlateSlot;
        [SerializeField] private SelectableItemSlot leggingsSlot;
        [SerializeField] private SelectableItemSlot shoesSlot;
        
        [SerializeField] private SelectableItemSlot[] accessorySlots;
        [SerializeField] private SelectableItemSlot[] toolSlots;

        public EquipInventoryView equipInventoryView;
        
        private void Awake()
        {
            foreach (var weaponSlot in leftWeaponSlots)
            {
                InitializeItemSlotButton(weaponSlot);
            }

            foreach (var weaponSlot in rightWeaponSlots)
            {
                InitializeItemSlotButton(weaponSlot);
            }
            
            InitializeItemSlotButton(helmetSlot);
            InitializeItemSlotButton(breastPlateSlot);
            InitializeItemSlotButton(leggingsSlot);
            InitializeItemSlotButton(shoesSlot);
            
            foreach (var accessorySlot in accessorySlots)
            {
                InitializeItemSlotButton(accessorySlot);
            }
            
            foreach (var toolSlot in toolSlots)
            {
                InitializeItemSlotButton(toolSlot);
            }
            
            DataManager.instance.selectedUiViewModel.PropertyChanged += UpdateBySelectedSlotData;
            DataManager.instance.playerEquipViewModel.PropertyChanged += UpdateSlotData;
        }
        
        public override void Open()
        {
            base.Open();
            
            describeViewPanel.SetActive(true);
            characterDataViewPanel.SetActive(true);
        }

        public override void Close()
        {
            base.Close();
            
            describeViewPanel.SetActive(false);
            characterDataViewPanel.SetActive(false);
        }
        
        protected override void UpdateView()
        {
            UpdateSlotData(null, null);
            UpdateBySelectedSlotData(null, null);
        }
        
        private void UpdateSlotData(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;
            
            var equipViewModel = DataManager.instance.playerEquipViewModel;

            for (var index = 0; index < equipViewModel.Rights.Count; index++)
            {
                var weapon = equipViewModel.Rights[index];
                rightWeaponSlots[index].SetItem(weapon);
            }

            for (var index = 0; index < equipViewModel.Lefts.Count; index++)
            {
                var leftWeapon = equipViewModel.Lefts[index];
                leftWeaponSlots[index].SetItem(leftWeapon);
            }

            for (var index = 0; index < equipViewModel.Accessories.Count; index++)
            {
                var accessory = equipViewModel.Accessories[index];
                accessorySlots[index].SetItem(accessory);
            }

            for (var index = 0; index < equipViewModel.Tools.Count; index++)
            {
                var tool = equipViewModel.Tools[index];
                toolSlots[index].SetItem(tool);
            }
        }
        
        private void UpdateBySelectedSlotData(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;
            
            var describeViewModel = DataManager.instance.selectedUiViewModel;
            var itemSlot = describeViewModel.selectedItemSlotData;
            
            if(itemSlot == null) return;
            
            if (slotNameText)
                slotNameText.text = itemSlot.GetSlotName();

            if (itemNameText)
            {
                var item = itemSlot.GetItem();
                if (item.IsNullOrEmpty())
                {
                    itemNameText.text = "-";
                }
                else
                {
                    itemNameText.text =  item.GetItemName();
                }
            }
        }

        private void InitializeItemSlotButton(Button itemSlot)
        {
            if (itemSlot is not EquipmentItemSlot equipmentItemSlot)
            {
                Debug.LogError("EquipmentItemSlot을 사용하세요");
            }
            else
            {
                itemSlot.onClick.AddListener(() =>
                {
                    equipInventoryView.SetTarget(equipmentItemSlot);
                    equipInventoryView.Push();
                });
            }
        }

        public override void Travel(Action<UIContainerEntity> action)
        {
            base.Travel(action);
            equipInventoryView.Travel(action);
        }
    }
}