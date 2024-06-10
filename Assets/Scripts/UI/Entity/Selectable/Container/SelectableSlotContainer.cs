using System.Collections.Generic;
using System.Linq;
using Data.PlayItem;
using UI.Entity.Selectable.Slot;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Entity.Selectable.Container
{
    /// <summary>
    /// Selectable Slot을 관리한다.
    /// </summary>
    public class SelectableSlotContainer : MonoBehaviour
    {
        [FormerlySerializedAs("selectableSlot")]
        public List<SelectableSlot> selectableSlots;

        private SelectableSlot _selectedSlot;

        // Slot Capacity가 부족해지면 슬롯 수 업데이트, Navigation Update

        private void Awake()
        {
            Initialize();
        }

        public void Select()
        {
            _selectedSlot.Select();
        }

        public void SelectDefault()
        {
            if (selectableSlots.Count > 0)
                selectableSlots[0].Select();
        }

        private void Initialize()
        {
            foreach (var selectableButton in selectableSlots)
            {
                selectableButton.SelectAction += Select;
            }
        }

        public void Decision()
        {
            _selectedSlot.Click();
        }

        private void Select(SelectableSlot slot)
        {
            _selectedSlot = slot;
        }

        public void UpdateItems(List<Item> items)
        {
            if (selectableSlots.Count < items.Count)
            {
                ExpandSlot();
            }

            for (var i = 0; i < items.Count; i++)
            {
                var slot = selectableSlots[i] as SelectableItemSlot;
                var item = items[i];

                if (slot != null)
                {
                    slot.SetItem(item);
                }
                else
                {
                    Debug.LogError("인벤토리에 Item Slot이 아닌 Slot이 있음");
                }
            }
        }

        public void UpdateCheck(Item[] items)
        {
            foreach (var slot in selectableSlots)
            {
                if (slot is SelectableItemSlot itemSlot)
                {
                    // 장착 여부 따라 Check
                    if (!itemSlot.GetItem().IsNullOrEmpty() && items.Contains(itemSlot.GetItem()))
                    {
                        itemSlot.Check(CheckType.Select);

                        // Select Item 여부에 따라 추가 Check
                        // itemSlot.Check(CheckType.Other);
                    }
                    else
                    {
                        itemSlot.Check(CheckType.None);
                    }
                }
            }
        }

        private void ExpandSlot()
        {
            Debug.LogError("확장시켜야댐");
        }
    }
}