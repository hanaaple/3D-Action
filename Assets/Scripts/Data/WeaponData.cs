using UnityEngine;

namespace Data
{
    public enum WeaponType
    {
        Fist,
        StraightSword,
        GreatSword
    }
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
    public class WeaponData : Item.Item
    {
        public GameObject prefab;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;

        public WeaponType weaponType;
        
        public RuntimeAnimatorController runtimeAnimatorController;
        public float damage;
    }
}