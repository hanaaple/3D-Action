using Data.Item.Base;
using UI.Selectable.Container.Item;
using UI.View.Entity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace UI.Selectable.Slot
{
    public enum CheckType
    {
        Select,
        Other,
        None
    }

    // 장비 종류
    public enum EquipmentType
    {
        Weapon,
        Armor,
        Accessory,
        Tool
    }
    
    // 장비 세부 종류
    public enum EquipmentDetailType
    {
        Weapon,
        Helmet,
        BreastPlate,
        Leggings,
        Shoes,
        Accessory,
        Tool
    }
    
    // 장비 슬롯 종류
    public enum EquipSlotType
    {
        LeftWeapon,
        RightWeapon,
        Helmet,
        BreastPlate,
        Leggings,
        Shoes,
        Accessory,
        Tool
    }
    
    // 각 슬롯이 비어있다면 그것들은 모두 동일하다.
    // 단 메서드 등은 다 다르다.

    // 슬롯 타입은 언제든 변경 가능하도록 
    
    
    /// <summary>
    /// Selectable인 Item Slot UI
    /// Container 내부의 Slot으로 Item을 담을 수 있다.
    /// </summary>
    public abstract class SelectableItemSlot : SelectableSlot
    {
        public DescribeViewType DescribeType;

        public virtual DescribeViewType describeType => DescribeType;

        //public abstract EquipmentType equipmentType { get; }
        private EquipContainerType _equipContainerType;
        
        [SerializeField] private Image icon;
        
        private BaseItem _baseItem;
        
        public abstract string GetItemName();
        
        public override bool Select()
        {
            //Debug.Log($"Select!  {gameObject.name}");
            var selected = base.Select();
            if(selected)
                PrimitiveUIManager.instance.selectedUiViewModel.selectedItemSlot = this;

            return selected;
        }
        
        public virtual void SetItem(BaseItem item)
        {
            _baseItem = item;
            if (item.IsNullOrBare())
            {
                icon.sprite = null;
                icon.enabled = false;
            }
            else
            {
                icon.sprite = item.GetItemData().slotSprite;
                icon.enabled = true;
            }
        }
        
        public BaseItem GetItem()
        {
            return _baseItem;
        }

        public T GetItem<T>() where T : BaseItem
        {
            return _baseItem as T;
        }
    }
}