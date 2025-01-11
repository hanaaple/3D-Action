using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Data.Item.Base;
using Data.Item.Data;
using Data.Item.Scriptable;
using Data.Play;
using UI.Selectable.Slot;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using Util;
using Tool = Data.Item.Data.Tool;

namespace Data.ViewModel
{    
    public struct EquipData
    {
        public BaseItem Item;
        public int Index;
        public EquipSlotType EquipSlotType;
    }
    // 장착 아이템 ViewModel
    
    // 기본적인 원칙(중복 금지, 데이터 제공 등)에 따른 책임 (동작) 내 프로퍼티 및 메서드 제공
    // ex) 이미 장착 중인 아이템을 누른 경우 장착 해제 시 -> 해당 View를 통한 장착 해제
    // 이미 장착된 아이템을 다른 슬롯에 장착할 때 -> ViewModel에서 장착 해제
    
    [Serializable]
    public sealed class EquipViewModel : INotifyPropertyChanged
    {
        private EquippedItemData _equippedItemData;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public EquippedItemData equippedItemData => _equippedItemData;
        
        public Span<Weapon> leftWeapons => _equippedItemData.leftWeapons;
        public Span<Weapon> rightWeapons => _equippedItemData.rightWeapons;
        public ReadOnlyArray<Armor> armors => _equippedItemData.armors;
        public Armor helmet => _equippedItemData.helmet;
        public Armor breastplate => _equippedItemData.breastplate;
        public Armor leggings => _equippedItemData.leggings;
        public Armor shoes => _equippedItemData.shoes;
        public ReadOnlyArray<Accessory> accessories => _equippedItemData.accessories;
        public ReadOnlyArray<Tool> tools => _equippedItemData.tools;

        private int toolIndex
        {
            get => _equippedItemData.toolIndex;
            set => SetField(ref _equippedItemData.toolIndex, value);
        }

        private int leftWeaponIndex
        {
            get => _equippedItemData.leftWeaponIndex;
            set => SetField(ref _equippedItemData.leftWeaponIndex, value);
        }

        public int rightWeaponIndex
        {
            get => _equippedItemData.rightWeaponIndex;
            set => SetField(ref _equippedItemData.rightWeaponIndex, value);
        }

        // 배열이 아니라 Wrapper로 감싼걸 Dictionary에 넣을까
        private Dictionary<EquipmentType, BaseEquipItem[]> _equipDictionary = new();
        
        public EquipViewModel(EquippedItemData equippedItemData)
        {
            _equippedItemData = equippedItemData;

            _equipDictionary.Add(EquipmentType.Weapon, equippedItemData.weapons);
            _equipDictionary.Add(EquipmentType.Armor, equippedItemData.armors);
            _equipDictionary.Add(EquipmentType.Accessory, equippedItemData.accessories);
            _equipDictionary.Add(EquipmentType.Tool, equippedItemData.tools);
            
            // _equipDictionary.Add(EquipSlotType.LeftWeapon, _equippedItemData.leftWeapons);
            // _equipDictionary.Add(EquipSlotType.RightWeapon, _equippedItemData.rightWeapons);
            // _equipDictionary.Add(EquipSlotType.Helmet, _equippedItemData.armors);
            // _equipDictionary.Add(EquipSlotType.Helmet, _equippedItemData.armors);
            // _equipDictionary.Add(EquipSlotType.Helmet, _equippedItemData.armors);
            // _equipDictionary.Add(EquipSlotType.Helmet, _equippedItemData.armors);
            // _equipDictionary.Add(EquipSlotType.Accessory, _equippedItemData.accessories);
            // _equipDictionary.Add(EquipSlotType.Tool, _equippedItemData.tools);
        }

        public void SetDefault(Weapon defaultWeapon, Armor defaultHelmet, Armor defaultBreastplate, Armor defaultLeggings,
            Armor defaultShoes)
        {
            _equippedItemData.defaultWeapon = defaultWeapon;
            _equippedItemData.defaultHelmet = defaultHelmet;
            _equippedItemData.defaultBreastplate = defaultBreastplate;
            _equippedItemData.defaultLeggings = defaultLeggings;
            _equippedItemData.defaultShoes = defaultShoes;
        }
        
        public void SetWeapon(Weapon weapon, int targetIndex, WeaponEquipType equipType, bool isNotifyChanged = true)
        {
            var equipSlotType = equipType switch
            {
                WeaponEquipType.Left => EquipSlotType.LeftWeapon,
                WeaponEquipType.Right => EquipSlotType.RightWeapon,
                _ => throw new ArgumentOutOfRangeException(nameof(equipType), equipType, null)
            };

            if (weapon.IsNullOrEmpty())
            {
                weapon = _equippedItemData.defaultWeapon;
            }

            UnEquipAlreadyEquippedItem(weapon, equipSlotType);

            Debug.LogWarning($"Set Weapon {targetIndex}, {equipType}, {weapon.GetItemDisplayName()}");
            
            ref var weaponReference = ref GetWeapon(equipType, targetIndex);
            SetField(ref weaponReference, weapon, isNotifyChanged);
        }

