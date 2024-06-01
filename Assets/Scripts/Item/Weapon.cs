using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
    public class Weapon : Item
    {
        public GameObject prefab;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;
    }
}