using System;
using UnityEngine;

namespace UI.Base
{
    /// <summary>
    /// UI Entity는 Container 및 Container 내부의 Slot을 관리합니다.
    /// UI는 Stack으로 관리됩니다.
    /// </summary>
    public abstract class BaseUIEntity : MonoBehaviour
    {
        [SerializeField] private GameObject uiPanel;
        // Stack Push & Pop by Manager
        
        private Action<BaseUIEntity> _pushAction;
        private Action _popAction;

        protected virtual GameObject panel => uiPanel == null ? gameObject : uiPanel;

        protected virtual UIInput GetUIInput()
        {
            return null;
        }
        
        // public virtual void Initialize()
        // {
        //     
        // }
        
        public void Push()
        {
            _pushAction?.Invoke(this);
        }

        protected void Pop()
        {
            _popAction?.Invoke();
        }
        
        public virtual void OpenOrLoad()
        {
            //Debug.Log($"Open {panel.name}");
            panel.SetActive(true);
        }

        public virtual void Close(bool isSelectClear = true)
        {
            //Debug.Log($"Close {panel.name}");
            panel.SetActive(false);
        }
        
        public void SetOnPush(Action<BaseUIEntity> push)
        {
            _pushAction = push;
        }
        
        public void SetOnPop(Action pop)
        {
            _popAction = pop;
        }
        
        /// <summary>
        /// Travel Connected UIEntity, if it's leaf do not implement
        /// </summary>
        public virtual void Travel(Action<BaseUIEntity> action)
        {
            action?.Invoke(this);
        }

        #region Input
        public void OnInput(InputType inputType)
        {
            if(!IsInputActive(inputType)) return;
            
            var input = GetUIInput();
            
            switch (inputType)
            {
                case InputType.Right:
                    input.RightArrow?.Invoke();
                    break;
                case InputType.Left:
                    input.LeftArrow?.Invoke();
                    break;
                case InputType.Up:
                    input.UpArrow?.Invoke();
                    break;
                case InputType.Down:
                    input.DownArrow?.Invoke();
                    break;
                case InputType.Decision:
                    input.Decision?.Invoke();
                    break;
                case InputType.SlotLeft:
                    input.SlotLeft?.Invoke();
                    break;
                case InputType.SlotRight:
                    input.SlotRight?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null);
            }
        }
        
        public bool IsInputActive(InputType inputType)
        {
            var uiInput = GetUIInput();
            if (uiInput == null || !uiInput.Enable) return false;
            
            return inputType switch
            {
                InputType.Right => uiInput.IsRightArrowActive != null && uiInput.IsRightArrowActive(),
                InputType.Left => uiInput.IsLeftArrowActive != null && uiInput.IsLeftArrowActive(),
                InputType.Up => uiInput.IsUpArrowActive != null && uiInput.IsUpArrowActive(),
                InputType.Down => uiInput.IsDownArrowActive != null && uiInput.IsDownArrowActive(),
                InputType.Decision => uiInput.IsDecisionActive != null && uiInput.IsDecisionActive(),
                InputType.SlotLeft => uiInput.IsSlotLeftActive != null && uiInput.IsSlotLeftActive(),
                InputType.SlotRight => uiInput.IsSlotRightActive != null && uiInput.IsSlotRightActive(),
                _ => throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null)
            };
        }

        public bool IsInputInterrupt(InputType inputType)
        {
            var uiInput = GetUIInput();
            if (uiInput == null || !uiInput.Enable) return false;

            return inputType switch
            {
                InputType.Right => uiInput.IsRightArrowInterrupt && uiInput.IsRightArrowActive != null && uiInput.IsRightArrowActive(),
                InputType.Left => uiInput.IsLeftArrowInterrupt && uiInput.IsLeftArrowActive != null && uiInput.IsLeftArrowActive(),
                InputType.Up => uiInput.IsUpArrowInterrupt && uiInput.IsUpArrowActive != null && uiInput.IsUpArrowActive(),
                InputType.Down => uiInput.IsDownArrowInterrupt && uiInput.IsDownArrowActive != null && uiInput.IsDownArrowActive(),
                InputType.Decision => uiInput.IsDecisionInterrupt && uiInput.IsDecisionActive != null && uiInput.IsDecisionActive(),
                InputType.SlotLeft => uiInput.IsSlotLeftInterrupt && uiInput.IsSlotLeftActive != null && uiInput.IsSlotLeftActive(),
                InputType.SlotRight => uiInput.IsSlotRightInterrupt && uiInput.IsSlotRightActive != null && uiInput.IsSlotRightActive(),
                _ => throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null)
            };
        }
        #endregion
    }
}
