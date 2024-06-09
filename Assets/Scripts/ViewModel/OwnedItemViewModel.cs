using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data.Play;
using Data.PlayItem;

namespace ViewModel
{
    public sealed class OwnedItemViewModel : INotifyPropertyChanged
    {
        private OwnedItemData _ownedItemData;
        public event PropertyChangedEventHandler PropertyChanged;

        public IReadOnlyList<Weapon> GetAllWeapons()
        {
            return _ownedItemData.weapons;
        }
        
        public IReadOnlyList<Armor> GetAllArmors()
        {
            return _ownedItemData.armors;
        }

        public IReadOnlyList<Accessory> GetAllAccessories()
        {
            return _ownedItemData.accessories;
        }

        public IReadOnlyList<Tool> GetAllTools()
        {
            return _ownedItemData.tools;
        }
        
        public void Initialize(OwnedItemData ownedItemData)
        {
            _ownedItemData = ownedItemData;

            // For Debug
            foreach (var item in _ownedItemData.weapons)
                item.Initialize();
            
            foreach (var item in _ownedItemData.armors)
                item.Initialize();

            foreach (var item in _ownedItemData.accessories)
                item.Initialize();

            foreach (var item in _ownedItemData.tools)
                item.Initialize();

            foreach (var item in _ownedItemData.valuables)
                item.Initialize();

            _ownedItemData.PropertyChanged += (send, e) => OnPropertyChanged(e.PropertyName);
        }

        public void AddItem(Item item)
        {
            var clone = item.Clone();
            clone.Initialize();
            _ownedItemData.AddItem(clone);
        }
        
        public void AddItems(Item[] items)
        {
            foreach (var item in items)
            {
                var clone = item.Clone();
                clone.Initialize();
                _ownedItemData.AddItem(clone);
            }
        }
        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}