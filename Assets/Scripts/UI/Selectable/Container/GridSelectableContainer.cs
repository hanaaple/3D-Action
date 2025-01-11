using System;
using System.Collections.Generic;
using System.Linq;
using UI.Selectable.Slot;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;

namespace UI.Selectable.Container
{
    [Serializable]
    public struct SelectableSlotRow
    {
        public SelectableSlotRow(int count)
        {
            selectableSlots = new SelectableSlot[count];
        }

        [FormerlySerializedAs("_selectableSlots")] [SerializeField] private SelectableSlot[] selectableSlots;
        public ReadOnlyArray<SelectableSlot> SelectableSlots => selectableSlots;
        public int Length => selectableSlots.Length;
        public SelectableSlot this[int index] => selectableSlots[index];

        public void SetSlot(SelectableSlot slotInstance, int index)
        {
            selectableSlots[index] = slotInstance;
        }
    }

    /// <summary>
    /// Left Up (0, 0) 기준으로 자동 정렬된 상태라 가정 후 계산됨.
    /// </summary>
    public class GridSelectableContainer : SelectableSlotContainer
    {
        [SerializeField] protected List<SelectableSlotRow> selectableSlotRows;
        
        [SerializeField] private IndexingOverFlowMode indexingMode;
        
        protected override void Awake()
        {
            base.Awake();
            
            for (var y = 0; y < selectableSlotRows.Count; y++)
            {
                var selectableSlotRow = selectableSlotRows[y];

                for (int x = 0; x < selectableSlotRow.Length; x++)
                {
                    InitializeNavigation(y, x);
                }
            }
        }

        protected void InitializeNavigation(int y, int x)
        {
            var curSlot = selectableSlotRows[y][x];
            
            //Debug.Log($"{y}:{x}");
            
            var leftSlot = GetLeftSlot(y, x);
            var rightSlot = GetRightSlot(y, x);
            var upSlot = GetUpSlot(y, x);
            var downSlot = GetDownSlot(y, x);
                    
            curSlot.up = upSlot;
            curSlot.down = downSlot;
            curSlot.left = leftSlot;
            curSlot.right = rightSlot;
        }

        public override List<SelectableSlot> GetSelectableSlots()
        {
            return selectableSlotRows.SelectMany(item => item.SelectableSlots).ToList();
        }
        
        private SelectableSlot GetLeftSlot(int y, int x)
        {
            if (selectableSlotRows[y][x].navigationType == NavigationType.Explicit)
            {
                return selectableSlotRows[y][x].left;
            }
            
            if (selectableSlotRows[y].Length > 1)
            {
                var prevIndex = x - 1;
                if (prevIndex < 0)
                {
                    if (indexingMode == IndexingOverFlowMode.Bounded)
                    {
                        prevIndex = 0;
                    }
                    else if (indexingMode == IndexingOverFlowMode.Circular)
                    {
                        prevIndex = selectableSlotRows[y].Length - 1;
                    }
                }

                x = prevIndex;
            }

            return selectableSlotRows[y][x];
        }
        
        private SelectableSlot GetRightSlot(int y, int x)
        {
            if (selectableSlotRows[y][x].navigationType == NavigationType.Explicit)
            {
                return selectableSlotRows[y][x].right;
            }

            if (selectableSlotRows[y].Length > 1)
            {
                var nextIndex = x + 1;

                if (nextIndex >= selectableSlotRows[y].Length)
                {
                    if (indexingMode == IndexingOverFlowMode.Bounded)
                    {
                        nextIndex = selectableSlotRows[y].Length - 1;
                    }
                    else if (indexingMode == IndexingOverFlowMode.Circular)
                    {
                        nextIndex = 0;
                    }   
                }

                x = nextIndex;
            }

            return selectableSlotRows[y][x];
        }
        
        private SelectableSlot GetUpSlot(int y, int x)
        {
            if (selectableSlotRows[y][x].navigationType == NavigationType.Explicit)
            {
                return selectableSlotRows[y][x].up;
            }
            
            if (selectableSlotRows.Count > 1)
            {
                var prevIndex = y - 1;
                if (prevIndex < 0)
                {
                    if (indexingMode == IndexingOverFlowMode.Bounded)
                    {
                        prevIndex = 0;
                    }
                    else if (indexingMode == IndexingOverFlowMode.Circular)
                    {
                        prevIndex = selectableSlotRows.Count - 1;
                    }
                }
                
                y = prevIndex;

                if (selectableSlotRows[y].Length <= x)
                {
                    x = selectableSlotRows[y].Length - 1; 
                }
            }
            
            return selectableSlotRows[y][x];
        }
        
        private SelectableSlot GetDownSlot(int y, int x)
        {
            if (selectableSlotRows[y][x].navigationType == NavigationType.Explicit)
            {
                return selectableSlotRows[y][x].down;
            }
            
            if (selectableSlotRows.Count > 1)
            {
                var nextIndex = y + 1;
                if (nextIndex >= selectableSlotRows.Count)
                {
                    if (indexingMode == IndexingOverFlowMode.Bounded)
                    {
                        nextIndex = selectableSlotRows.Count - 1;
                    }
                    else if (indexingMode == IndexingOverFlowMode.Circular)
                    {
                        nextIndex = 0;
                    }
                }
                
                y = nextIndex;

                if (selectableSlotRows[y].Length <= x)
                {
                    x = selectableSlotRows[y].Length - 1; 
                }
            }
            
            return selectableSlotRows[y][x];
        }
    }
}