        public void SetArmor(Armor armor, bool isNotifyChanged = true)
        {
            switch (armor.GetArmorData().armorPart)
            {
                case ArmorPart.Helmet:
                    if (armor.IsNullOrEmpty())
                    {
                        armor = _equippedItemData.defaultHelmet;
                    }

                    UnEquipAlreadyEquippedItem(armor, EquipSlotType.Helmet);
                    SetField(ref _equippedItemData.helmet, armor, isNotifyChanged);
                    break;
                case ArmorPart.Breastplate:
                    if (armor.IsNullOrEmpty())
                    {
                        armor = _equippedItemData.defaultBreastplate;
                    }

                    UnEquipAlreadyEquippedItem(armor, EquipSlotType.BreastPlate);
                    SetField(ref _equippedItemData.breastplate, armor, isNotifyChanged);
                    break;
                case ArmorPart.Leggings:
                    if (armor.IsNullOrEmpty())
                    {
                        armor = _equippedItemData.defaultLeggings;
                    }

                    UnEquipAlreadyEquippedItem(armor, EquipSlotType.Leggings);
                    SetField(ref _equippedItemData.leggings, armor, isNotifyChanged);
                    break;
                case ArmorPart.Shoes:
                    if (armor.IsNullOrEmpty())
                    {
                        armor = _equippedItemData.defaultShoes;
                    }

                    UnEquipAlreadyEquippedItem(armor, EquipSlotType.Shoes);
                    SetField(ref _equippedItemData.shoes, armor, isNotifyChanged);
                    break;
                default:
                    Debug.LogError($"잘못된 타입 사용, {armor.GetArmorData().armorPart}은 Armor가 아닙니다.");
                    break;
            }
        }

        public void SetAccessory(Accessory targetItem, int targetIndex, bool isNotifyChanged = true)
        {
            UnEquipAlreadyEquippedItem(targetItem, EquipSlotType.Accessory);
            SetField(ref _equippedItemData.accessories[targetIndex], targetItem, isNotifyChanged);
        }

        public void SetTool(Tool targetItem, int targetIndex, bool isNotifyChanged = true)
        {
            UnEquipAlreadyEquippedItem(targetItem, EquipSlotType.Tool);
            SetField(ref _equippedItemData.tools[targetIndex], targetItem, isNotifyChanged);
        }
        
        /// <summary>
        /// 이미 다른 슬롯에 장착 중이라면 해당 아이템은 장착 해제 후 새로운 슬롯에 장착된다.
        /// </summary>
        public void SetItem(BaseItem changeToBaseItem, EquipSlotType targetEquipSlotType, int targetEquippedIndex = 0, bool isNotifyChanged = true)
        {
            if (targetEquipSlotType is EquipSlotType.LeftWeapon or EquipSlotType.RightWeapon)
            {
                var weaponType = targetEquipSlotType.ConvertToWeapon();
                var weapon = changeToBaseItem as Weapon;
                SetWeapon(weapon, targetEquippedIndex, weaponType, isNotifyChanged);
            }
            else if (targetEquipSlotType is EquipSlotType.Helmet or EquipSlotType.BreastPlate or EquipSlotType.Leggings or EquipSlotType.Shoes)
            {
                var armor = changeToBaseItem as Armor;
                SetArmor(armor, isNotifyChanged);
            }
            else if (targetEquipSlotType == EquipSlotType.Accessory)
            {
                var accessory = changeToBaseItem as Accessory;
                SetAccessory(accessory, targetEquippedIndex, isNotifyChanged);
            }
            else if (targetEquipSlotType == EquipSlotType.Tool)
            {
                var tool = changeToBaseItem as Tool;
                SetTool(tool, targetEquippedIndex, isNotifyChanged);
            }
            else
            {
                Debug.LogError($"{targetEquipSlotType}  Error");
            }
        }
        
        // 만약 없다가 생긴다면 Index는 첫 Item의 Index로 지정
        public void MoveToNextToolIndex()
        {
            int index = (toolIndex + 1) % tools.Count;
            while (tools[index].IsNullOrEmpty() && toolIndex != index)
            {
                index = (index + 1) % tools.Count;
            }

            toolIndex = index;
        }

