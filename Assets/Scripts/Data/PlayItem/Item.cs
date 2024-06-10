using System;
using Data.Static.Scriptable;
using UnityEngine;

namespace Data.PlayItem
{
    /// <summary>
    /// 아이템 데이터
    /// 상속하여 사용
    /// </summary>
    [Serializable]
    public abstract class Item
    {
        // For Debug
        [HideInInspector] public string scriptableObjectName;
        [SerializeField] protected string id;

        public void Initialize()
        {
            scriptableObjectName = GetItemData()?.name;
        }
        
        public abstract ItemData GetItemData();

        public virtual void SetItemData(ItemData itemData)
        {
            id = itemData.id;
        }

        // 수식어 + 이름 + 강화 수치
        public virtual string GetItemName()
        {
            return GetItemData()?.itemName;
        }

        public abstract Item Clone();
    }

    public static class ItemExtensions
    {
        // Inspector View에 노출되며 자동으로 인스턴스화 될 수도 있다.
        public static bool IsNullOrEmpty(this Item item)
        {
            return item == null || item.GetItemData() == null;
        }
    }
}