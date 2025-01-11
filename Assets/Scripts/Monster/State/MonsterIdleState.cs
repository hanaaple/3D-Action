using System;
using Monster.State.Base;

namespace Monster.State
{
    public class MonsterIdleState : MonsterActionState
    {
        public MonsterIdleState(MonsterContext playerContext, Enum state) : base(playerContext, state)
        {
        }
        
        protected override void OnEnterState(MonsterStateMachine stateMachine)
        {
        }

        protected override void OnExitState(MonsterStateMachine stateMachine)
        {
        }

        protected override void Update(MonsterStateMachine stateMachine, bool isOnChange = false)
        {
            
        }

        protected override void LateUpdate(MonsterStateMachine stateMachine)
        {
        }
        
        protected override void FixedUpdate(MonsterStateMachine stateMachine, bool isOnChange = false)
        {
        }

        protected override bool StateChangeEnable(MonsterStateMachine stateMachine)
        {
            return true;
        }
    }
}