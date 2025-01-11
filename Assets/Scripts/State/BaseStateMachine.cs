using System;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public abstract class BaseStateMachine<T> : IStateMachine where T : BaseState
    {
        // 토이프로젝트 - Debug를 커스텀하고 필드 값에 따라 자동으로 디버깅 여부 체크하도록 구현하기
        public bool IsDebug;
    
        private T _currentState;
        protected Dictionary<Enum, T> States;

        public void UpdateState()
        {
            _currentState?.Update(this);
        }

        public void LateUpdateState()
        {
            _currentState?.LateUpdate(this);
        }
        
        public void FixedUpdateState()
        {
            _currentState?.FixedUpdate(this);
        }

        public void ChangeState(Enum type, bool isUpdate = false)
        {
            if (IsDebug)
                Debug.Log($"{_currentState?.StateType} -> {type}");
        
            if (type.Equals(_currentState?.StateType))
                return;
            
            _currentState?.OnExitState(this);

            _currentState = GetState(type);

            _currentState?.OnEnterState(this);

            if (isUpdate)
                _currentState?.Update(this, true);
        }

        public T GetState(Enum type)
        {
            return States.GetValueOrDefault(type);
        }

        public Enum GetCurrentState()
        {
            return _currentState.StateType;
        }

        public bool CurrentStateEquals(Enum type)
        {
            return _currentState.StateType.Equals(type);
        }

        public bool ChangeStateEnable(Enum type)
        {
            var state = GetState(type);
            
            if (state.StateChangeEnable(this))
            {
                return true;
            }

            return false;
        }
    }
}