        // 만약 없다가 생긴다면 Index는 첫 Item의 Index로 지정
        public void MoveToNextWeaponIndex(WeaponEquipType equipType)
        {
            int weaponIndex = 0;
            Span<Weapon> weapons = null;
            
            if (equipType == WeaponEquipType.Right)
            {
                weaponIndex = rightWeaponIndex;
                weapons = rightWeapons;
            }
            else if (equipType == WeaponEquipType.Left)
            {
                weaponIndex = leftWeaponIndex;
                weapons = leftWeapons;
            }

            int nextIndex = weaponIndex;
            do
            {
                nextIndex = (nextIndex + 1) % weapons.Length;
            } while (weapons[nextIndex].IsNullOrBare() && weaponIndex != nextIndex);
            
            
            // 무기가 1개밖에 없거나 전부 맨손인 경우
            if (nextIndex == weaponIndex)
            {
                // 무기 1개인 경우
                if (!weapons[nextIndex].IsNullOrBare())
                {
                    nextIndex = (nextIndex + 1) % weapons.Length;
                }
            }
            
            Debug.Log($"{equipType}: {weaponIndex} -> {nextIndex}");
            
            if (equipType == WeaponEquipType.Right)
            {
                rightWeaponIndex = nextIndex;
            }
            else if (equipType == WeaponEquipType.Left)
            {
                leftWeaponIndex = nextIndex;
            }
        }
        
        // 성능적인 문제가 크게 발생하지 않을 것으로 예상하여 GetAll
        public EquipData[] GetAllEquippedItems()
        {
            int count = 0;
            
            foreach (var item in rightWeapons)
                if (item.IsNullOrBare())
                    count++;
            foreach (var item in leftWeapons)
                if (item.IsNullOrBare())
                    count++;
            foreach (var item in armors)
                if (item.IsNullOrBare())
                    count++;
            


            var equipData = new EquipData[count + accessories.Count + tools.Count];
            
            // TODO: Check 맨손, 맨몸
            // 역시 여기서 없애주는게 좋을듯. 
            int dataIndex = 0;
            SetEquipData(equipData, ref dataIndex, rightWeapons);
            SetEquipData(equipData, ref dataIndex, leftWeapons);
            SetEquipData(equipData, ref dataIndex, helmet);
            SetEquipData(equipData, ref dataIndex, breastplate);
            SetEquipData(equipData, ref dataIndex, leggings);
            SetEquipData(equipData, ref dataIndex, shoes);
            SetEquipData(equipData, ref dataIndex, accessories);
            SetEquipData(equipData, ref dataIndex, tools);

            return equipData;
        }

        // 아이템 및 해당 아이템의 Index가 무엇인지도 알아야됨.
        
        // 그런데 ItemType이 여러개이면? Flag이면?
        // public EquipData[] GetItems(ItemType itemType)
        // {
        //     var baseItems = _equipMap[itemType].ToArray();
        //     var equipData = new EquipData[baseItems.Length];
        //     int dataIndex = 0;
        //     SetEquipData(equipData, ref dataIndex, baseItems);
        //     
        //     return equipData;
        // }
        
        public EquipData GetItem(EquipSlotType targetEquipSlotType, int targetEquippedIndex = 0)
        {
            EquipData equipData = new EquipData();
            BaseItem item = null;
            if (targetEquipSlotType == EquipSlotType.LeftWeapon)
            {
                item = leftWeapons[targetEquippedIndex];
            }
            else if (targetEquipSlotType == EquipSlotType.RightWeapon)
            {
                item = rightWeapons[targetEquippedIndex];
            }
            else if (targetEquipSlotType == EquipSlotType.Accessory)
            {
                item = accessories[targetEquippedIndex];
            }
            else if (targetEquipSlotType == EquipSlotType.Tool)
            {
                item = tools[targetEquippedIndex];
            }
            else if (targetEquipSlotType == EquipSlotType.Helmet)
            {
                item = helmet;
            }
            else if (targetEquipSlotType == EquipSlotType.BreastPlate)
            {
                item = breastplate;
            }
            else if (targetEquipSlotType == EquipSlotType.Leggings)
            {
                item = leggings;
            }
            else if (targetEquipSlotType == EquipSlotType.Shoes)
            {
                item = shoes;
            }
            else
            {
                Debug.LogError($"Type Error. Type: {targetEquipSlotType}, Index: {targetEquippedIndex}");
            }

            equipData.Item = item;
            equipData.Index = targetEquippedIndex;
            equipData.EquipSlotType = targetEquipSlotType;
            
            return equipData;
        }

        /// <returns> if return null, it's bare hands</returns>
        public Weapon GetCurrentWeapon(WeaponEquipType weaponEquipType)
        {
            Weapon weapon = null;

            if (weaponEquipType == WeaponEquipType.Left)
            {
                weapon = leftWeapons[leftWeaponIndex];
                //Debug.LogWarning($"{weaponEquipType},  {leftWeaponIndex}");
            }
            else if (weaponEquipType == WeaponEquipType.Right)
            {
                weapon = rightWeapons[rightWeaponIndex];
                //Debug.LogWarning($"{weaponEquipType},  {rightWeaponIndex}");
            }

            //Debug.LogWarning($"{weapon.IsNullOrBare()}");
            
            return weapon;
        }

