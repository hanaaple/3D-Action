using System;
using Data.Item.Base;
using Data.Item.Scriptable;
using UI.View.Entity;
using UnityEngine;
using Util;

namespace Data.Item.Data
{
    /// <summary>
    /// 무기
    /// </summary>
    [Serializable]
    public class Weapon : BaseEquipItem
    {
        // 기본 데이터 (임시 디버깅용)
        [NonSerialized] public WeaponStaticData WeaponStaticData;
        public override ItemType itemType => ItemType.Weapon;
        public override EquipType equipType => EquipType.Weapon;
        public override DescribeViewType describeType => DescribeViewType.Weapon;
        
        // 추가 데이터
        public int enhancementValue;
        // 인챈트 상태
        
        private Weapon(WeaponStaticData weaponStaticData)
        {
            WeaponStaticData = weaponStaticData;
            Initialize();
        }

        public override ItemStaticData GetItemData()
        {
            return GetWeaponData();
        }

        public WeaponStaticData GetWeaponData()
        {
            if (WeaponStaticData == null && !string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("어디서 Item을 넣는가");
                // 안전한 커플링으로 풀업 X
                WeaponStaticData = ScriptableObjectManager.instance.GetScriptableObjectById(id) as WeaponStaticData;
            }
            
            return WeaponStaticData;
        }

        public override void SetItemData(ItemStaticData itemStaticData)
        {
            WeaponStaticData = itemStaticData as WeaponStaticData;
        }

        public override string GetItemDisplayName()
        {
            var itemData = GetItemData();
            return itemData != null ? $"{itemData.itemName}+{enhancementValue}" : "-";
        }
        
        public override BaseItem Clone()
        {
            var item = new Weapon(WeaponStaticData)
            {
                enhancementValue = enhancementValue,
            };

            return item;
        }

        public override bool GetIsDuplicable()
        {
            return true;
        }

        public override string GetItemDetailType()
        {
            return WeaponStaticData.weaponType.ToString();
        }
    }
}