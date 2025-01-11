using BehaviourTreeGraph.Runtime.Node.Action.Monster.Base;
using Monster;
using UnityEngine;

namespace BehaviourTreeGraph.Runtime.Node.Monster.Action.Move
{
    public class MoveNode : MonsterActionNode
    {
        public TargetMode targetMode;

        public bool isRun;

        public Transform target;
        
        // public bool customMoveSpeed;
        // [ConditionalHideInInspector("customMoveSpeed")]
        // public float moveSpeed;
        
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
            MonsterBlackBoard.MonsterContext.MonsterController.StopInPlace();
        }

        protected override NodeState OnUpdate()
        {
            if (MonsterBlackBoard.monsterManager.IsIn(targetMode) && MonsterBlackBoard.MonsterContext.MonsterController.IsStopped())
            {
                return NodeState.Success;
            }
            else if (MonsterBlackBoard.MonsterContext.CharacterAnimationManager.isBusy)
            {
                return NodeState.Failure;
            }
            else
            {
                MonsterBlackBoard.monsterManager.MoveTo(targetMode, isRun);
                return NodeState.Running;
            }
        }
    }
}