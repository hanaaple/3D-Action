using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Item;

namespace Model
{
    // 보유 중인 장비 데이터
    [Serializable]
    public class OwnedItemData : INotifyPropertyChanged
    {
        public Weapon[] weapons;
        public Armor[] armor;
        public Accessory[] accessory;
        public Tool[] tools;
        public Valuable[] valuables;

        public void AddItem<T>() where T : Item.Item
        {
            OnPropertyChanged();
        }

        public void RemoveItem<T>() where T : Item.Item
        {
            // 모든 아이템은 그대로 존재해
            // 단, 개수가 0인것은 화면에 보이지 않아
            // 나쁘지 않은데
            
            
            // Tool - 소모품 or 재사용 가능 등에 따라 중복 허가
            // Accessory - 중복 불허
            // Valuable - 중복 불허
            // Armor - 중복 불허
            // Weapon - 중복 불허
            
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}