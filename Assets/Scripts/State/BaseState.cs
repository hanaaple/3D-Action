using System;

namespace State
{
    public abstract class BaseState
    {
        public readonly Enum StateType;

        protected BaseState(Enum state)
        {
            StateType = state;
        }
        
        public abstract void OnEnterState(IStateMachine stateMachine);
        public abstract void OnExitState(IStateMachine stateMachine);
        public abstract void Update(IStateMachine stateMachine, bool isOnChange = false);
        public abstract void LateUpdate(IStateMachine stateMachine);
        public abstract void FixedUpdate(IStateMachine stateMachine, bool isOnChange = false);
        public abstract bool StateChangeEnable(IStateMachine stateMachine);
    }
}