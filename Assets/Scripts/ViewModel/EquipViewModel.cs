using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Item;
using Model;
using UnityEngine.InputSystem.Utilities;

namespace ViewModel
{
    // 장착 아이템 ViewModel
    public class EquipViewModel : INotifyPropertyChanged
    {
        private EquippedItemData _equippedItemData;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public Weapon[] Lefts
        {
            get => _equippedItemData.lefts;
        }
        
        public Weapon[] Rights
        {
            get => _equippedItemData.rights;
        }

        private Item.Item[] Tools
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
        
        public void Initialize(EquippedItemData equippedItemData)
        {
            _equippedItemData = equippedItemData;
            _equippedItemData.PropertyChanged += (send, e) => OnPropertyChanged(e.PropertyName);
        }

        public Weapon GetCurrentLeftWeapon()
        {
            return Lefts[LeftIndex];
        }
        
        public Weapon GetCurrentRightWeapon()
        {
            return Rights[RightIndex];
        }
        
        public ReadOnlyArray<Item.Item> GetTools(int maxCount)
        {
            List<Item.Item> items = new List<Item.Item>();
            
            int index = ToolIndex;
            
            do
            {
                if (Tools[index] != null)
                {
                    items.Add(Tools[index]);
                }
                
                index = (index + 1) % Tools.Length;
            }
            while (ToolIndex != index && items.Count < maxCount);
            return items.ToArray();
        }
        
        // Tool - 만약 없다가 생긴다면 ToolIndex는 첫 Tool의 Index로 지정
        public void SetToolIndexNext()
        {
            int index = (ToolIndex + 1) % Tools.Length;
            while (Tools[index] == null && ToolIndex != index)
            {
                index = (index + 1) % Tools.Length;
            }

            ToolIndex = index;
        }
        
        public void SetRightWeaponIndexNext()
        {
            int index = (RightIndex + 1) % Rights.Length;
            while (Rights[index] == null && RightIndex != index)
            {                
                index = (index + 1) % Rights.Length;
            }

            // 무기 -> 맨손인 경우
            if (Rights[index] != null && index == RightIndex)
            {
                index = (index + 1) % Rights.Length;
            }
            // 맨손 -> 맨손인 경우
            else if (Rights[index] == null && index == RightIndex)
            {
                index = 1;
            }

            RightIndex = index;
        }

        public void SetLeftWeaponIndexNext()
        {
            int index = (LeftIndex + 1) % Lefts.Length;
            while (Lefts[index] == null && LeftIndex != index)
            {                
                index = (index + 1) % Lefts.Length;
            }

            // 무기 -> 맨손인 경우
            if (Lefts[index] != null && index == LeftIndex)
            {
                index = (index + 1) % Lefts.Length;
            }
            // 맨손 -> 맨손인 경우
            else if (Lefts[index] == null && index == LeftIndex)
            {
                index = 1;
            }

            LeftIndex = index;
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}