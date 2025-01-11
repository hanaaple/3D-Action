using System;
using Data.Item.Base;
using Data.Item.Scriptable;
using UI.View.Entity;
using Util;

namespace Data.Item.Data
{
    public enum ToolType
    {
        Expendables,
        Reusable,
    }
    
    /// <summary>
    /// 도구
    /// </summary>
    [Serializable]
    public class Tool : BaseEquipItem
    {
        [NonSerialized] private ToolStaticData _toolStaticData;
        public override ItemType itemType => ItemType.Tool;
        public override EquipType equipType => EquipType.Tool;
        public override DescribeViewType describeType => DescribeViewType.Tool;
        
        public override string GetItemDetailType()
        {
            return _toolStaticData.toolType.ToString();
        }

        public int enhancementValue;
        public int possessionCount;
        public int maximumPossessionCount;

        public ToolType toolType;

        private Tool(ToolStaticData toolStaticData)
        {
            _toolStaticData = toolStaticData;
            Initialize();
        }
        
        public override ItemStaticData GetItemData()
        {
            return GetToolData();
        }
        
        public ToolStaticData GetToolData()
        {
            if (!string.IsNullOrEmpty(id) && _toolStaticData == null)
            {
                // 안전한 커플링으로 풀업 X
                _toolStaticData = ScriptableObjectManager.instance.GetScriptableObjectById(id) as ToolStaticData;
            }

            return _toolStaticData;
        }
        
        public override void SetItemData(ItemStaticData itemStaticData)
        {
            _toolStaticData = itemStaticData as ToolStaticData;
        }

        public override string GetItemDisplayName()
        {
            var itemData = GetItemData();
            return itemData != null ? $"{itemData.itemName}+{enhancementValue}" : "-";
        }
        
        public override BaseItem Clone()
        {
            var item = new Tool(_toolStaticData)
            {
                enhancementValue = enhancementValue,
                possessionCount = possessionCount,
                maximumPossessionCount = maximumPossessionCount,
                toolType = toolType
            };

            return item;
        }

        public override bool GetIsDuplicable()
        {
            return ((ToolStaticData)GetItemData()).GetIsDuplicable();
        }

        // 조금 더 생각해보기
        public override void TryAddCount()
        {
            if (toolType == ToolType.Expendables)
            {
                possessionCount++;
            }
            else if (toolType == ToolType.Reusable)
            {
                
            }
        }
    }
}