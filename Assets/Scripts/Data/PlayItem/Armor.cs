using System;
using Data.Static.Scriptable;
using Util;

namespace Data.PlayItem
{
    [Serializable]
    public class Armor : Item
    {
        [NonSerialized] private ArmorData _armorData;

        public override ItemData GetItemData()
        {
            if (!string.IsNullOrEmpty(id) && _armorData == null)
            {
                _armorData = ScriptableObjectManager.Instance.GetScriptableObjectById(id) as ArmorData;
            }

            return _armorData;
        }
        
        public override void SetItemData(ItemData itemData)
        {
            base.SetItemData(itemData);
            _armorData = itemData as ArmorData;
        }
        
        public override Item Clone()
        {
            var item = new Armor
            {
                scriptableObjectName = scriptableObjectName,
                id = id,
                _armorData = _armorData,
            };

            return item;
        }
    }
}