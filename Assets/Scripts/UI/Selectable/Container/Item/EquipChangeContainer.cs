using System;
using System.Collections.Generic;
using Data.ViewModel;
using UI.Selectable.Slot;
using UI.View.Entity;
using UI.View.Inventory;
using UnityEngine;
using Util;

namespace UI.Selectable.Container.Item
{
    // Container Type을 Container마다 따로 할 이유는 없지만, 그렇다고 따로 하지 않을 이유도 없음.
    // (Enum을 1개로 정리하고 어쩌고 저쩌고 하면 그럴 이유가 되긴 하는데, 해당 책임이 너무 커짐.)
    public enum EquipContainerType
    {
        Weapon,
        Helmet,
        BreastPlate,
        Leggings,
        Shoes,
        Accessory,
        Tool,
    }

    public struct ContainerData
    {
        public int StartIndex;
        public int Length;
        public int EndIndex => Length + StartIndex - 1;
        public EquipSlotType EquipSlotType;
    }

    public enum IndexOrder
    {
        First,
        Last
    }

    /// <summary>
    /// Equip -> Change Inventory의 각 Container
    /// </summary>
    public class EquipChangeContainer : BaseItemContainer
    {
        // 각 컨테이너마다 Index를 갖는 것은 효율적이진 않지만
        // 객체에 접근하는 비용 + 4byte를 생각하면 매우 작은 수치이다. -> 그럼 어떻게 구현하려 했는데? 기억 안나네
        private List<ContainerData> _containerData;
        
        // _containerData의 Index
        private int _containerDataIndex;
        
        // 전체 Index
        private int _totalIndex;
        
        private EquipContainerType _equipContainerType;
        private Action<EquipChangeContainer, SelectableSlot> _onAddSlot;
        
        // 전체 Count
        private int IndexingCount => _containerData.Count > 0 ? _containerData[^1].EndIndex + 1 : 0;
        /// <summary>
        /// 0 ~ 5 -> 0 ~ 2, 0 ~ 2
        /// </summary>
        public int CurrentEquipIndex => _totalIndex - _containerData[_containerDataIndex].StartIndex;
        public EquipSlotType CurrentEquipSlotType => _containerData[_containerDataIndex].EquipSlotType;
        
        public override Enum GetContainerEnumValue()
        {
            return _equipContainerType;
        }

        public void Initialize(EquipContainerType containerType, Action<EquipChangeContainer, SelectableSlot> onAddSlot)
        {
            _containerData = new List<ContainerData>();
            _onAddSlot = onAddSlot;
            _equipContainerType = containerType;
            
            Initialize();
        }

        protected override void OnAddSelectableSlot(SelectableSlot slot)
        {
            base.OnAddSelectableSlot(slot);
            _onAddSlot(this, slot);
            
            var itemSlot = slot as SelectableItemSlot;
            itemSlot.DescribeType = _equipContainerType.ConvertToDescribeViewType();
        }

        public void AddData(EquipSlotType equipSlotType, int length)
        {
            var data = new ContainerData
            {
                EquipSlotType = equipSlotType,
                Length = length,
                StartIndex = _containerData.Count > 0 ? _containerData[^1].EndIndex + 1 : 0
            };
            
            _containerData.Add(data);
        }

        public void SetIndex(int index)
        {
            _totalIndex = index;
            for (var i = 0; i < _containerData.Count; i++)
            {
                var containerData = _containerData[i];
                if (containerData.EndIndex >= _totalIndex)
                {
                    _containerDataIndex = i;
                    break;
                }
            }
        }
        
        public void SetIndex(IndexOrder indexOrder)
        {
            if (indexOrder == IndexOrder.First)
            {
                SetIndex(_containerData[0].StartIndex);
            }
            else if (indexOrder == IndexOrder.Last)
            {
                SetIndex(IndexingCount - 1);
            }
            else
            {
                throw new ArgumentOutOfRangeException("indexOrder");
            }
        }

        /// <returns> if indexing is over, return true </returns>
        public bool IndexingContainer(IndexingDirection indexingDirection)
        {
            var containerData = _containerData[_containerDataIndex];
            var index = _totalIndex;

            index = indexingDirection switch
            {
                IndexingDirection.Previous => index - 1,
                IndexingDirection.Next => index + 1,
                _ => throw new ArgumentOutOfRangeException()
            };

            // OverFlow
            if (index < 0 || index >= IndexingCount)
            {
                return true;
            }

            int containerDataIndex = _containerDataIndex;

            
            // OverFlow (ex - Left -> Right) 
            if (index < containerData.StartIndex || index > containerData.EndIndex)
            {
                containerDataIndex = indexingDirection switch
                {
                    IndexingDirection.Previous => containerDataIndex - 1,
                    IndexingDirection.Next => containerDataIndex + 1,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            Debug.Log($"{_containerDataIndex}, {_totalIndex} -> {containerDataIndex}, {index}");

            SetIndex(index);

            return false;
        }
        
        public override void UpdateCheck(EquipData equipData, bool isEquipped)
        {           
            var slot = FindSlot(equipData.Item);
            if (isEquipped)
            {
                // Equip -> 장착 중 + Equip 슬롯에 따라 체크
                if (equipData.Index == CurrentEquipIndex)
                {
                    slot.Check(CheckType.Select);    
                }
                else
                {
                    slot.Check(CheckType.Other);
                }
            }
            else
            {
                slot.Check(CheckType.None);
            }
        }

        public override string GetContainerName()
        {
            if (IndexingCount == 1)
                return $"{CurrentEquipSlotType.ConvertToDisplayName()}";
            
            // 0 ~ 6이 아니라 0 ~ 2, 0 ~ 2
            return $"{CurrentEquipSlotType.ConvertToDisplayName()} {CurrentEquipIndex + 1}";
        }
    }
}