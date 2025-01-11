using Character;
using Monster;
using UnityEngine;

public class TurnBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var animationManager = animator.GetComponent<CharacterAnimationManager>();
        animationManager.isBusy = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var controller = animator.GetComponent<MonsterController>();
        if (stateInfo.normalizedTime > 0.8f)
        {
            // Debug.Log($"Set Rotation, {controller.DesiredRotation.eulerAngles}");
            animator.rootRotation = Quaternion.Lerp(animator.rootRotation, controller.DesiredRotation, Time.deltaTime * 10);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var controller = animator.GetComponent<MonsterController>();
        var animationManager = animator.GetComponent<CharacterAnimationManager>();
        // Debug.Log($"{controller.DesiredRotation.eulerAngles}, {animator.rootRotation.eulerAngles}");
        // Debug.LogError($"{stateInfo.normalizedTime},   {stateInfo.normalizedTime * stateInfo.length}");
        
        animator.rootRotation = controller.DesiredRotation;
        animationManager.isBusy = false;
        animator.applyRootMotion = false;
    }
}
