using System;

namespace Data.Item.Base
{
    public enum EquipType
    {
        Weapon,
        Helmet,
        BreastPlate,
        Leggings,
        Shoes,
        Accessory,
        Tool,
    }

    // 이걸 굳이 해야될까?
    [Serializable]
    public abstract class BaseEquipItem : BaseItem
    {
        public abstract EquipType equipType { get; }

        public override bool Equals(string comparison)
        {
            if (comparison == itemType.ToString())
                return true;
            
            if (comparison == equipType.ToString())
                return true;

            if (comparison == GetItemDetailType())
                return true;

            return false;
        }
    }
}