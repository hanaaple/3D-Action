using UnityEngine;

namespace CharacterControl.Behaviour
{
    public class ShoulderWeaponBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<Player>().StateMachine.ChangeStateByInputOrIdle();
        }
    }
}
