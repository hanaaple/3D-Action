namespace CharacterControl.State
{
    public class AttackState : BaseActionState
    {
        public AttackState(ThirdPlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState(ActionStateMachine stateMachine)
        {
        }

        public override void OnExitState(ActionStateMachine stateMachine)
        {
        }

        public override void Update(ActionStateMachine stateMachine, bool isOnChange = false)
        {
            // 움직임 불가능
        }

        public override void LateUpdate(ActionStateMachine stateMachine)
        {
        }

        // 공격 중에 다른 State 불가능하도록 추가 구현
        public override bool StateChangeEnable(ActionStateMachine stateMachine)
        {
            // if (stateMachine.IsTypeEqualToCurrentState(typeof(IdleState)) ||
            //     stateMachine.IsTypeEqualToCurrentState(typeof(AttackState)) ||
            //     stateMachine.IsTypeEqualToCurrentState(typeof(StrongAttackState)))
            // {
            //     return true;
            // }
            
            return false;
        }
    }
}