using System;
using Data;

namespace Item
{
    [Serializable]
    public class Weapon
    {
        // 기본 데이터
        public WeaponData weaponData;

        // 추가 데이터
        public int weaponEnhancementValue;
        // 인챈트 상태
    }
}