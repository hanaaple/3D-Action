using UnityEngine;

namespace CharacterControl.Behaviour
{
    /// <summary>
    /// Roll Animation 종료 Behaviour
    /// </summary>
    public class RollBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponent<Player>().StateMachine.ChangeStateByInputOrIdle();
        }
    }
}
