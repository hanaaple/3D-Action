using UnityEngine;

namespace Player.Behaviour
{
    /// <summary>
    /// Animation 종료 시, State를 변경하는 Behaviour
    /// </summary>
    public class ExitChangeStateBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponentInParent<ActionPlayer>().StateMachine.ChangeStateByInputOrIdle();
        }
    }
}