        public BaseItem GetCurrentTool()
        {
            if (tools[toolIndex].IsNullOrEmpty())
            {
                return null;
            }

            return tools[toolIndex];
        }

        // TODO: List 객체를 생성하고 Array 객체를 매번 생성한다. 최적화 필요.
        public ReadOnlyArray<ItemStaticData> GetEquippedToolData(int maxCount)
        {
            List<ItemStaticData> itemData = new List<ItemStaticData>();

            int index = toolIndex;

            do
            {
                if (!tools[index].IsNullOrEmpty())
                {
                    itemData.Add(tools[index].GetItemData());
                }

                index = (index + 1) % tools.Count;
            } while (toolIndex != index && itemData.Count < maxCount);

            return itemData.ToArray();
        }

        public int GetMaxWeaponCount()
        {
            return leftWeapons.Length;
        }

        private ref Weapon GetWeapon(WeaponEquipType equipType, int targetIndex)
        {
            if (equipType == WeaponEquipType.Left)
            {
                return ref _equippedItemData.leftWeapons[targetIndex];
            }
            else if (equipType == WeaponEquipType.Right)
            {
                return ref _equippedItemData.rightWeapons[targetIndex];
            }
            else
            {
                Debug.LogError($"Error {equipType}");
                throw new Exception();
            }
        }

        private static void SetEquipData(EquipData[] equipData, ref int dataIndex, params BaseItem[] items)
        {
            int index = 0;
            foreach (var item in items)
            {
                if(item.IsNullOrBare()) continue;
                equipData[dataIndex].Item = item;
                equipData[dataIndex].Index = index;
                dataIndex++;
                index++;
            }
        }
        
        private static void SetEquipData(EquipData[] equipData, ref int dataIndex, IEnumerable<BaseItem> items)
        {
            int index = 0;
            foreach (var item in items)
            {
                if(item.IsNullOrBare()) continue;
                equipData[dataIndex].Item = item;
                equipData[dataIndex].Index = index;
                dataIndex++;
                index++;
            }
        }
        
        private void SetEquipData(EquipData[] equipData, ref int dataIndex, Span<Weapon> items)
        {
            int index = 0;
            foreach (var item in items)
            {
                if(item.IsNullOrBare()) continue;
                equipData[dataIndex].Item = item;
                equipData[dataIndex].Index = index;
                dataIndex++;
                index++;
            }
        }

        /// <summary>
        /// 아이템 장착 전, 안전하게 장착해제하는 메서드이다.
        /// </summary>
        private void UnEquipAlreadyEquippedItem(BaseItem targetItem, EquipSlotType equipSlotType)
        {
            if (targetItem.IsNullOrBare()) return;
            var equipmentType = equipSlotType.ConvertToEquipment();
            
            if (IsAlreadyEquipped(targetItem, equipmentType, out var equippedIndex))
            {
                // equippedIndex 0 ~ 2 -> Left, 0 ~ 2
                // equippedIndex 3 ~ 5 -> Right, 0 ~ 2
                
                if (equipSlotType is EquipSlotType.LeftWeapon or EquipSlotType.RightWeapon)
                {
                    if (equippedIndex >= leftWeapons.Length)
                    {
                        equipSlotType = EquipSlotType.RightWeapon;
                        equippedIndex -= leftWeapons.Length;
                    }
                    else
                    {
                        equipSlotType = EquipSlotType.LeftWeapon;
                    }
                }
                
                SetItem(null, equipSlotType, equippedIndex, false);
            }
        }

        // private void UnEquipAlreadyEquippedItem(Weapon weapon, EquipSlotType equipSlotType, int targetIndex)
        // {
        //     if(weapon.IsNullOrBare()) return;
        //     
        //     if (IsAlreadyEquipped(weapon, equipSlotType, out var equipIndex) && equipIndex != targetIndex)
        //     {
        //         SetItem(null, equipSlotType, equipIndex, false);
        //     }
        // }

        /// <param name="equippedIndex"> when return false, the value is -1 </param>
        private bool IsAlreadyEquipped(BaseItem targetItem, EquipmentType equipmentType, out int equippedIndex)
        {
            if (targetItem.IsNullOrBare())
            {
                equippedIndex = -1;
                return false;
            }
            
            var items = _equipDictionary[equipmentType];
            equippedIndex = Array.FindIndex(items, item => item == targetItem);
            
            return equippedIndex != -1;
        }
        
        private void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, null);
        }
        
        private bool SetField<T>(ref T field, T value, bool isNotifyChanged = true)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;

            if(isNotifyChanged) OnPropertyChanged();
            return true;
        }
    }
}