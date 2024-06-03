using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterControl.State
{
    // State Pattern - Context
    public class ActionStateMachine
    {
        private BaseActionState _currentBaseActionState;


        private Dictionary<Type, BaseActionState> _states;
        
        // 토이프로젝트 - Debug를 커스텀하고 필드 값에 따라 자동으로 디버깅 여부 체크하도록 구현하기
        public bool IsDebug;

        public void Initialize(ThirdPlayerController controller, bool isStateMachineDebug)
        {
            IsDebug = isStateMachineDebug;
            _states = new Dictionary<Type, BaseActionState>();
            
            _states.Add(typeof(IdleState), new IdleState(controller));
            _states.Add(typeof(JumpState), new JumpState(controller));
            _states.Add(typeof(RollState), new RollState(controller));
            _states.Add(typeof(LeftAttackState), new LeftAttackState(controller));
            _states.Add(typeof(RightAttackState), new RightAttackState(controller));
            _states.Add(typeof(StrongAttackState), new StrongAttackState(controller));
            _states.Add(typeof(LeftHandChangeState), new LeftHandChangeState(controller));
            _states.Add(typeof(RightHandChangeState), new RightHandChangeState(controller));
            
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

            if(isUpdate)
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
            _currentBaseActionState.Controller.ChangeStateByInputOrIdle(this);
        }
    }
}