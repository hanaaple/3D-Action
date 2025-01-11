using System;
using System.Collections.Generic;
using Data.Item.Instance;
using Data.Play;
using UnityEngine;
using Util;

namespace Character
{
    public enum BodyType
    {
        LeftHand,
        RightHand
    }

    [Serializable]
    public struct Body
    {
        public BodyType bodyType;
        public Transform transform;
    }
    
    public class CharacterBodyManager : MonoBehaviour
    {
        [SerializeField] private Body[] bodyPart;
        
        private readonly Dictionary<BodyType, Transform> _bodyDictionary = new (EnumComparer.For<BodyType>());
        
        private readonly Dictionary<BodyType, WeaponInstance> _items = new (EnumComparer.For<BodyType>());

        private void Awake()
        {
            foreach (var body in bodyPart)
            {
                _bodyDictionary.Add(body.bodyType, body.transform);
            }
        }

        public Transform GetPartOfBody(BodyType bodyType)
        {
            return _bodyDictionary.GetValueOrDefault(bodyType);
        }

        public WeaponInstance GetBindItemInstance(BodyType bodyType)
        {
            return _items.GetValueOrDefault(bodyType);
        }

        public void Release(BodyType bodyType)
        {
            _items[bodyType] = null;
        }

        public void BindItem(BodyType bodyType, WeaponInstance weaponInstance)
        {
            Debug.LogWarning($"Bind {weaponInstance.GetItem().GetItemDisplayName()} to {bodyType}");
            // 1. Set Item
            _items[bodyType] = weaponInstance;
            
            // 2. Bind
            var targetTransform = GetPartOfBody(bodyType);

            WeaponEquipType weaponEquipType = WeaponEquipType.None;
            if (bodyType == BodyType.LeftHand)
            {
                weaponEquipType = WeaponEquipType.Left;
            }
            else if(bodyType == BodyType.RightHand)
            {
                weaponEquipType = WeaponEquipType.Right;
            }
            
            weaponInstance.BindConstraint(targetTransform, weaponEquipType);
        }
    }
}