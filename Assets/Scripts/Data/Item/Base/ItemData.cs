using System;
using UnityEngine;

namespace Data.Item.Base
{
    // 아이템을 주든 팔든 이런식으로 세팅해놓은 걸 사용.
    [Serializable]
    public class ItemData<T, TData> where T : BaseItem where TData : ItemStaticData
    {
        [SerializeField] private T item;
        public TData itemStaticData;

        public T GetItem()
        {
            if (item.GetItemData() == null)
            {
                item.SetItemData(itemStaticData);
            }

            return item;
        }
    }
}