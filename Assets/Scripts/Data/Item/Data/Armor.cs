using System;
using Data.Item.Base;
using Data.Item.Scriptable;
using UI.View.Entity;
using Util;

namespace Data.Item.Data
{
    /// <summary>
    /// 방어구
    /// </summary>
    [Serializable]
    public class Armor : BaseEquipItem
    {
        [NonSerialized] private ArmorStaticData _armorStaticData;
        public override ItemType itemType => ItemType.Armor;

        public override DescribeViewType describeType => DescribeViewType.Armor;

        public override EquipType equipType
        {
            get
            {
                return _armorStaticData.armorPart switch
                {
                    ArmorPart.Breastplate => EquipType.BreastPlate,
                    ArmorPart.Helmet => EquipType.Helmet,
                    ArmorPart.Leggings => EquipType.Leggings,
                    ArmorPart.Shoes => EquipType.Shoes,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private Armor(ArmorStaticData armorStaticData)
        {
            _armorStaticData = armorStaticData;
            Initialize();
        }
        
        public override string GetItemDetailType()
        {
            return _armorStaticData.armorType.ToString();
        }

        public ArmorStaticData GetArmorData()
        {
            return _armorStaticData;
        }
        
        public override ItemStaticData GetItemData()
        {
            if (!string.IsNullOrEmpty(id) && _armorStaticData == null)
            {
                // 안전한 커플링으로 풀업 X
                _armorStaticData = ScriptableObjectManager.instance.GetScriptableObjectById(id) as ArmorStaticData;
            }

            return _armorStaticData;
        }

        // 루팅의 경우 -> SetItemData를 해줌
        // 루팅이 아닌 경우, 그 외 어떻게든 SetItemData를 안해준 경우 -> 
        
        public override void SetItemData(ItemStaticData itemStaticData)
        {
            _armorStaticData = itemStaticData as ArmorStaticData;
        }
        
        public override BaseItem Clone()
        {
            var item = new Armor( _armorStaticData);

            return item;
        }

        public override bool GetIsDuplicable()
        {
            return true;
        }
    }
}