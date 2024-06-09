using System;
using Data.Static.Scriptable;
using Util;

namespace Data.PlayItem
{
    public enum ToolType
    {
        Expendables,
        Reusable,
    }
    
    [Serializable]
    public class Tool : Item
    {
        [NonSerialized] private ToolData _toolData;

        public int enhancementValue;
        public int possessionCount;
        public int maximumPossessionCount;

        public ToolType toolType;

        public override ItemData GetItemData()
        {
            if (!string.IsNullOrEmpty(id) && _toolData == null)
            {
                _toolData = ScriptableObjectManager.Instance.GetScriptableObjectById(id) as ToolData;
            }

            return _toolData;
        }
        
        public override void SetItemData(ItemData itemData)
        {
            base.SetItemData(itemData);
            _toolData = itemData as ToolData;
        }

        public override string GetItemName()
        {
            return GetItemData() ? $"{GetItemData().itemName}+{enhancementValue}" : "";
        }
        
        public override Item Clone()
        {
            var item = new Tool
            {
                scriptableObjectName = scriptableObjectName,
                id = id,
                _toolData = _toolData,
                enhancementValue = enhancementValue,
                possessionCount = possessionCount,
                maximumPossessionCount = maximumPossessionCount,
                toolType = toolType
            };

            return item;
        }
    }
}