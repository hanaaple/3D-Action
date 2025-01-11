using System.Collections.Generic;
using UI.Base;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace UI
{    
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    
    public enum InputType
    {
        Left,
        Right,
        Up,
        Down,
        Decision,
        SlotLeft,
        SlotRight
    }
    /// <summary>
    /// 1. Stack으로 UI Entity를 관리한다.
    /// 2. UI Entity에 대한 Input을 받는다
    /// </summary>
    
    
    // 문제 1. Selectable에 의해 다른 곳을 클릭하면 Select가 해제된다
    // 문제 2. Navigation 시스템은 어차피 설정해줘야된다. 자동으로 해봤자 오히려 불편해진다.
    
    // 그냥 싱글톤으로 할까 너무 불편한데
    public class UIManager : MonoBehaviour
    {
        [Header("인풋 우선순위에 따라 넣으세요")] [SerializeField]
        private BaseUIEntity[] baseUIEntities;

        [FormerlySerializedAs("rootUIEntity")] [SerializeField] protected BaseUIEntity rootBaseUIEntity;

        private PlayerInput _playerInput;
        private Stack<BaseUIEntity> _stack;

        public void Initialize(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }
        
        protected virtual void Awake()
        {
            _stack = new Stack<BaseUIEntity>();
            
            rootBaseUIEntity.Travel(uiEntity => { uiEntity.SetOnPush(Push); });
            rootBaseUIEntity.Travel(uiEntity => { uiEntity.SetOnPop(Pop); });
        }

        protected void Push(BaseUIEntity baseUIEntity)
        {
            // Debug.Log($"Push {uiContainerEntity.name}");
            if (_stack.Count > 0)
            {
                var uiEntity = _stack.Peek();
                uiEntity.Close(isSelectClear: false); // Select가 삭제되면 안되는것도 있음. 아니, Select가 삭제되더라도 기존의 것은 유지되어야됨.
            }

            _stack.Push(baseUIEntity);
            baseUIEntity.OpenOrLoad();
        }

        private void Pop()
        {
            OnClose(null);
        }

        protected virtual void UpdateState()
        {
            if (_stack.Count > 0)
            {
                Cursor.lockState = CursorLockMode.None;
                _playerInput.SwitchCurrentActionMap("Move");
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                _playerInput.SwitchCurrentActionMap("Player");
            }
        }

        
        // TODO: 입력 관련하여 현 상황은 그리 좋다고 판단되지 않음. 단, 최초 개발 의도와 동일한 작동을 하기에 바꾸지 않는다.
        // Player Input의 Send Messages에 의해 작동한다.
        // 이를 변경하기 위해 람다 혹은 함수로 사용해야한다.
        
#if ENABLE_INPUT_SYSTEM
        public void OnEsc(InputValue inputValue)
        {
            if (_stack.Count > 0)
            {
                while (_stack.TryPop(out var uiEntity))
                {
                    uiEntity.Close();
                }
            }
            else
            {
                Push(rootBaseUIEntity);
            }

            UpdateState();
        }

        public void OnClose(InputValue inputValue)
        {
            if (_stack.TryPop(out var oldUiEntity))
            {
                oldUiEntity.Close();
                
                if (_stack.TryPeek(out var uiEntity))
                {
                    uiEntity.OpenOrLoad();
                }
            }

            UpdateState();
        }

        public void OnDecision(InputValue inputValue)
        {
            OnInput(InputType.Decision);
        }

        public void OnRightArrow(InputValue inputValue)
        {
            OnInput(InputType.Right);
        }

        public void OnLeftArrow(InputValue inputValue)
        {
            OnInput(InputType.Left);
        }

        public void OnDownArrow(InputValue inputValue)
        {
            OnInput(InputType.Down);
        }
        
        public void OnUpArrow(InputValue inputValue)
        {
            OnInput(InputType.Up);
        }

        public void OnSlotLeft(InputValue inputValue)
        {
            OnInput(InputType.SlotLeft);
        }
        
        public void OnSlotRight(InputValue inputValue)
        {
            OnInput(InputType.SlotRight);
        }
        
        public bool IsLeftArrowInterrupt()
        {
            return IsInputInterrupt(InputType.Left);
        }

        public bool IsRightArrowInterrupt()
        {
            return IsInputInterrupt(InputType.Right);
        }
        
        // public bool DownArrowInterrupt()
        // {
        //     return IsInputEnable(InputType.Down);
        // }
        //
        // public bool UpArrowInterrupt()
        // {
        //     return IsInputEnable(InputType.Up);
        // }
        
        public bool IsDecisionInterrupt()
        {
            return IsInputInterrupt(InputType.Decision);
        }

        private void OnInput(InputType inputType)
        {
            if (_stack.Count == 0)
            {
                foreach (var uiEntity in baseUIEntities)
                {
                    if (uiEntity.IsInputActive(inputType))
                    {
                        uiEntity.OnInput(inputType);
                        break;
                    }
                }
            }
            else
            {
                var uiEntity = _stack.Peek();
                uiEntity.OnInput(inputType);
            }
        }
        
        
        // UI의 Input이 가능한 경우 다른 Input을 받지 않기 위해 사용  -> UI Input이 가능해도 상관없는 경우가 있음.
        private bool IsInputInterrupt(InputType inputType)
        {
            if (_stack.Count > 0) return true;

            foreach (var uiEntity in baseUIEntities)
            {
                if (uiEntity.IsInputInterrupt(inputType))
                    return true;
            }

            return false;
        }

#endif
    }
}