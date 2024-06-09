using System;
using Data.Static.Scriptable;
using Util;

namespace Data.PlayItem
{
    [Serializable]
    public class Weapon : Item
    {
        // 기본 데이터
        [NonSerialized] private WeaponData _weaponData;

        // 추가 데이터
        public int enhancementValue;
        // 인챈트 상태
        
        public override ItemData GetItemData()
        {
            if (!string.IsNullOrEmpty(id) && _weaponData == null)
            {
                _weaponData = ScriptableObjectManager.Instance.GetScriptableObjectById(id) as WeaponData;
            }
            
            return _weaponData;
        }

        public override void SetItemData(ItemData itemData)
        {
            base.SetItemData(itemData);
            _weaponData = itemData as WeaponData;
        }

        public override string GetItemName()
        {
            return GetItemData() ? $"{GetItemData().itemName}+{enhancementValue}" : "";
        }
        
        public override Item Clone()
        {
            var item = new Weapon
            {
                scriptableObjectName = scriptableObjectName,
                id = id,
                _weaponData = _weaponData,
                enhancementValue = enhancementValue,
            };

            return item;
        }
    }
}