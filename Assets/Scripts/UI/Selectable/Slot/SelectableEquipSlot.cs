using UI.View.Entity;
using UnityEngine;
using Util;

namespace UI.Selectable.Slot
{
    public class SelectableEquipSlot : SelectableItemSlot
    {
        [SerializeField] private EquipSlotType equipSlotType;
        [SerializeField] private int slotIndexOffset;
        
        private string _slotName;
        private int _slotIndex;

        public int containerSlotIndex => _slotIndex + slotIndexOffset;

        public override DescribeViewType describeType
        {
            get
            {
                var equipmentType = equipSlotType.ConvertToEquipment();
                return equipmentType.ConvertToDescribeType();
            }
        }
        //public override EquipmentDetailType equipmentType => equipSlotType.ConvertToEquipmentDetail();
        
        public void Initialize(string slotName, int index, EquipSlotType equipSlot)
        {
            _slotName = slotName;
            _slotIndex = index;
            equipSlotType = equipSlot;
        }

        public override string GetItemName()
        {
            if (equipSlotType is EquipSlotType.Helmet or EquipSlotType.BreastPlate or EquipSlotType.Leggings
                or EquipSlotType.Shoes)
            {
                return $"{_slotName}";
            }
            else
            {
                return $"{_slotName} {_slotIndex + 1}";
            }
        }
    }
}