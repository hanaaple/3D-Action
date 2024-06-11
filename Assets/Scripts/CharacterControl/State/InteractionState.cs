using CharacterControl.State.Base;

namespace CharacterControl.State
{
    public class InteractionState : BaseActionState
    {
        public InteractionState(PlayerContext playerContext) : base(playerContext)
        {
        }

        public override void OnEnterState(ActionStateMachine stateMachine)
        {
            // 각 애니메이션은 IInteractable의 Interaction()에 의해 작동된다.
            // 종료 후 ChangeState 또한 IInteractable에 의해 작동한다.
            PlayerContext.PlayerInteraction.Interaction();
        }

        public override void OnExitState(ActionStateMachine stateMachine)
        {
        }

        public override void Update(ActionStateMachine stateMachine, bool isOnChange = false)
        {
        }

        public override void LateUpdate(ActionStateMachine stateMachine)
        {
        }

        public override bool StateChangeEnable(ActionStateMachine stateMachine)
        {
            if (stateMachine.GetCurrentStateType() == typeof(IdleState) &&
                PlayerContext.PlayerInteraction.IsInteractionExist() && PlayerContext.Controller.IsGrounded)
            {
                return true;
            }

            return false;
        }
    }
}