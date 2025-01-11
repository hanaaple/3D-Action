using System;
using State;

namespace Player.State.Base
{
    public abstract class BasePlayerActionState : BaseState
    {
        protected readonly PlayerContext PlayerContext;

        protected BasePlayerActionState(PlayerContext playerContext, Enum state) : base(state)
        {
            PlayerContext = playerContext;
        }

        public override void OnEnterState(IStateMachine stateMachine)
        {
            var actionStateMachine = stateMachine as PlayerStateMachine;
            OnEnterState(actionStateMachine);
        }
        
        public override void OnExitState(IStateMachine stateMachine)
        {
            var actionStateMachine = stateMachine as PlayerStateMachine;
            OnExitState(actionStateMachine);
        }
        
        public override void Update(IStateMachine stateMachine, bool isOnChange = false)
        {
            var actionStateMachine = stateMachine as PlayerStateMachine;
            Update(actionStateMachine, isOnChange);
        }
        
        public override void LateUpdate(IStateMachine stateMachine)
        {
            var actionStateMachine = stateMachine as PlayerStateMachine;
            LateUpdate(actionStateMachine);
        }
        
        public override void FixedUpdate(IStateMachine stateMachine, bool isOnChange = false)
        {
            var actionStateMachine = stateMachine as PlayerStateMachine;
            FixedUpdate(actionStateMachine, isOnChange);
        }
        
        public override bool StateChangeEnable(IStateMachine stateMachine)
        {
            var actionStateMachine = stateMachine as PlayerStateMachine;
            return StateChangeEnable(actionStateMachine);
        }

        // this execution order is slower than Controller
        protected abstract void OnEnterState(PlayerStateMachine stateMachine);
        protected abstract void OnExitState(PlayerStateMachine stateMachine);
        protected abstract void Update(PlayerStateMachine stateMachine, bool isOnChange = false);
        protected abstract void LateUpdate(PlayerStateMachine stateMachine);
        protected abstract void FixedUpdate(PlayerStateMachine stateMachine, bool isOnChange = false);
        protected abstract bool StateChangeEnable(PlayerStateMachine stateMachine);
    }
}