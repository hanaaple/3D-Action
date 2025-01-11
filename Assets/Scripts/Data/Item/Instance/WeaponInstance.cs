using Data.Item.Base;
using Data.Item.Data;
using Data.Item.Scriptable;
using Data.Play;
using UnityEngine;
using UnityEngine.Animations;

namespace Data.Item.Instance
{
    // 기존 상속하여 하였으나, 아이템 인스턴스가 무기뿐이기에 단일 클래스로 변경.
    public class WeaponInstance : MonoBehaviour
    {
        [SerializeField] private ParentConstraint parentConstraint;

        private Weapon _weapon;
        
        private void Awake()
        {
            Debug.LogWarning("Awake");
            parentConstraint = GetComponent<ParentConstraint>();
        }

        public void Initialize(Weapon weapon)
        {
            _weapon = weapon;
        }

        public BaseItem GetItem()
        {
            return _weapon;
        }
        
        public void SetActive(bool isActive)
        {
            Debug.LogWarning($"Active {gameObject} - {isActive}");
            gameObject.SetActive(isActive);

            // if(!isActive)
            //     OnRelease();
        }

        // protected void OnRelease()
        // {
        //     Debug.LogWarning("OnRelease");
        // }
        
        public void BindConstraint(Transform traceTransform, WeaponEquipType equipType)
        {
            WeaponStaticData weaponStaticData = _weapon.WeaponStaticData;
            
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
                parentConstraint.SetRotationOffset(0, weaponStaticData.rightRotationOffset);
                parentConstraint.SetTranslationOffset(0, weaponStaticData.rightTranslationOffset);
            }
            else if (equipType == WeaponEquipType.Left)
            {
                parentConstraint.SetRotationOffset(0, weaponStaticData.leftRotationOffset);
                parentConstraint.SetTranslationOffset(0, weaponStaticData.leftTranslationOffset);
            }
        }
    }
}