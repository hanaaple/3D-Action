using System;
using Data.Item.Base;
using Data.Item.Scriptable;
using Util;

namespace Data.Item.Data
{
    /// <summary>
    /// 귀중품
    /// </summary>
    // [Serializable]
    // public class Valuable : BaseItem
    // {
    //     [NonSerialized] private ValuableStaticData _valuableStaticData;
    //     public override ItemType itemType => ItemType.Valuable;
    //     
    //     public override string GetItemDetailType()
    //     {
    //         return _valuableStaticData.valuableType.ToString();
    //     }
    //
    //     // 보유 개수
    //     
    //     private Valuable(ValuableStaticData valuableStaticData)
    //     {
    //         _valuableStaticData = valuableStaticData;
    //         Initialize();
    //     }
    //     
    //     public override ItemStaticData GetItemData()
    //     {
    //         if (!string.IsNullOrEmpty(id) && _valuableStaticData == null)
    //         {
    //             // 안전한 커플링으로 풀업 X
    //             _valuableStaticData = ScriptableObjectManager.instance.GetScriptableObjectById(id) as ValuableStaticData;
    //         }
    //
    //         return _valuableStaticData;
    //     }
    //     
    //     public override void SetItemData(ItemStaticData itemStaticData)
    //     {
    //         _valuableStaticData = itemStaticData as ValuableStaticData;
    //     }
    //     
    //     public override BaseItem Clone()
    //     {
    //         var item = new Valuable(_valuableStaticData);
    //
    //         return item;
    //     }
    //
    //     public override bool GetIsDuplicable()
    //     {
    //         return false;
    //     }
    // }
}