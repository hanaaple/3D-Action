using System;
using Data.Item.Base;
using Data.Item.Scriptable;
using UI.View.Entity;
using Util;

namespace Data.Item.Data
{
    /// <summary>
    /// 악세사리
    /// </summary>
    [Serializable]
    public class Accessory : BaseEquipItem
    {
        [NonSerialized] private AccessoryStaticData _accessoryStaticData;
        public override ItemType itemType => ItemType.Accessory;
        public override EquipType equipType => EquipType.Accessory;
        public override DescribeViewType describeType => DescribeViewType.Accessory;
        
        public override string GetItemDetailType()
        {
            return _accessoryStaticData.accessoryType.ToString();
        }

        public int enhancementValue;

        private Accessory(AccessoryStaticData accessoryStaticData)
        {
            _accessoryStaticData = accessoryStaticData;
            Initialize();
        }
        
        public override ItemStaticData GetItemData()
        {
            return GetAccessoryData();
        }
        
        public AccessoryStaticData GetAccessoryData()
        {
            if (!string.IsNullOrEmpty(id) && _accessoryStaticData == null)
            {
                // 안전한 커플링으로 풀업 X
                _accessoryStaticData = ScriptableObjectManager.instance.GetScriptableObjectById(id) as AccessoryStaticData;
            }
            
            return _accessoryStaticData;
        }
        
        public override void SetItemData(ItemStaticData itemStaticData)
        {
            _accessoryStaticData = itemStaticData as AccessoryStaticData;
        }

        public override string GetItemDisplayName()
        {
            var itemData = GetItemData();
            return itemData != null ? $"{itemData.itemName}+{enhancementValue}" : "-";
        }

        public override BaseItem Clone()
        {
            var item = new Accessory(_accessoryStaticData)
            {
                enhancementValue = enhancementValue
            };

            return item;
        }

        public override bool GetIsDuplicable()
        {
            return true;
        }
        
    }
}