using System.Collections.Generic;
using UI.Selectable.Slot;
using UnityEngine;

namespace UI.Selectable.Container
{
    public enum IndexingOverFlowMode
    {
        Circular,
        Bounded
    }
    
    /// <summary>
    /// Input에 따라 Selectable Slot을 관리한다
    /// Slot을 관리하며 UIEntity에 의해 관리됩니다.
    /// </summary>
    public abstract class SelectableSlotContainer : MonoBehaviour
    {
        // Npc merchant (Sale, Purchase, Upgrade)
        
        public bool isSelected => _selectedSlot != null;
        
        private SelectableSlot _selectedSlot;

        public abstract List<SelectableSlot> GetSelectableSlots();
        
        protected virtual void Awake()
        {
            var selectableSlots = GetSelectableSlots();
            // On Add SelectableSlot
            foreach (var selectable in selectableSlots)
            {
                OnAddSelectableSlot(selectable);
            }
        }

        /// <summary>
        /// Add Slot을 Awake에서 실행해주기에 Slot을 임의로 생성한다면, Button을 Clear한 이후 AddListener 실행.
        /// </summary>
        protected virtual void OnAddSelectableSlot(SelectableSlot slot)
        {
            slot.Initialize(OnBeginSelect);
        }
        
        public void Decision()
        {
            _selectedSlot?.Decision();
        }

        public void SelectNext(Direction direction)
        {
            if(_selectedSlot == null) return;

            var nextSlot = _selectedSlot.GetSelectNext(direction);
            if(nextSlot != null && nextSlot.SelectEnable())
                nextSlot.Select();
        }

        public void Select()
        {
            _selectedSlot?.Select();
        }

        public void SelectDefault()
        {
            var selectableSlots = GetSelectableSlots();
            //Debug.LogWarning($"{selectableSlots.Count}, ");
            if(selectableSlots.Count == 0) return;
            //Debug.LogWarning("Try selecting default slot");
            var selectedSlot = selectableSlots[0];
            if(selectedSlot != null && selectedSlot.SelectEnable())
                selectedSlot.Select();
        }
        
        // This use when container is closed
        public void ClearSelectState()
        {
            // DeHighlight? Highlight를 Script에서 관리하는 경우 해줘야됨.
            _selectedSlot?.DeSelect();
            _selectedSlot = null;
        }

        private void OnBeginSelect(SelectableSlot slot)
        {
            if (_selectedSlot == slot) return;
            
            _selectedSlot?.DeSelect();
            _selectedSlot = slot;
            
            //Debug.LogWarning($"Set Select {_selectedSlot.gameObject.name}");
        }

        // Selectable을 상속하여 사용할때 사용되던 것.
        // public abstract void SetInteractable(bool isInteractable);
        //{
        // interactable = isInteractable;
        //}
    }
}