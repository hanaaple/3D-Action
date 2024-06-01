using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "Armor", menuName = "Item/Armor")]
    public class Armor : Item
    {
        public GameObject prefab;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;
    }
}