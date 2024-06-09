using Data.Play;
using UnityEngine;
using UnityEngine.Animations;

namespace Data.Static.Scriptable
{
    public enum WeaponType
    {
        Fist,
        StraightSword,
        GreatSword,
        Katana
    }

    [CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
    public class WeaponData : ItemData
    {
        public GameObject prefab;
        public Vector3 leftTranslationOffset;
        public Vector3 leftRotationOffset;

        public Vector3 rightTranslationOffset;
        public Vector3 rightRotationOffset;

        public WeaponType weaponType;

        public RuntimeAnimatorController runtimeAnimatorController;
        public float damage;
        public float attackMotionSpeed;
        public float weight;

        public void LoadConstraintSetting(ParentConstraint parentConstraint, Transform traceTransform, WeaponEquipType equipType)
        {
            while (parentConstraint.sourceCount > 0)
            {
                parentConstraint.RemoveSource(0);
            }

            ConstraintSource constraintSource = new ConstraintSource
            {
                sourceTransform = traceTransform,
                weight = 1
            };

            parentConstraint.constraintActive = true;
            parentConstraint.AddSource(constraintSource);
            if (equipType == WeaponEquipType.Right)
            {
                parentConstraint.SetRotationOffset(0, rightRotationOffset);
                parentConstraint.SetTranslationOffset(0, rightTranslationOffset);
            }
            else if (equipType == WeaponEquipType.Left)
            {
                parentConstraint.SetRotationOffset(0, leftRotationOffset);
                parentConstraint.SetTranslationOffset(0, leftTranslationOffset);
            }
        }
    }
}