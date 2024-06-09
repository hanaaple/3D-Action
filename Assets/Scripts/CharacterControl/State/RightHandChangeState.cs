using UnityEngine;

namespace CharacterControl.State
{
    public class RightHandChangeState : BaseActionState
    {
        private readonly int _animIdRightHand = Animator.StringToHash("RightWeaponChange");

        public RightHandChangeState(PlayerContext playerContext) : base(playerContext)
        {
        }

        public override void OnEnterState(ActionStateMachine stateMachine)
        {
            // WeaponChangeBehaviour.OnStateExit()에 의해 종료
            PlayerContext.Controller.Animator.SetTrigger(_animIdRightHand);
        }

        public override void OnExitState(ActionStateMachine stateMachine)
        {
        }

        public override void Update(ActionStateMachine stateMachine, bool isOnChange = false)
        {
            PlayerContext.Controller.UpdateSpeed();
            PlayerContext.Controller.Rotate();
            PlayerContext.Controller.Translate();
        }

        public override void LateUpdate(ActionStateMachine stateMachine)
        {
        }

        public override bool StateChangeEnable(ActionStateMachine stateMachine)
        {
            if (PlayerContext.Controller.IsGrounded && stateMachine.IsTypeEqualToCurrentState(typeof(IdleState)))
            {
                return true;
            }

            return false;
        }
    }
}