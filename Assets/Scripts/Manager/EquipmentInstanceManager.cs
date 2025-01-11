using System.Collections.Generic;
using Character;
using Data.Item.Base;
using Data.Item.Data;
using Data.Item.Instance;
using Data.Play;
using Data.ViewModel;
using UnityEngine;
using UnityEngine.Pool;

namespace Manager
{
    /// <summary>
    /// 장비 Instance 관리자
    /// </summary>
    public class EquipmentInstanceManager : MonoBehaviour
    {
        private readonly Dictionary<string, IObjectPool<WeaponInstance>> _objectPoolMap = new();

        // 뭔가 이상한데 이거도 바꿔야되나.. (ViewModel을 넘겨주는 방식이나, ObjectPool이 등록만 하고 삭제는 안하거나, weapon의 변화에 대해서만 작동해야되며
        // 뭐가 바뀌든 계속 Swap을 하고 있음.
        public void UpdateEquipment(CharacterBodyManager bodyManager, EquipViewModel equipViewModel)
        {
            RegistItemPool(equipViewModel);
            var currentLeftWeapon = equipViewModel.GetCurrentWeapon(WeaponEquipType.Left);
            var currentRightWeapon = equipViewModel.GetCurrentWeapon(WeaponEquipType.Right);
            
            SwapWeapon(bodyManager, currentLeftWeapon, BodyType.LeftHand);
            SwapWeapon(bodyManager, currentRightWeapon, BodyType.RightHand);
        }
        
        // 현재 착용 중인 아이템에 대해 ObjectPool을 생성하고 등록한다.
        private void RegistItemPool(EquipViewModel equipViewModel)
        {
            foreach (var weapon in equipViewModel.rightWeapons)
            {
                if (weapon.IsNullOrBare()) continue;

                if (!_objectPoolMap.ContainsKey(weapon.id))
                {
                    var objectPool = CreateObjectPool(weapon);
                    _objectPoolMap.Add(weapon.id, objectPool);
                }
            }

            foreach (var weapon in equipViewModel.leftWeapons)
            {
                if (weapon.IsNullOrBare()) continue;

                if (!_objectPoolMap.ContainsKey(weapon.id))
                {
                    var objectPool = CreateObjectPool(weapon);
                    _objectPoolMap.Add(weapon.id, objectPool);
                }
            }
             
            // TODO: 등록삭제
        }
        
        private void SwapWeapon(CharacterBodyManager characterBodyManager, BaseItem targetItem, BodyType bodyType)
        {
            Debug.LogWarning($"Swap Weapon {targetItem.scriptableObjectName} {bodyType}");
            var prevItemInstance = characterBodyManager.GetBindItemInstance(bodyType);

            if (prevItemInstance != null)
            {
                var prevWeapon = prevItemInstance.GetItem();
                if (prevWeapon == targetItem) return;
                
                //if(prevWeapon.IsNullOrBare())
                //Debug.LogWarning($"Release {bodyType} {targetItem.IsNullOrEmpty()}");
                ReleaseEquipment(characterBodyManager, bodyType);
                // Release를 해서 Null이 된건가?
            }

            //Debug.LogWarning($"Equip {bodyType}   {targetItem.IsNullOrEmpty()}");
            EquipWeapon(characterBodyManager, targetItem, bodyType);
        }

        private static IObjectPool<WeaponInstance> CreateObjectPool(Weapon weapon)
        {
            var objectPool = new ObjectPool<WeaponInstance>(
                () =>
                {
                    var prefab = weapon.WeaponStaticData.prefab;
                    var instantiateWeapon = Instantiate(prefab);
                    var instance = instantiateWeapon.GetComponent<WeaponInstance>();
                    instance.Initialize(weapon);
                    instantiateWeapon.SetActive(false);
                    return instance;
                },
                instance => { instance.SetActive(true); },
                instance => { instance.SetActive(false); },
                Destroy, true, 1
            );
            return objectPool;
        }

        private void ReleaseEquipment(CharacterBodyManager characterBodyManager, BodyType bodyType)
        {
            var prevItemInstance = characterBodyManager.GetBindItemInstance(bodyType);
            if (prevItemInstance == null) return;

            var item = prevItemInstance.GetItem();
            if (item.IsNullOrBare()) return;


            var objectPool = _objectPoolMap[item.id];
            objectPool.Release(prevItemInstance);

            characterBodyManager.Release(bodyType);
        }
        
        private void EquipWeapon(CharacterBodyManager characterBodyManager, BaseItem targetItem, BodyType bodyType)
        {
            if (targetItem.IsNullOrBare()) return;
            
            var itemInstance = _objectPoolMap[targetItem.id].Get();
            
            //Debug.LogWarning($"{itemInstance},  {itemInstance.gameObject.activeSelf}");

            var weaponInstance = itemInstance.GetComponent<WeaponInstance>();
            characterBodyManager.BindItem(bodyType, weaponInstance);
        }
        
        // Armor 등 추가시 추가 구현
    }
}