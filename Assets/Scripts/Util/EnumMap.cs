using System.Collections.Generic;
using Data.Item.Scriptable;
using Data.Play;
using UI.Selectable.Container.Item;
using UI.Selectable.Slot;
using UI.View.Entity;

namespace Util
{
    // Csv나 Json으로 매칭해두는게 좋아보이기도
    // 생각보다 너무 많아짐. -> Enum을 너무 다양하게 사용하는 것을 막거나 다른 방법을 사용해야 될 것으로 보임.
    public static class EnumMap
    {
        private static readonly Dictionary<EquipContainerType, DescribeViewType> EquipContainerDescribeViewTypes =
            new(EnumComparer.For<EquipContainerType>())
            {
                { EquipContainerType.Weapon, DescribeViewType.Weapon }, { EquipContainerType.Helmet, DescribeViewType.Armor },
                { EquipContainerType.BreastPlate, DescribeViewType.Armor },
                { EquipContainerType.Leggings, DescribeViewType.Armor },
                { EquipContainerType.Shoes, DescribeViewType.Armor },
                { EquipContainerType.Accessory, DescribeViewType.Accessory }, { EquipContainerType.Tool, DescribeViewType.Tool }
            };
        
        private static readonly Dictionary<EquipmentType, DescribeViewType> DescribeViewTypes =
            new(EnumComparer.For<EquipmentType>())
            {
                { EquipmentType.Weapon, DescribeViewType.Weapon }, { EquipmentType.Armor, DescribeViewType.Armor },
                { EquipmentType.Accessory, DescribeViewType.Accessory }, { EquipmentType.Tool, DescribeViewType.Tool }
            };

        private static readonly Dictionary<EquipSlotType, string> EquipSlotDisplayNames =
            new(EnumComparer.For<EquipSlotType>())
            {
                { EquipSlotType.LeftWeapon, "왼손 무기" }, { EquipSlotType.RightWeapon, "오른손 무기" },
                { EquipSlotType.Helmet, "헬멧" }, { EquipSlotType.BreastPlate, "흉갑" },
                { EquipSlotType.Leggings, "각반" }, { EquipSlotType.Shoes, "신발" },
                { EquipSlotType.Accessory, "악세사리" },
                { EquipSlotType.Tool, "사용 아이템" }
            };

        private static readonly Dictionary<EquipSlotType, EquipmentType> EquipmentTypes =
            new(EnumComparer.For<EquipSlotType>())
            {
                { EquipSlotType.LeftWeapon, EquipmentType.Weapon }, { EquipSlotType.RightWeapon, EquipmentType.Weapon },
                { EquipSlotType.Helmet, EquipmentType.Armor }, { EquipSlotType.BreastPlate, EquipmentType.Armor },
                { EquipSlotType.Leggings, EquipmentType.Armor }, { EquipSlotType.Shoes, EquipmentType.Armor },
                { EquipSlotType.Accessory, EquipmentType.Accessory },
                { EquipSlotType.Tool, EquipmentType.Tool }
            };

        private static readonly Dictionary<EquipSlotType, EquipmentDetailType> EquipmentDetailTypes =
            new(EnumComparer.For<EquipSlotType>())
            {
                { EquipSlotType.LeftWeapon, EquipmentDetailType.Weapon },
                { EquipSlotType.RightWeapon, EquipmentDetailType.Weapon },
                { EquipSlotType.Helmet, EquipmentDetailType.Helmet },
                { EquipSlotType.BreastPlate, EquipmentDetailType.BreastPlate },
                { EquipSlotType.Leggings, EquipmentDetailType.Leggings },
                { EquipSlotType.Shoes, EquipmentDetailType.Shoes },
                { EquipSlotType.Accessory, EquipmentDetailType.Accessory },
                { EquipSlotType.Tool, EquipmentDetailType.Tool }
            };


        private static readonly Dictionary<EquipSlotType, WeaponEquipType> WeaponEquipTypes =
            new(EnumComparer.For<EquipSlotType>())
            {
                { EquipSlotType.LeftWeapon, WeaponEquipType.Left }, { EquipSlotType.RightWeapon, WeaponEquipType.Right }
            };

        private static readonly Dictionary<EquipSlotType, ArmorPart> ArmorEquipTypes =
            new(EnumComparer.For<EquipSlotType>())
            {
                { EquipSlotType.Helmet, ArmorPart.Helmet }, { EquipSlotType.BreastPlate, ArmorPart.Breastplate },
                { EquipSlotType.Shoes, ArmorPart.Shoes }, { EquipSlotType.Leggings, ArmorPart.Leggings }
            };

        public static EquipmentType ConvertToEquipment(this EquipSlotType op)
        {
            return EquipmentTypes[op];
        }

        public static EquipmentDetailType ConvertToEquipmentDetail(this EquipSlotType op)
        {
            return EquipmentDetailTypes[op];
        }

        public static WeaponEquipType ConvertToWeapon(this EquipSlotType op)
        {
            return WeaponEquipTypes[op];
        }

        public static ArmorPart ConvertToArmor(this EquipSlotType op)
        {
            return ArmorEquipTypes[op];
        }

        public static string ConvertToDisplayName(this EquipSlotType op)
        {
            return EquipSlotDisplayNames[op];
        }

        public static DescribeViewType ConvertToDescribeType(this EquipmentType op)
        {
            return DescribeViewTypes[op];
        }
        
        public static DescribeViewType ConvertToDescribeViewType(this EquipContainerType op)
        {
            return EquipContainerDescribeViewTypes[op];
        }
    }
}