using System;
using Data.Item.Data;
using UnityEngine.InputSystem.Utilities;
using Util;

namespace Data.Play
{
    public enum WeaponEquipType
    {
        None,
        Left,
        Right
    }

    /// <summary>
    /// 장착 중인 장비 데이터
    /// 데이터 정제, 분석 등의 메서드를 제공하지 않는다.
    /// </summary>
    [Serializable]
    public struct EquippedItemData
    {
        // TODO: null 혹은 맨몸인 상태는 어떻게 정의할 것인가.

        // 0 ~ 2 Left, 3 ~ 5 Right
        public Weapon[] weapons;
        public Accessory[] accessories;
        public Armor[] armors; // 0 ~ 3 -> helmet, breastplate, leggings, shoes
        public Tool[] tools;

        // 현재 선택 or 장착된 아이템의 Index
        public int leftWeaponIndex;
        public int rightWeaponIndex;
        public int toolIndex;

        public ref Armor helmet => ref armors[0];
        public ref Armor breastplate => ref armors[1];
        public ref Armor leggings => ref armors[2];
        public ref Armor shoes => ref armors[3];

        public Span<Weapon> leftWeapons => weapons.AsSpan(0, weapons.Length / 2);
        public Span<Weapon> rightWeapons => weapons.AsSpan(weapons.Length / 2);

        public Weapon defaultWeapon;
        public Armor defaultHelmet;
        public Armor defaultBreastplate;
        public Armor defaultLeggings;
        public Armor defaultShoes;

        public EquippedItemData(int weaponCapacity, int accessoryCapacity, int toolCapacity)
        {
            armors = new Armor[4];
            weapons = new Weapon[weaponCapacity * 2];
            accessories = new Accessory[accessoryCapacity];
            tools = new Tool[toolCapacity];

            leftWeaponIndex = 0;
            rightWeaponIndex = 0;
            toolIndex = 0;

            this.defaultWeapon = null;
            this.defaultHelmet = null;
            this.defaultBreastplate = null;
            this.defaultLeggings = null;
            this.defaultShoes = null;
        }
    }
}