using System.Collections.Generic;
using System.ComponentModel;
using Data;
using Data.PlayItem;
using UI.Entity.Base;
using UI.Entity.Selectable.Container;
using UI.Entity.Selectable.Slot;
using UnityEngine;

namespace UI.View.Inventory
{
    public abstract class BaseInventoryView : UIContainerEntity
    {
        // Container Slide
        
        [SerializeField] private GameObject describeViewPanel;
        [SerializeField] private GameObject characterDataViewPanel;
        
        protected virtual void Awake()
        {
            DataManager.instance.playerOwnedItemViewModel.PropertyChanged += UpdateInventoryView;
            DataManager.instance.playerEquipViewModel.PropertyChanged += UpdateEquippedSlotView;
        }

        protected virtual void Start()
        {
        }
        
        protected override void UpdateView()
        {
            UpdateEquippedSlotView(null, null);
            UpdateInventoryView(null, null);
        }

        private void UpdateEquippedSlotView(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;

            var equipViewModel = DataManager.instance.playerEquipViewModel;
            
            var allEquippedItems = equipViewModel.GetAllEquippedItems();
            
            foreach (var container in selectableSlotContainers)
            {
                container.UpdateCheck(allEquippedItems);
            }
        }

        private void UpdateInventoryView(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;
            
            var ownedItemViewModel = DataManager.instance.playerOwnedItemViewModel;

            foreach (var container in selectableSlotContainers)
            {
                if (container is not InventoryContainer itemContainerView)
                {
                    Debug.LogError($"{container.gameObject}가 SlotType이 없음");
                    continue;
                }

                List<Item> items = new List<Item>();

                if ((itemContainerView.slotType & EquipmentType.Weapon) == EquipmentType.Weapon)
                {
                    items.AddRange(ownedItemViewModel.GetAllWeapons());
                }

                if ((itemContainerView.slotType & EquipmentType.Armor) == EquipmentType.Armor)
                {
                    items.AddRange(ownedItemViewModel.GetAllArmors());
                }

                if ((itemContainerView.slotType & EquipmentType.Accessory) == EquipmentType.Accessory)
                {
                    items.AddRange(ownedItemViewModel.GetAllAccessories());
                }

                if ((itemContainerView.slotType & EquipmentType.Tool) == EquipmentType.Tool)
                {
                    items.AddRange(ownedItemViewModel.GetAllTools());
                }

                itemContainerView.UpdateItems(items);
            }
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
    }
}