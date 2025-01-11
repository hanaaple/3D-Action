using Data.Item.Base;
using UnityEngine;

namespace Data.Item.Scriptable
{
    public enum WeaponType
    {
        Fist,
        StraightSword,
        GreatSword,
        Katana
    }

    public enum AttackType
    {
        Slash,
        Spear,
        Hit
    }
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
    public class WeaponStaticData : ItemStaticData
    {
        public GameObject prefab;
        public Vector3 leftTranslationOffset;
        public Vector3 leftRotationOffset;

        public Vector3 rightTranslationOffset;
        public Vector3 rightRotationOffset;

        public WeaponType weaponType;
        public AttackType attackType;
        public float staminaAmountUsed;

        public RuntimeAnimatorController runtimeAnimatorController;
        public float damage;
        public float poiseDamage;

        public float strengthWeight;
        public float workmanshipWeight;
        public float intellectWeight;
        
        public float attackMotionSpeed = 1;
        public float weight;
        
        public bool isBare;
    }
}