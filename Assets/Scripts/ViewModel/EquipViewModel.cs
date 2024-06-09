using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data.Play;
using Data.PlayItem;
using Data.Static.Scriptable;
using UnityEngine.InputSystem.Utilities;
using Tool = Data.PlayItem.Tool;

namespace ViewModel
{
    // 장착 아이템 ViewModel
    [Serializable]
    public sealed class EquipViewModel : INotifyPropertyChanged
    {
        private EquippedItemData _equippedItemData;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyArray<Weapon> Lefts
        {
            get => _equippedItemData.Lefts;
        }

        public ReadOnlyArray<Weapon> Rights
        {
            get => _equippedItemData.Rights;
        }
        
        public Armor Helmet
        {
            get => _equippedItemData.Helmet;
        }
        
        public Armor Breastplate
        {
            get => _equippedItemData.Breastplate;
        }
        
        public Armor Leggings
        {
            get => _equippedItemData.Leggings;
        }
        
        public Armor Shoes
        {
            get => _equippedItemData.Shoes;
        }
        
        public ReadOnlyArray<Accessory> Accessories
        {
            get => _equippedItemData.Accessories;
        }

        public ReadOnlyArray<Tool> Tools
        {
            get => _equippedItemData.Tools;
        }

        private int ToolIndex
        {
            get => _equippedItemData.ToolIndex;
            set => _equippedItemData.ToolIndex = value;
        }

        private int LeftIndex
        {

            get => _equippedItemData.LeftIndex;
            set => _equippedItemData.LeftIndex = value;
        }

        public int RightIndex
        {
            get => _equippedItemData.RightIndex;
            set => _equippedItemData.RightIndex = value;
        }

        public Item[] GetAllEquippedItems()
        {
            var list = new List<Item>();
            list.AddRange(Rights);
            list.AddRange(Lefts);
            list.Add(Helmet);
            list.Add(Breastplate);
            list.Add(Leggings);
            list.Add(Shoes);
            list.AddRange(Accessories);
            list.AddRange(Tools);
            return list.ToArray();
        }

        public void Initialize(EquippedItemData equippedItemData)
        {
            _equippedItemData = equippedItemData;

            InitializeForDebug();
            
            _equippedItemData.PropertyChanged += (send, e) => OnPropertyChanged(e.PropertyName);
        }

        private void InitializeForDebug()
        {
            foreach (var item in Lefts)
            {
                item?.Initialize();
            }

            foreach (var item in Rights)
            {
                item?.Initialize();
            }

            foreach (var item in Accessories)
            {
                item?.Initialize();
            }

            foreach (var item in Tools)
            {
                item?.Initialize();
            }

            Helmet?.Initialize();
            Breastplate?.Initialize();
            Leggings?.Initialize();
            Shoes?.Initialize();
        }

        public void SetWeapon(int targetIndex, Weapon weapon, WeaponEquipType equipType)
        {
            var leftIndex = Lefts.IndexOf(item => item == weapon);
            var rightIndex = Rights.IndexOf(item => item == weapon);
            
            if (leftIndex != -1)
            {
                _equippedItemData.EquipWeapon(leftIndex, null, WeaponEquipType.Left);
            }
            else if (rightIndex != -1)
            {
                _equippedItemData.EquipWeapon(rightIndex, null, WeaponEquipType.Right);
            }

            if (equipType == WeaponEquipType.Left)
            {
                if (leftIndex != targetIndex)
                {
                    _equippedItemData.EquipWeapon(targetIndex, weapon, WeaponEquipType.Left);
                }
            }
            else if (equipType == WeaponEquipType.Right)
            {
                if (rightIndex != targetIndex)
                {
                    _equippedItemData.EquipWeapon(targetIndex, weapon, WeaponEquipType.Right);
                }
            }
        }

        public void SetHelmet(Armor armor)
        {
            _equippedItemData.SetHelmet(armor);
        }
        
        public void SetBreastPlate(Armor armor)
        {
            _equippedItemData.SetBreastPlate(armor);
        }
        
        public void SetLeggings(Armor armor)
        {
            _equippedItemData.SetLeggings(armor);
        }
        
        public void SetShoes(Armor armor)
        {
            _equippedItemData.SetShoes(armor);
        }
        
        public void SetAccessory(int index, Accessory accessory)
        {
            _equippedItemData.SetAccessory(index, accessory);
        }
        
        public void SetTool(int index, Tool tool)
        {
            _equippedItemData.SetTool(index, tool);
        }
        
        /// <returns> if return null, it's bare hands</returns>
        public Weapon GetCurrentLeftWeapon()
        {
            if (Lefts[LeftIndex].IsNullOrEmpty())
            {
                return null;
            }

            return Lefts[LeftIndex];
        }

        /// <returns> if return null, it's bare hands</returns>
        public Weapon GetCurrentRightWeapon()
        {
            if (Rights[RightIndex].IsNullOrEmpty())
            {
                return null;
            }

            return Rights[RightIndex];
        }

        public Item GetCurrentTool()
        {
            if (Tools[ToolIndex].IsNullOrEmpty())
            {
                return null;
            }
            
            return Tools[ToolIndex];
        }

        public ReadOnlyArray<ItemData> GetToolData(int maxCount)
        {
            List<ItemData> itemData = new List<ItemData>();

            int index = ToolIndex;

            do
            {
                if (!Tools[index].IsNullOrEmpty())
                {
                    itemData.Add(Tools[index].GetItemData());
                }

                index = (index + 1) % Tools.Count;
            } while (ToolIndex != index && itemData.Count < maxCount);

            return itemData.ToArray();
        }

        // Tool - 만약 없다가 생긴다면 ToolIndex는 첫 Tool의 Index로 지정
        public void SetToolIndexNext()
        {
            int index = (ToolIndex + 1) % Tools.Count;
            while (Tools[index].IsNullOrEmpty() && ToolIndex != index)
            {
                index = (index + 1) % Tools.Count;
            }

            ToolIndex = index;
        }

        public void SetRightWeaponIndexNext()
        {
            int index = (RightIndex + 1) % Rights.Count;
            while (Rights[index].IsNullOrEmpty() && RightIndex != index)
            {
                index = (index + 1) % Rights.Count;
            }

            // 무기 -> 맨손인 경우
            if (!Rights[index].IsNullOrEmpty() && index == RightIndex)
            {
                index = (index + 1) % Rights.Count;
            }
            // 맨손 -> 맨손인 경우
            else if (Rights[index].IsNullOrEmpty() && index == RightIndex)
            {
                index = 1;
            }

            RightIndex = index;
        }

        public void SetLeftWeaponIndexNext()
        {
            int index = (LeftIndex + 1) % Lefts.Count;
            while (Lefts[index].IsNullOrEmpty() && LeftIndex != index)
            {
                index = (index + 1) % Lefts.Count;
            }

            // 무기 -> 맨손인 경우
            if (!Lefts[index].IsNullOrEmpty() && index == LeftIndex)
            {
                index = (index + 1) % Lefts.Count;
            }
            // 맨손 -> 맨손인 경우
            else if (Lefts[index].IsNullOrEmpty() && index == LeftIndex)
            {
                index = 1;
            }

            LeftIndex = index;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}