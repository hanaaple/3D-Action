using System.Collections.Generic;
using System.ComponentModel;
using Data;
using Data.Play;
using Data.PlayItem;
using Data.Static.Scriptable;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Pool;

namespace CharacterControl
{
    /// <summary>
    /// 장비 Instance 관리자
    /// </summary>
    public class EquipmentInstanceManager : MonoBehaviour
    {
        private Item _leftWeapon;
        private Item _rightWeapon;
        private GameObject _previousLeftWeapon;
        private GameObject _previousRightWeapon;

        // private Item _helmet;
        // private Item _breastplate;
        // private Item _leggings;
        // private Item _shoes;


        [SerializeField] private Player player;

        // Item id, 해당 Item의 ObjectPool
        // <Item id, ObjectPool<GameObject>>

        private readonly Dictionary<string, IObjectPool<GameObject>> _equipmentMap = new();

        private void Awake()
        {
            var playerEquipViewModel = DataManager.instance.playerEquipViewModel;
            playerEquipViewModel.PropertyChanged += RegistItem;
            playerEquipViewModel.PropertyChanged += EquipInstance;
        }

        private void EquipInstance(object sender, PropertyChangedEventArgs e)
        {
            var equipViewModel = DataManager.instance.playerEquipViewModel;
            var currentLeftWeapon = equipViewModel.GetCurrentLeftWeapon();
            var currentRightWeapon = equipViewModel.GetCurrentRightWeapon();

            SwapWeapon(WeaponEquipType.Left, ref _leftWeapon, currentLeftWeapon, ref _previousRightWeapon,
                player.leftHand);
            SwapWeapon(WeaponEquipType.Right, ref _rightWeapon, currentRightWeapon, ref _previousLeftWeapon,
                player.rightHand);
        }

        private void SwapWeapon(WeaponEquipType equipType, ref Item prevWeapon, Item targetWeapon,
            ref GameObject prevWeaponInstance, Transform bindTransform)
        {
            if (prevWeapon == targetWeapon) return;

            if (!prevWeapon.IsNullOrEmpty())
            {
                var prevObjectPool = _equipmentMap[prevWeapon.GetItemData().id];
                prevObjectPool.Release(prevWeaponInstance);
            }

            prevWeapon = targetWeapon;

            if (!targetWeapon.IsNullOrEmpty())
            {
                prevWeaponInstance = _equipmentMap[targetWeapon.GetItemData().id].Get();

                var parentConstraint = prevWeaponInstance.GetComponent<ParentConstraint>();
                ((WeaponData)targetWeapon.GetItemData()).LoadConstraintSetting(parentConstraint, bindTransform,
                    equipType);
            }
        }

        private void RegistItem(object sender, PropertyChangedEventArgs e)
        {
            var equipViewModel = DataManager.instance.playerEquipViewModel;

            foreach (var weapon in equipViewModel.Rights)
            {
                if (weapon.IsNullOrEmpty()) continue;

                if (!_equipmentMap.ContainsKey(weapon.GetItemData().id))
                {
                    var weaponData = weapon.GetItemData() as WeaponData;

                    var objectPool = AddObjectPool(weaponData.prefab);
                    _equipmentMap.Add(weaponData.id, objectPool);
                }
            }

            foreach (var weapon in equipViewModel.Lefts)
            {
                if (weapon.IsNullOrEmpty()) continue;

                if (!_equipmentMap.ContainsKey(weapon.GetItemData().id))
                {
                    var weaponData = weapon.GetItemData() as WeaponData;

                    var objectPool = AddObjectPool(weaponData.prefab);
                    _equipmentMap.Add(weaponData.id, objectPool);
                }
            }
        }

        private IObjectPool<GameObject> AddObjectPool(GameObject prefab)
        {
            var objectPool = new ObjectPool<GameObject>(
                () =>
                {
                    var instantiateWeapon = Instantiate(prefab);
                    instantiateWeapon.SetActive(false);
                    return instantiateWeapon;
                },
                instance => { instance.SetActive(true); },
                instance => { instance.SetActive(false); },
                Destroy, true, 1
            );
            return objectPool;
        }
    }
}