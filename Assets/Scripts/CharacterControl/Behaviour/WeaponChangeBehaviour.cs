using UnityEngine;

namespace CharacterControl.Behaviour
{
    /// <summary>
    /// WeaponChange Animation 종료 Behaviour
    /// </summary>
    public class WeaponChangeBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<Player>().StateMachine.ChangeStateByInputOrIdle();
        }
    }
}
