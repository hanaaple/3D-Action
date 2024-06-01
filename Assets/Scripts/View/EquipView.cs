using System.ComponentModel;
using UnityEngine;

namespace View
{
    public class EquipView : MonoBehaviour
    {
        
        // EquippedItemDataChange -> EquipmentView
        private void Start()
        {
            // _playerUIViewModel.OnEquippedDataChange += UpdateUI;
            // _playerUIViewModel.OnSelectionChange += UpdateDescribeUI;
        }
        
        // DataManager.instance.equippedItemData를 기반으로 Update Equip View
        private void UpdateUI(object sender, PropertyChangedEventArgs e)
        {
            // equippedItemData를 기반으로 Update Equipped Slot
            // 
        }
        
        // Slot.OnSelectSlot -> Describe
    }
}