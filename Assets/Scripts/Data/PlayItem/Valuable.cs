using System;
using Data.Static.Scriptable;
using Util;

namespace Data.PlayItem
{
    [Serializable]
    public class Valuable : Item
    {
        [NonSerialized] private ValuableData _valuableData;
        
        // 보유 개수
        
        public override ItemData GetItemData()
        {
            if (!string.IsNullOrEmpty(id) && _valuableData == null)
            {
                _valuableData = ScriptableObjectManager.Instance.GetScriptableObjectById(id) as ValuableData;
            }

            return _valuableData;
        }
        
        public override void SetItemData(ItemData itemData)
        {
            base.SetItemData(itemData);
            _valuableData = itemData as ValuableData;
        }
        
        public override Item Clone()
        {
            var item = new Valuable
            {
                scriptableObjectName = scriptableObjectName,
                id = id,
                _valuableData = _valuableData
            };

            return item;
        }
    }
}