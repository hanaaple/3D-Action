using Data;
using Data.Play;
using Data.PlayItem;
using UI.Entity.Selectable.Slot;
using UI.View.Inventory;
using UnityEngine;

namespace UI.View.Equip
{
    public class EquipInventoryView : BaseInventoryView
    {
        private EquipmentItemSlot _targetEquipSlot;

        protected override void Start()
        {
            base.Start();

            foreach (var container in selectableSlotContainers)
            {
                foreach (var containerSelectableSlot in container.selectableSlots)
                {
                    containerSelectableSlot.onClick.AddListener(() =>
                    {
                        var itemSlot = containerSelectableSlot as SelectableItemSlot;
                        if (itemSlot == null)
                        {
                            Debug.LogError("Item Slot이 아님");
                        }
                        else
                        {
                            var item = itemSlot.GetItem();

                            if (itemSlot.equipmentType == EquipmentType.Weapon)
                            {
                                Weapon weapon = null;
                                if (item is Weapon || item.IsNullOrEmpty())
                                    weapon = item as Weapon;
                                else
                                {
                                    Debug.LogError(
                                        $"잘못된 아이템을 넣음 Container: {itemSlot.equipmentType}, SlotType: {_targetEquipSlot.slotType} Item:{item.GetItemData()}");
                                }

                                if (_targetEquipSlot.slotType == EquipSlotType.RightWeapon)
                                    DataManager.instance.playerEquipViewModel.SetWeapon(
                                        _targetEquipSlot.equippedItemIndex, weapon, WeaponEquipType.Right);
                                else if (_targetEquipSlot.slotType == EquipSlotType.LeftWeapon)
                                    DataManager.instance.playerEquipViewModel.SetWeapon(
                                        _targetEquipSlot.equippedItemIndex, weapon, WeaponEquipType.Left);
                                else
                                {
                                    Debug.LogError(
                                        $"{itemSlot.equipmentType} Container에서 {_targetEquipSlot.slotType} Slot을 누름");
                                }
                            }
                            else if (itemSlot.equipmentType == EquipmentType.Armor)
                            {
                                Armor armor = null;
                                if (item is Armor || item.IsNullOrEmpty())
                                    armor = item as Armor;
                                else
                                {
                                    Debug.LogError(
                                        $"잘못된 아이템을 넣음 Container: {itemSlot.equipmentType}, SlotType: {_targetEquipSlot.slotType} Item:{item.GetItemData()}");
                                }

                                if (_targetEquipSlot.slotType == EquipSlotType.Helmet)
                                    DataManager.instance.playerEquipViewModel.SetHelmet(armor);
                                else if (_targetEquipSlot.slotType == EquipSlotType.BreastPlate)
                                    DataManager.instance.playerEquipViewModel.SetBreastPlate(armor);
                                else if (_targetEquipSlot.slotType == EquipSlotType.Leggings)
                                    DataManager.instance.playerEquipViewModel.SetLeggings(armor);
                                else if (_targetEquipSlot.slotType == EquipSlotType.Shoes)
                                    DataManager.instance.playerEquipViewModel.SetShoes(armor);
                                else
                                {
                                    Debug.LogError(
                                        $"{itemSlot.equipmentType} Container에서 {_targetEquipSlot.slotType} Slot을 누름");
                                }
                            }
                            else if (itemSlot.equipmentType == EquipmentType.Accessory)
                            {
                                Accessory accessory = null;
                                if (item is Accessory || item.IsNullOrEmpty())
                                    accessory = item as Accessory;
                                else
                                {
                                    Debug.LogError(
                                        $"잘못된 아이템을 넣음 Container: {itemSlot.equipmentType}, SlotType: {_targetEquipSlot.slotType} Item:{item.GetItemData()}");
                                }

                                if (_targetEquipSlot.slotType == EquipSlotType.Accessory)
                                    DataManager.instance.playerEquipViewModel.SetAccessory(
                                        _targetEquipSlot.equippedItemIndex, accessory);
                                else
                                {
                                    Debug.LogError(
                                        $"{itemSlot.equipmentType} Container에서 {_targetEquipSlot.slotType} Slot을 누름");
                                }
                            }
                            else if (itemSlot.equipmentType == EquipmentType.Tool)
                            {
                                Tool tool = null;
                                if (item is Tool || item.IsNullOrEmpty())
                                    tool = item as Tool;
                                else
                                {
                                    Debug.LogError(
                                        $"잘못된 아이템을 넣음 Container: {itemSlot.equipmentType}, SlotType: {_targetEquipSlot.slotType} Item:{item.GetItemData()}");
                                }

                                if (_targetEquipSlot.slotType == EquipSlotType.Tool)
                                    DataManager.instance.playerEquipViewModel.SetTool(
                                        _targetEquipSlot.equippedItemIndex, tool);
                                else
                                {
                                    Debug.LogError(
                                        $"{itemSlot.equipmentType} Container에서 {_targetEquipSlot.slotType} Slot을 누름");
                                }
                            }
                        }

                        Pop();
                    });
                }
            }
        }

        public void SetTarget(EquipmentItemSlot itemSlot)
        {
            _targetEquipSlot = itemSlot;
        }
    }
}