using System;
using Data.Item.Base;
using UI.Selectable.Container.Item;
using UI.View.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Selectable.Slot
{
    public class SelectableInventorySlot : SelectableItemSlot
    {
        //private EquipmentDetailType _equipmentDetailType;
        [SerializeField] private Image checkIcon;
        
        [SerializeField] private Color selectColor;
        [SerializeField] private Color otherColor;

        //public override EquipmentDetailType equipmentType => _equipmentDetailType;

        // public void Initialize(EquipmentDetailType equipSlot)
        // {
        //     _equipmentDetailType = equipSlot;
        // }
        public override void Initialize(Action<SelectableSlot> onBeginSelect)
        {
            base.Initialize(onBeginSelect);
            // 그냥 Container를 알고 있으면 바로 가면 되는데 -> 의존 역전이 발생하자늠
        }

        public override void SetItem(BaseItem item)
        {
            base.SetItem(item);
            //Debug.LogWarning($"Set Item {gameObject.name}");
        }

        public void Check(CheckType checkType)
        {
            if (!checkIcon) return;

            if (checkType == CheckType.Select)
            {
                checkIcon.color = selectColor;
            }
            else if (checkType == CheckType.Other)
            {
                checkIcon.color = otherColor;
            }
            else if (checkType == CheckType.None)
            {
                var color = checkIcon.color;
                color.a = 0;
                checkIcon.color = color;
            }
        }

        public override string GetItemName()
        {
            var item = GetItem();
            if (item.IsNullOrBare()) return "-";

            return item.GetItemDisplayName();
        }
    }
}