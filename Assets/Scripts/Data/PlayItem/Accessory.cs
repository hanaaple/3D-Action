using System;
using Data.Static.Scriptable;
using Util;

namespace Data.PlayItem
{
    [Serializable]
    public class Accessory : Item
    {
        [NonSerialized] private AccessoryData _accessoryData;

        public int enhancementValue;

        public override ItemData GetItemData()
        {
            if (!string.IsNullOrEmpty(id) && _accessoryData == null)
            {
                _accessoryData = ScriptableObjectManager.Instance.GetScriptableObjectById(id) as AccessoryData;
            }
            
            return _accessoryData;
        }
        
        public override void SetItemData(ItemData itemData)
        {
            base.SetItemData(itemData);
            _accessoryData = itemData as AccessoryData;
        }

        public override string GetItemName()
        {
            return GetItemData() ? $"{GetItemData().itemName}+{enhancementValue}" : "";
        }

        public override Item Clone()
        {
            var item = new Accessory
            {
                scriptableObjectName = scriptableObjectName,
                id = id,
                _accessoryData = _accessoryData,
                enhancementValue = enhancementValue
            };

            return item;
        }
    }
}