using System;
using System.Collections.Generic;
using CharacterControl.State;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterControl
{
    [Serializable]
    public struct InputBufferData
    {
        public InputBufferData(Type type, float pressedTime)
        {
            Type = type;
            this.pressedTime = pressedTime;
            name = type.Name;
        }

        // For Debug
        [HideInInspector] public string name;
        public Type Type;
        public float pressedTime;
    }

    public class InputStateHandler : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool run;
        public bool crouch;
        public bool lockOnOff;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        public float inputBufferThreshold = 0.5f;

        // 입력된 시간
        private readonly Queue<InputBufferData> _inputBuffer = new();
        
        // 추후 디버깅 가능한 Queue 구현
        [SerializeField] private List<InputBufferData> debuggingInputBuffer;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnRun(InputValue value)
        {
            RunInput(value.isPressed);
        }

        public void OnCrouch(InputValue value)
        {
            CrouchInput(value.isPressed);
        }

        public void OnLock(InputValue value)
        {
            LockInput(value.isPressed);
        }

        public void OnRolling(InputValue value)
        {
            RollInput();
        }

        public void OnJump(InputValue value)
        {
            JumpInput();
        }

        public void OnAttack(InputValue value)
        {
            AttackInput();
        }

        public void OnStrongAttack(InputValue value)
        {
            StrongAttackInput(value.isPressed);
        }

        public void OnRightHandChange(InputValue value)
        {
            RightHandChangeInput();
        }

        public void OnLeftHandChange(InputValue value)
        {
            LeftHandChangeInput();
        }
#endif


        private void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        private void LookInput(Vector2 newLookDelta)
        {
            look = newLookDelta;
        }

        private void RunInput(bool newRunState)
        {
            run = newRunState;
        }

        private void RollInput()
        {
            // Debug.Log("Add Roll Inpu");
            _inputBuffer.Enqueue(new InputBufferData(typeof(RollState), Time.unscaledTime));
            debuggingInputBuffer.Add(new InputBufferData(typeof(RollState), Time.unscaledTime));
        }

        private void JumpInput()
        {
            _inputBuffer.Enqueue(new InputBufferData(typeof(JumpState), Time.unscaledTime));
            debuggingInputBuffer.Add(new InputBufferData(typeof(JumpState), Time.unscaledTime));
        }

        private void CrouchInput(bool newCrouchState)
        {
            crouch = newCrouchState;
        }

        private void LockInput(bool newLockState)
        {
            lockOnOff = newLockState;
        }

        private void AttackInput()
        {
            _inputBuffer.Enqueue(new InputBufferData(typeof(AttackState), Time.unscaledTime));
            debuggingInputBuffer.Add(new InputBufferData(typeof(AttackState), Time.unscaledTime));
        }

        private void StrongAttackInput(bool newAttackState)
        {
            if (newAttackState)
            {
                _inputBuffer.Enqueue(new InputBufferData(typeof(StrongAttackState), Time.unscaledTime));
                debuggingInputBuffer.Add(new InputBufferData(typeof(StrongAttackState), Time.unscaledTime));
            }
            else
            {
                // 차지 중인 경우 -> 차지를 멈추고 공격
                // 차지하다가 자동으로 실행된 경우에는?
                // _inputBuffer.Enqueue(new InputBufferData(ActionType.StrongAttack, Time.unscaledTime));
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private static void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void LeftHandChangeInput()
        {
            _inputBuffer.Enqueue(new InputBufferData(typeof(LeftHandChangeState), Time.unscaledTime));
            debuggingInputBuffer.Add(new InputBufferData(typeof(LeftHandChangeState), Time.unscaledTime));
        }

        private void RightHandChangeInput()
        {
            _inputBuffer.Enqueue(new InputBufferData(typeof(RightHandChangeState), Time.unscaledTime));
            debuggingInputBuffer.Add(new InputBufferData(typeof(RightHandChangeState), Time.unscaledTime));
        }

        // Set Script Order 앞으로
        private void Update()
        {
            while (true)
            {
                if (_inputBuffer.TryPeek(out var bufferData))
                {
                    if (bufferData.pressedTime + inputBufferThreshold < Time.unscaledTime)
                    {
                        _inputBuffer.Dequeue();
                        debuggingInputBuffer.RemoveAt(0);
                        continue;
                    }
                }

                break;
            }
        }


        public InputBufferData TryDeQueue()
        {
            _inputBuffer.TryDequeue(out var inputBufferData);
            return inputBufferData;
        }
        
        public InputBufferData DeQueue()
        {
            if (!HasBuffer()) throw new Exception("InputBuffer is Empty");
            debuggingInputBuffer.RemoveAt(0);
            // Debug.Log($"{string.Join(", ", _inputBuffer.Select(item => item.Type))}");
            return _inputBuffer.Dequeue();
        }

        public InputBufferData Peek()
        {
            if (!HasBuffer()) throw new Exception("InputBuffer is Empty");
            return _inputBuffer.Peek();
        }
        
        public bool HasBuffer()
        {
            //Debug.Log(_inputBuffer.Count);
            return _inputBuffer.Count > 0;
        }
    }
}