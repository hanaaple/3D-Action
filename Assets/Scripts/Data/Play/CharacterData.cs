using System;

namespace Data.Play
{
    /// <summary>
    /// 장비, 스탯에 따른 계산 값  ex - (default + 스탯 * 1 + 악세 * 2)
    /// hp, mp, Sp, 공격력, 방어력, 장비 중량 등
    /// 캐릭터 데이터
    /// </summary>
    [Serializable]
    public struct CharacterData
    {
        // 생명력
        // 정신력
        // 지구력
        // 근력
        // 기량
        // 지력
        
        
        public float attack;    
        public float defense;   // 방어력
        public float healthPoint;   //
        public float manaPoint;
        public float staminaPoint;
        public float equipWeight;
        public float poiseHealthPoint;
        
        public float staminaRecoveryWeight;
        public float poiseHealthRecoveryWeight;

        public float maxHealthPoint;
        public float maxManaPoint;
        public float maxStaminaPoint;
        public float maxEquipWeight;
        public float maxPoiseHealthPoint;
    }
}