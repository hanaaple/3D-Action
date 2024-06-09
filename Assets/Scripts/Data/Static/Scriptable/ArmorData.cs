using UnityEngine;

namespace Data.Static.Scriptable
{
    public enum ArmorType
    {
        Helmet,
        Breastplate,
        Leggings,
        Shoes
    }
    
    [CreateAssetMenu(fileName = "Armor", menuName = "Item/Armor")]
    public class ArmorData : ItemData
    {
        public GameObject prefab;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;
        public float weight;
        public ArmorType armorType;
    }
}