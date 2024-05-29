using System;
using UnityEngine;

namespace CharacterControl.State
{
    // State Pattern - Context
    public class ActionStateMachine
    {
        private BaseActionState _currentBaseActionState;

        private IdleState _idleState;
        private JumpState _jumpState;
        private RollState _rollState;
        private AttackState _attackState;
        private StrongAttackState _strongAttackState;
        private LeftHandChangeState _leftHandChangeState;
        private RightHandChangeState _rightHandChangeState;

        // 토이프로젝트 - Debug를 커스텀하고 필드 값에 따라 자동으로 디버깅 여부 체크하도록 구현하기
        public bool IsDebug;

        public void Initialize(ThirdPlayerController controller, bool isStateMachineDebug)
        {
            _idleState = new IdleState(controller);
            _jumpState = new JumpState(controller);
            _rollState = new RollState(controller);
            _attackState = new AttackState(controller);
            _strongAttackState = new StrongAttackState(controller);
            _leftHandChangeState = new LeftHandChangeState(controller);
            _rightHandChangeState = new RightHandChangeState(controller);

            _currentBaseActionState = _idleState;
            _currentBaseActionState.OnEnterState(this);

            IsDebug = isStateMachineDebug;
        }

        public void UpdateState()
        {
            _currentBaseActionState?.Update(this);
        }

        public void LateUpdateState()
        {
            _currentBaseActionState?.LateUpdate(this);
        }

        public void ChangeState(Type type)
        {
            if (IsDebug)
                Debug.Log($"{_currentBaseActionState.GetType()} -> {type}");

            if (_currentBaseActionState == null || type == _currentBaseActionState.GetType())
                return;

            _currentBaseActionState?.OnExitState(this);

            _currentBaseActionState = GetState(type);

            _currentBaseActionState?.OnEnterState(this);

            _currentBaseActionState?.Update(this, true);
        }

        public BaseActionState GetState(Type type)
        {
            BaseActionState actionState = null;
            if (type == typeof(IdleState))
            {
                actionState = _idleState;
            }
            else if (type == typeof(JumpState))
            {
                actionState = _jumpState;
            }
            else if (type == typeof(RollState))
            {
                actionState = _rollState;
            }
            else if (type == typeof(AttackState))
            {
                actionState = _attackState;
            }
            else if (type == typeof(StrongAttackState))
            {
                actionState = _strongAttackState;
            }
            else if (type == typeof(RightHandChangeState))
            {
                actionState = _rightHandChangeState;
            }
            else if (type == typeof(LeftHandChangeState))
            {
                actionState = _leftHandChangeState;
            }

            return actionState;
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