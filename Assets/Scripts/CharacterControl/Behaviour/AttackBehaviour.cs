using UnityEngine;

namespace CharacterControl.Behaviour
{
    public class AttackBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo sourceStateInfo, int layerIndex)
        {
            var targetStateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

            if (targetStateInfo.IsName("Idle"))
            {
                animator.GetComponent<Player>().StateMachine.ChangeStateByInputOrIdle();
            }
        }
    }
}
