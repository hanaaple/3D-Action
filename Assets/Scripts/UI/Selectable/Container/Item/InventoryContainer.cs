using System;
using Data.Item.Base;
using Data.ViewModel;
using UI.Selectable.Slot;
using UI.View.Entity;
using Util;

namespace UI.Selectable.Container.Item
{
    [Flags]
    public enum ItemContainerType
    {
        Weapon = 1 << 0,
        Accessory = 1 << 1,
        Tool = 1 << 2,
        Helmet = 1 << 3,
        BreastPlate = 1 << 4,
        Leggings = 1 << 5,
        Shoes = 1 << 6,
    }
    
    // Npc 판매, 강화 
    public class InventoryContainer : BaseItemContainer
    {
        // 인벤토리이다.
        // Npc 판매, 구매 및 강화
        // 인벤, 판매 - 동일
        // 이걸 클래스로 하여금 하고 Equals에 맞게 하도록?
        private ItemContainerType _itemContainerType;
        
        public void Initialize(ItemContainerType containerType)
        {
            _itemContainerType = containerType;
            Initialize();
        }
        
        public override Enum GetContainerEnumValue()
        {
            return _itemContainerType;
        }
        
        // item.DescribeType = 현재 Container의 DescribeView

        protected override string GetItemDivideType(BaseItem item)
        {
            // DivideType은 ItemContainerType에 따라
            return base.GetItemDivideType(item);
        }

        public override void UpdateCheck(EquipData equipData, bool isEquipped)
        {         
            var slot = FindSlot(equipData.Item);
            
            // Inventory -> 장착 중인 경우 무조건 체크
            if (isEquipped)
            {
                slot.Check(CheckType.Select);
            }
            else
            {
                slot.Check(CheckType.None);
            }
        }

        public override string GetContainerName()
        {
            // 모든아이템, 도구, 아이템 제작 소재, 강화소재, 귀중품, 근접 무기, 투구, 흉갑, 각반 등
            // _itemContainerType ->
            if (_itemContainerType == (ItemContainerType)(~0))
            {
                return "모든 아이템 (임시)";
            }
            return $"{_itemContainerType.ToString()} (임시) - 변환 필요";
        }
       
    }
}