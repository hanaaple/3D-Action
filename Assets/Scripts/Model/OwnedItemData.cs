using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Item;

namespace Model
{
    // 왼손 무기
    // 오른손 무기
    // + 화살, 볼트?
    // 방어구
    // 악세사리
    // 사용 아이템

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
            
            // 그런데 Weapon은 중복 불허잖씀
            
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}