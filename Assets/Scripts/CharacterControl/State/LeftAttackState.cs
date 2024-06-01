namespace CharacterControl.State
{
    public class LeftAttackState : BaseActionState
    {
        public LeftAttackState(ThirdPlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState(ActionStateMachine stateMachine)
        {
            // 장착된 무기에 따라 공격
        }

        public override void OnExitState(ActionStateMachine stateMachine)
        {
        }

        public override void Update(ActionStateMachine stateMachine, bool isOnChange = false)
        {
            // 공격 도중 추가 공격한 경우
            // -> 콤보
            
            // 그렇다면 콤보를 어떻게 구현할 것인가 -> 
            
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