using UnityEngine;

namespace CharacterControl.Behaviour
{
    public class WeaponChangeBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<Player>().StateMachine.ChangeStateByInputOrIdle();
        }
    }
}
