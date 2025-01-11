using System.Collections.Generic;
using Data.Item.Base;
using Data.Play;

namespace Data.ViewModel
{
    // 보유 중인 아이템 ViewModel
    public sealed class OwnedItemViewModel
    {
        public delegate void ChangeItemEventHandler(BaseItem item, bool isAdded);
        
        private OwnedItemData _ownedItemData;
        
        public event ChangeItemEventHandler PropertyChanged;

        public OwnedItemViewModel(OwnedItemData ownedItemData)
        {
            _ownedItemData = ownedItemData;
        }

        public OwnedItemData ownedItemData => _ownedItemData;

        // TODO: 맨손 등의 아이템에 대해서는 어떻게 해야되는가.
        // 성능에 대해서는 장담하지 못한다.
        public List<BaseItem> GetItems(string comparision)
        {
            var list = _ownedItemData.Items.FindAll(item => item.Equals(comparision));
            return list;
        }
        
        // public List<BaseItem> GetItems(ContainerEnumType containerEnumType, string key)
        // {
        //     // 중복은?
        //     var list = new List<BaseItem>();
        //     if (containerEnumType.HasFlag(ContainerEnumType.ItemType))
        //     {
        //         if (Enum.TryParse<ItemType>(key, out var itemType))
        //         {
        //             list.AddRange(GetItemsByItemType(itemType));
        //         }
        //     }
        //
        //     if (containerEnumType.HasFlag(ContainerEnumType.EquipItem))
        //     {
        //         if (Enum.TryParse<EquipType>(key, out var equipType))
        //         {
        //             list.AddRange(GetItemsByEquipType(equipType));
        //         }
        //     }
        //
        //     if (containerEnumType.HasFlag(ContainerEnumType.ItemDetailType))
        //     {
        //         list.AddRange(GetItemsByItemDetailType(key));
        //     }
        //
        //     return list;
        // }
        
        public List<BaseItem> GetItemsByItemType(ItemType itemType)
        {
            var list = _ownedItemData.Items.FindAll(item => item.itemType == itemType);
            return list;
        }
        
        // Katana, Fist, ...
        public List<BaseItem> GetItemsByItemDetailType(string detailType)
        {
            var list = _ownedItemData.Items.FindAll(item => item.GetItemDetailType() == detailType);
            return list;
        }
        
        /// <param name="item"> this is item data (not item instance) </param>
        public T AddItem<T>(T item) where T : BaseItem
        {
            var clone = item.Clone();
            _ownedItemData.AddItem(clone);
            
            OnPropertyChanged(clone, true);
            
            return clone as T;
        }

        public void RemoveItem(BaseItem item)
        {
            _ownedItemData.RemoveItem(item);
            OnPropertyChanged(item, false);
        }
        
        private void OnPropertyChanged(BaseItem item, bool isAdded)
        {
            PropertyChanged?.Invoke(item, isAdded);
        }

        // public T GetItemByInstanceId<T>(string instanceId) where T : BaseItem
        // {
        //     var items = new List<BaseItem>((IEnumerable<BaseItem>)_ownedItemData.Items[typeof(T)]);
        //     return items.Find(item => item.instanceId == instanceId) as T;
        //
        //     // return _ownedItemData.Items[typeof(T)].Find(item => item.instanceId == instanceId);
        // }

        // public IReadOnlyList<Weapon> GetAllWeapons()
        // {
        //     return _ownedItemData.weapons;
        // }
        //
        // public IReadOnlyList<Armor> GetAllArmors()
        // {
        //     return _ownedItemData.armors;
        // }
        //
        // public IReadOnlyList<Accessory> GetAllAccessories()
        // {
        //     return _ownedItemData.accessories;
        // }
        //
        // public IReadOnlyList<Tool> GetAllTools()
        // {
        //     return _ownedItemData.tools;
        // }
        
        // public Armor GetArmor(string instanceId)
        // {
        //     return _ownedItemData.armors.Find(item => item.instanceId == instanceId);
        // }
    }
}