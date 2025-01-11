using System;
using State;

namespace Monster.State.Base    
{
    public abstract class MonsterActionState : BaseState
    {
        protected readonly MonsterContext MonsterContext;

        protected MonsterActionState(MonsterContext playerContext, Enum state) : base(state)
        {
            MonsterContext = playerContext;
        }
        
        public override void OnEnterState(IStateMachine stateMachine)
        {
            var actionStateMachine = stateMachine as MonsterStateMachine;
            OnEnterState(actionStateMachine);
        }
        
        public override void OnExitState(IStateMachine stateMachine)
        {
            var actionStateMachine = stateMachine as MonsterStateMachine;
            OnExitState(actionStateMachine);
        }
        
        public override void Update(IStateMachine stateMachine, bool isOnChange = false)
        {
            var actionStateMachine = stateMachine as MonsterStateMachine;
            Update(actionStateMachine, isOnChange);
        }
        
        public override void LateUpdate(IStateMachine stateMachine)
        {
            var actionStateMachine = stateMachine as MonsterStateMachine;
            LateUpdate(actionStateMachine);
        }
        
        public override void FixedUpdate(IStateMachine stateMachine, bool isOnChange = false)
        {
            var actionStateMachine = stateMachine as MonsterStateMachine;
            FixedUpdate(actionStateMachine, isOnChange);
        }
        
        public override bool StateChangeEnable(IStateMachine stateMachine)
        {
            var actionStateMachine = stateMachine as MonsterStateMachine;
            return StateChangeEnable(actionStateMachine);
        }
        
        protected abstract void OnEnterState(MonsterStateMachine stateMachine);
        protected abstract void OnExitState(MonsterStateMachine stateMachine);
        protected abstract void Update(MonsterStateMachine stateMachine, bool isOnChange = false);
        protected abstract void LateUpdate(MonsterStateMachine stateMachine);
        protected abstract void FixedUpdate(MonsterStateMachine stateMachine, bool isOnChange = false);
        protected abstract bool StateChangeEnable(MonsterStateMachine stateMachine);
    }
}