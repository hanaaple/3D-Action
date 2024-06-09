using System.Collections.Generic;
using UI.Entity.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    // 1. UI 입력은 각자 받을 것이 몇몇 존재한다.
    // 기본 슬롯, 인벤토리 슬롯 타입 변경 등

    // 2. UI 입력은 최상단 Stack에서만 이루어져야한다.

    // 여기서 stack이란 PlayerUIManager에 존재한다.
    // -> 각 UI Entity에서 인풋 가능 여부에 따라 실행하던가
    // -> 인풋에 따른 액션을 구현해놓고 PlayerUIManager에서 실행하던가



    /// <summary>
    /// 1. Stack으로 UI Entity를 관리한다.
    /// 2. UI Entity에 대한 Input을 받는다
    /// </summary>
    public class PlayUIManager : MonoBehaviour
    {
        [SerializeField] private UIContainerEntity[] baseUIEntities;
        [SerializeField] private UIContainerEntity rootUIEntity;
        [SerializeField] private PlayerInput playerInput;

        private Stack<UIContainerEntity> _stack;

        private void Start()
        {
            _stack = new Stack<UIContainerEntity>();

            rootUIEntity.Travel(uiEntity => { uiEntity.PushAction = Push; });
            rootUIEntity.Travel(uiEntity => { uiEntity.PopAction = Pop; });
        }

        private void Push(UIContainerEntity uiContainerEntity)
        {
            _stack.Push(uiContainerEntity);
            uiContainerEntity.Open();
        }

        private void Pop()
        {
            OnClose(null);
        }

        private void UpdateState()
        {
            if (_stack.Count > 0)
            {
                Cursor.lockState = CursorLockMode.None;
                playerInput.SwitchCurrentActionMap("Move");
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                playerInput.SwitchCurrentActionMap("Player");
            }
        }

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
                Push(rootUIEntity);
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
                    uiEntity.Open();
                }
            }

            UpdateState();
        }

        public void OnDecision(InputValue inputValue)
        {
            if (_stack.Count == 0)
            {
                foreach (var uiEntity in baseUIEntities)
                {
                    uiEntity.OnClick();
                }
            }
            else
            {
                var entity = _stack.Peek();
                entity.OnClick();
            }
        }

        public void OnChangeRightWeapon(InputValue inputValue)
        {
            if (_stack.Count == 0)
            {
                foreach (var uiEntity in baseUIEntities)
                {
                    uiEntity.OnRightArrow();
                }
            }
            else
            {
                var entity = _stack.Peek();
                entity.OnRightArrow();
            }
        }

        public void OnChangeLeftWeapon(InputValue inputValue)
        {
            if (_stack.Count == 0)
            {
                foreach (var uiEntity in baseUIEntities)
                {
                    uiEntity.OnLeftArrow();
                }
            }
            else
            {
                var entity = _stack.Peek();
                entity.OnLeftArrow();
            }
        }

        public void OnChangeItem(InputValue inputValue)
        {
            if (_stack.Count == 0)
            {
                foreach (var uiEntity in baseUIEntities)
                {
                    uiEntity.OnDownArrow();
                }
            }
            else
            {
                var entity = _stack.Peek();
                entity.OnDownArrow();
            }
        }
#endif
    }
}