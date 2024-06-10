using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl.State.Base
{
    /// <summary>
    /// State Pattern - Context
    /// </summary>
    public class ActionStateMachine
    {
        private BaseActionState _currentBaseActionState;

        private Dictionary<Type, BaseActionState> _states;

        // 토이프로젝트 - Debug를 커스텀하고 필드 값에 따라 자동으로 디버깅 여부 체크하도록 구현하기
        public bool IsDebug;

        private PlayerContext _playerContext;

        public void Initialize(PlayerContext playerContext, bool isStateMachineDebug)
        {
            _playerContext = playerContext;
            IsDebug = isStateMachineDebug;
            _states = new Dictionary<Type, BaseActionState>();

            _states.Add(typeof(IdleState), new IdleState(playerContext));
            _states.Add(typeof(JumpState), new JumpState(playerContext));
            _states.Add(typeof(RollState), new RollState(playerContext));
            _states.Add(typeof(LeftAttackState), new LeftAttackState(playerContext));
            _states.Add(typeof(RightAttackState), new RightAttackState(playerContext));
            _states.Add(typeof(StrongAttackState), new StrongAttackState(playerContext));
            _states.Add(typeof(LeftHandChangeState), new LeftHandChangeState(playerContext));
            _states.Add(typeof(RightHandChangeState), new RightHandChangeState(playerContext));
            _states.Add(typeof(InteractionState), new InteractionState(playerContext));

            ChangeState(typeof(IdleState));
        }

        public void UpdateState()
        {
            _currentBaseActionState?.Update(this);
        }

        public void LateUpdateState()
        {
            _currentBaseActionState?.LateUpdate(this);
        }

        public void ChangeState(Type type, bool isUpdate = false)
        {
            if (IsDebug)
                Debug.Log($"{_currentBaseActionState?.GetType()} -> {type}");

            if (type == _currentBaseActionState?.GetType())
                return;

            _currentBaseActionState?.OnExitState(this);

            _currentBaseActionState = GetState(type);

            _currentBaseActionState?.OnEnterState(this);

            if (isUpdate)
                _currentBaseActionState?.Update(this, true);
        }

        public BaseActionState GetState(Type type)
        {
            return _states.GetValueOrDefault(type);
        }

        public Type GetCurrentStateType()
        {
            return _currentBaseActionState.GetType();
        }

        public bool IsTypeEqualToCurrentState(Type type)
        {
            return _currentBaseActionState.GetType() == type;
        }

        public void ChangeStateByInputOrIdle()
        {
            var inputStateHandler = _playerContext.PlayerInputHandler;
            if (ChangeStateEnableByInput(inputStateHandler))
            {
                var inputBufferData = inputStateHandler.DeQueue();
                ChangeState(inputBufferData.Type, true);
            }
            else
            {
                ChangeState(typeof(IdleState), true);
            }
        }

        public bool ChangeStateEnableByInput(InputStateHandler inputStateHandler)
        {
            if (!inputStateHandler.TryPeek(out var bufferData))
            {
                return false;
            }

            return ChangeStateEnable(bufferData.Type);
        }

        public bool ChangeStateEnable(Type type)
        {
            var state = GetState(type);

            if (state.StateChangeEnable(this))
            {
                return true;
            }

            return false;
        }

        public bool TryChangeStateByInput()
        {
            var inputStateHandler = _playerContext.PlayerInputHandler;

            if (!ChangeStateEnableByInput(inputStateHandler))
            {
                return false;
            }

            var bufferData = inputStateHandler.DeQueue();

            ChangeState(bufferData.Type, true);

            return true;
        }
    }
}