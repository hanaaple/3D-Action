using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data.PlayItem;
using UnityEngine;

namespace Data.Play
{
    public enum WeaponEquipType
    {
        Left,
        Right
    }
    
    /// <summary>
    /// 장착 중인 장비 데이터
    /// </summary>
    [Serializable]
    public class EquippedItemData : INotifyPropertyChanged
    {
        [SerializeField] private Weapon[] lefts;
        [SerializeField] private Weapon[] rights;
        [SerializeField] private Accessory[] accessories;
        [SerializeField] private Armor helmet;
        [SerializeField] private Armor breastplate;
        [SerializeField] private Armor leggings;
        [SerializeField] private Armor shoes;
        [SerializeField] private Tool[] tools;

        // 현재 선택 or 장착된 아이템의 Index
        [SerializeField] private int leftIndex;
        [SerializeField] private int rightIndex;
        [SerializeField] private int toolIndex;

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        public Weapon[] Lefts => lefts;
        public Weapon[] Rights => rights;
        
        public Accessory[] Accessories => accessories;
        
        public Armor Helmet => helmet;
        public Armor Breastplate => breastplate;
        public Armor Leggings => leggings;
        public Armor Shoes => shoes;

        public Tool[] Tools => tools;
        
        public int ToolIndex
        {
            get => toolIndex;
            set
            {
                if (toolIndex == value) return;

                toolIndex = value;
                OnPropertyChanged();
            }
        }

        public int LeftIndex
        {
            get => leftIndex;
            set
            {
                if (leftIndex == value) return;

                leftIndex = value;
                OnPropertyChanged();
            }
        }

        public int RightIndex
        {
            get => rightIndex;
            set
            {
                if (rightIndex == value) return;

                rightIndex = value;
                OnPropertyChanged();
            }
        }

        public EquippedItemData(int leftCapacity, int rightCapacity, int accessoryCapacity, int toolCapacity)
        {
            lefts = new Weapon[leftCapacity];
            rights = new Weapon[rightCapacity];
            accessories = new Accessory[accessoryCapacity];
            tools = new Tool[toolCapacity];
        }

        public void EquipWeapon(int index, Weapon weapon, WeaponEquipType equipType)
        {
            if (equipType == WeaponEquipType.Left)
            {
                lefts[index] = weapon;
            }
            else if (equipType == WeaponEquipType.Right)
            {
                rights[index] = weapon;
            }

            OnPropertyChanged();
        }
        
        public void SetHelmet(Armor armor)
        {
            helmet = armor;
            OnPropertyChanged();
        }
        
        public void SetBreastPlate(Armor armor)
        {
            breastplate = armor;
            OnPropertyChanged();
        }
        
        public void SetLeggings(Armor armor)
        {
            leggings = armor;
            OnPropertyChanged();
        }
        
        public void SetShoes(Armor armor)
        {
            shoes = armor;
            OnPropertyChanged();
        }

        public void SetAccessory(int index, Accessory accessory)
        {
            accessories[index] = accessory;
            OnPropertyChanged();
        }

        public void SetTool(int index, Tool tool)
        {
            tools[index] = tool;
            OnPropertyChanged();
        }
        
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}