using System;
using System.Collections.Generic;
using Data.Item.Base;
using UnityEngine;

namespace Data.Play
{
    /// <summary>
    /// 보유 중인 장비 데이터
    /// </summary>
    [Serializable]
    public struct OwnedItemData
    {
        // Tool - 소모품 or 재사용 가능 등에 따라 중복 허가
        // Accessory - 중복 불허
        // Valuable - 중복 불허
        // Armor - 중복 불허
        // Weapon - 중복 불허
        
        // TODO 
        // 아이템은 ItemType -> EquipType (????) -> DetailType 순으로 세분화된다.
        // 이에 따라 데이터를 나누면 될 것이다. (세분화 되는 것이 확정이 아니므로 임시로 List로 구현)
        public List<BaseItem> Items;
        
        public void Initialize()
        {
            Items = new List<BaseItem>();
        }
        
        /// <summary>
        /// 추가하는 아이템은 복사 후 가져올 것
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(BaseItem item)
        {
            // 아이템이 있으며 중복이 불가능하면 개수 추가 시도
            if (!item.GetIsDuplicable() && TryGetItem(item.id, out var existItem))
            {
                existItem.TryAddCount();
            }
            // 아이템이 없거나, 중복이 가능하면 Instance 추가
            else
            {
                Debug.Log($"Add Item\n{item.GetItemDisplayData()}");
                Items.Add(item);
            }
        }

        public void RemoveItem(BaseItem item)
        {
            Items.Remove(item);
        }

        private bool TryGetItem(string itemId, out BaseItem existItem)
        {
            existItem = Items.Find(item => item.id == itemId);

            return existItem != null;
        }
    }
}