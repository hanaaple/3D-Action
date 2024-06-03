using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Item;
using UnityEngine;

namespace Model
{
    // 장착 중인 장비 데이터
    [Serializable]
    public class EquippedItemData : INotifyPropertyChanged
    {
        // null로 비교하지 말고 함수로 비교하게 변경
        public Weapon[] lefts;
        public Weapon[] rights;
        public Accessory[] accessories;
        // 방어구 부위마다 나눠야됨
        public Armor[] armors;
        public Tool[] tools;

        // 현재 선택 or 장착된 아이템의 Index
        [SerializeField] private int leftIndex;
        [SerializeField] private int rightIndex;
        [SerializeField] private int toolIndex;

        public Tool[] Tools
        {
            get => tools;
        }
        
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

        public EquippedItemData(int leftCapacity, int rightCapacity, int accessoryCapacity, int armorCapacity, int toolCapacity)
        {
            lefts = new Weapon[leftCapacity];
            rights = new Weapon[rightCapacity];
            accessories = new Accessory[accessoryCapacity];
            armors = new Armor[armorCapacity];
            tools = new Tool[toolCapacity];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void EquipWeapon(Weapon weapon, int index)
        {
            // weapons[index] = item as Weapon;
            OnPropertyChanged();
        }

        public void UnEquipWeapon(int index)
        {
            // weapons[index] = null;

            OnPropertyChanged();
        }
        
        public void EquipItem<T>(T item, int index) where T : Item.Item
        {
            if (typeof(T) == typeof(Accessory))
            {
                accessories[index] = item as Accessory;
            }
            else if (typeof(T) == typeof(Armor))
            {
                armors[index] = item as Armor;
            }
            else if (typeof(T) == typeof(Tool))
            {
                tools[index] = item as Tool;
            }
            else
            {
                Debug.LogError($"{typeof(T)}은 장착 가능한 Item이 아닙니다.");
            }

            OnPropertyChanged(nameof(T));
        }

        public void UnEquipItem<T>(int index) where T : Item.Item
        {
            if (typeof(T) == typeof(Accessory))
            {
                accessories[index] = null;
            }
            else if (typeof(T) == typeof(Armor))
            {
                armors[index] = null;
            }
            else if (typeof(T) == typeof(Tool))
            {
                tools[index] = null;
            }
            else
            {
                Debug.LogError($"{typeof(T)}은 탈착 가능한 Item이 아닙니다.");
            }

            OnPropertyChanged(nameof(T));
        }
        
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}