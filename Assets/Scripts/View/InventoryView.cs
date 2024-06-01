using System.ComponentModel;
using UnityEngine;

namespace View
{
    public class InventoryView : MonoBehaviour
    {
        private void Start()
        {
            // _playerViewModel.OnOwnedItemDataChange += UpdateInventoryView;
            // _playerViewModel.OnEquippedItemDataChange += UpdateSlotUI;
            
            // 장착 중인 스킬 정보
        }
        
        private void UpdateInventoryView(object sender, PropertyChangedEventArgs e)
        {
            // DataManager.instance.ownedItemData 기반으로 Update Equip View
        }
        
        // Slot.OnSelectSlot -> Describe
    }
}