using Data.Item.Base;
using UnityEngine;

namespace Data.Item.Scriptable
{
    public enum ArmorPart
    {
        Helmet,
        Breastplate,
        Leggings,
        Shoes
    }

    public enum ArmorType
    {
        ClothArmor,
        BootsArmor,
        PlateArmor
    }
    
    // TODO
    // 이거 헬멧, 흉갑, 각반 등 나눠야겠느데?
    [CreateAssetMenu(fileName = "Armor", menuName = "Item/Armor")]
    public class ArmorStaticData : ItemStaticData
    {
        public ArmorPart armorPart;
        public ArmorType armorType;
        // public GameObject prefab;
        // public Vector3 positionOffset;
        // public Vector3 rotationOffset;
        public int weight;  // 중량
        public int defense; // 물리
        //public int poise; 강인도

        public float antiStrike;
        public float antiPierce;
        public float antiSlash;
        
        public bool isBare;
    }
}