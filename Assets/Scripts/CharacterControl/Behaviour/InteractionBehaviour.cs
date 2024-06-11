using Interaction;
using UnityEngine;

namespace CharacterControl.Behaviour
{
    public class InteractionBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<PlayerInteraction>().EndInteraction();
        }
    }
}