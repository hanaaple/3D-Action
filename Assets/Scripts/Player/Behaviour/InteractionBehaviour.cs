using UnityEngine;

namespace Player.Behaviour
{
    public class InteractionBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponentInParent<PlayerInteractor>().EndInteraction();
        }
    }
}