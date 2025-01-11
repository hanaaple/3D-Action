using System.Collections.Generic;
using System.Linq;
using UI.Selectable.Slot;
using UnityEngine;

namespace UI.Selectable.Container
{
    public class VerticalSelectableContainer : SelectableSlotContainer
    {
        public SelectableSlot[] selectableSlots;

        [SerializeField] private IndexingOverFlowMode indexingMode;

        protected override void Awake()
        {
            base.Awake();
            for (var index = 0; index < selectableSlots.Length; index++)
            {
                InitializeNavigation(index);
            }
        }

        private void InitializeNavigation(int index)
        {
            var prevSlot = GetPrevSlot(index);
            var slot = selectableSlots[index];
            var nextSlot = GetNextSlot(index);

            //Debug.Log($"Prev: {prevSlot.gameObject}, Cur: {gameObject},  Next: {nextSlot.gameObject}");

            slot.up = prevSlot;
            slot.down = nextSlot;
        }

        public override List<SelectableSlot> GetSelectableSlots()
        {
            return selectableSlots.ToList();
        }

        private SelectableSlot GetPrevSlot(int index)
        {
            if (selectableSlots[index].navigationType == NavigationType.Explicit)
            {
                return selectableSlots[index].left;
            }
            
            if (selectableSlots.Length <= 1)
            {
                return selectableSlots[index];
            }
            else
            {
                var prevIndex = index - 1;
                if (prevIndex < 0)
                {
                    if (indexingMode == IndexingOverFlowMode.Bounded)
                    {
                        prevIndex = 0;
                    }
                    else if (indexingMode == IndexingOverFlowMode.Circular)
                    {
                        prevIndex = selectableSlots.Length - 1;
                    }
                }
                return selectableSlots[prevIndex];
            }
        }

        private SelectableSlot GetNextSlot(int index)
        {
            if (selectableSlots[index].navigationType == NavigationType.Explicit)
            {
                return selectableSlots[index].right;
            }
            
            if (selectableSlots.Length <= 1)
            {
                return selectableSlots[index];
            }
            else
            {
                var nextIndex = index + 1;
                if (nextIndex >= selectableSlots.Length)
                {
                    if (indexingMode == IndexingOverFlowMode.Bounded)
                    {
                        nextIndex = selectableSlots.Length - 1;
                    }
                    else if (indexingMode == IndexingOverFlowMode.Circular)
                    {
                        nextIndex = 0;
                    }
                }
                return selectableSlots[nextIndex];
            }
        }
    }
}