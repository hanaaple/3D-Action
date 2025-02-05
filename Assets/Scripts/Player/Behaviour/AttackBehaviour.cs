using UnityEngine;

namespace Player.Behaviour
{
    /// <summary>
    /// Attack Animation 종료 Behaviour
    /// </summary>
    public class AttackBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo sourceStateInfo, int layerIndex)
        {
            var targetStateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

            if (targetStateInfo.IsName("Idle"))
            {
                animator.GetComponentInParent<ActionPlayer>().StateMachine.ChangeStateByInputOrIdle();
            }
        }
    }
}
