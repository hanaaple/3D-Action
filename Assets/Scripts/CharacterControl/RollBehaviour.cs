using UnityEngine;

namespace CharacterControl
{
    public class RollBehaviour : StateMachineBehaviour
    {
        private static readonly int AnimIdRoll = Animator.StringToHash("Roll");
    
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(AnimIdRoll, false);
        }
    }
}
