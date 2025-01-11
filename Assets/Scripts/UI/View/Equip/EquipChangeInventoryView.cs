using System;
using System.Collections.Generic;
using Data.Item.Base;
using Manager;
using UI.Selectable.Container.Item;
using UI.Selectable.Slot;
using UI.View.Inventory;
using UnityEngine;

namespace UI.View.Equip
{
    /// <summary>
    /// 장비창에서 슬롯 클릭 시 Display되는 View
    /// 교체할 장비 종류의 인벤토리를 보여준다.
    /// Input도 되어있다.
    /// </summary>
    public class EquipChangeInventoryView : BaseItemContainerView
    {
        private EquipChangeContainer[] _inventoryContainers;
        
        private readonly Dictionary<EquipContainerType, EquipChangeContainer> _containerDictionary = new ();

        protected override BaseItemContainer[] GetItemContainers()
        {
            return _inventoryContainers;
        }

        // 각 Equip Container Type에 해당하는 Container를 Instantiate한다.
        public void Initialize()
        {
            //Debug.LogWarning("Initialize!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            
            // Container - Instantiate & Initialize
            var values = Enum.GetValues(typeof(EquipContainerType));
            _inventoryContainers = new EquipChangeContainer[values.Length];
            for (var index = 0; index < values.Length; index++)
            {
                EquipContainerType containerType = (EquipContainerType)values.GetValue(index);
                if (_containerDictionary.ContainsKey(containerType)) continue;
                
                var container = Instantiate(containerPrefab, containerParent).GetComponent<EquipChangeContainer>();
                container.Initialize(containerType, OnAddSlot);
                
                _containerDictionary.Add(containerType, container);
                _inventoryContainers[index] = container;
            }
        }

        public void SetContainerLength(EquipSlotType equipSlotType, int length)
        {
            var containerType = ConvertToContainerType(equipSlotType);
            var container = _containerDictionary[containerType];
            container.AddData(equipSlotType, length);
        }

        /// <summary>
        /// SetTarget & Push 순서대로 해야되는 것에서 의존성이 발생함.
        /// </summary>
        public void SetTarget(EquipContainerType containerType, int slotIndex)
        {
            var targetContainer = _containerDictionary[containerType];
            ContainerIndex = Array.FindIndex(_inventoryContainers, item => item == targetContainer);
            targetContainer.SetIndex(slotIndex);
            containerNameText.text = _inventoryContainers[ContainerIndex].GetContainerName();
        }

        protected override void IndexingContainer(IndexingDirection indexingDirection)
        {
            int prevIndex = ContainerIndex;
            
            var container = _inventoryContainers[ContainerIndex];

            var indexingIsOver = container.IndexingContainer(indexingDirection);
            if (indexingIsOver)
            {
                base.IndexingContainer(indexingDirection);
                container = _inventoryContainers[ContainerIndex];
                if (indexingDirection == IndexingDirection.Next)
                {
                    container.SetIndex(IndexOrder.First);
                }
                else if (indexingDirection == IndexingDirection.Previous)
                {
                    container.SetIndex(IndexOrder.Last);
                }

                Debug.Log($"Container Change: {prevIndex} -> {ContainerIndex},    {_inventoryContainers[ContainerIndex].name}");
            }
            
            containerNameText.text = _inventoryContainers[ContainerIndex].GetContainerName();

            // TODO
            // Update UI SlotIndex (Weapon 1 -> Weapon 2)
            // UpdateUI();
        }
        
        private void OnAddSlot(EquipChangeContainer container, SelectableSlot slot)
        {
            slot.Clear();
            slot.AddListener(() =>
            {
                if (slot is not SelectableItemSlot itemSlot)
                {
                    Debug.LogError($"{slot.gameObject}는 Item Slot이 아님");
                    return;
                }

                var changeToItem = itemSlot.GetItem();
                if (changeToItem.IsNullOrBare()) return;

                EquipSlotType equipSlotType = container.CurrentEquipSlotType;
                int targetEquipIndex = container.CurrentEquipIndex;

                var playerEquipViewModel = PlaySceneManager.instance.playerDataManager.equipViewModel;
                BaseItem currentEquippedItem =
                    playerEquipViewModel.GetItem(equipSlotType, targetEquipIndex).Item;

                // 이미 장착 중인 아이템을 누른 경우
                if (currentEquippedItem == changeToItem)
                {
                    changeToItem = null;
                }

                
                //Debug.LogWarning($"{changeToItem.GetItemDisplayData()}");
                Debug.LogWarning($"Change Item  {changeToItem.IsNullOrEmpty()} {changeToItem.GetItemDisplayName()}");
                playerEquipViewModel.SetItem(changeToItem, equipSlotType, targetEquipIndex);

                Pop();
            });
        }
        
        private static EquipContainerType ConvertToContainerType(EquipSlotType equipSlotType)
        {
            return equipSlotType switch
            {
                EquipSlotType.LeftWeapon or EquipSlotType.RightWeapon => EquipContainerType.Weapon,
                EquipSlotType.Helmet => EquipContainerType.Helmet,
                EquipSlotType.BreastPlate => EquipContainerType.BreastPlate,
                EquipSlotType.Leggings => EquipContainerType.Leggings,
                EquipSlotType.Shoes => EquipContainerType.Shoes,
                EquipSlotType.Accessory => EquipContainerType.Accessory,
                EquipSlotType.Tool => EquipContainerType.Tool,
                _ => throw new ArgumentOutOfRangeException(nameof(equipSlotType), equipSlotType, null)
            };
        }
    }
}