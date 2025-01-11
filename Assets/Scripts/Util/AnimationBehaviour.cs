using Character;
using UnityEngine;

public class AnimationBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var animationManager = animator.GetComponent<CharacterAnimationManager>();
        animationManager.OnAnimationEnd?.Invoke();
        animationManager.OnAnimationEnd = null;
    }
}
