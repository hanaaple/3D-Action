using Data.Item.Base;
using UnityEngine;

namespace Data.Item.Scriptable
{
    public enum AccessoryType
    {
        
    }
    
    [CreateAssetMenu(fileName = "Accessory", menuName = "Item/Accessory")]
    public class AccessoryStaticData : ItemStaticData
    {
        public AccessoryType accessoryType;
        public float weight;
        public string itemEffect;
    }
}