using BehaviourTreeGraph.Runtime.Node.Action.Monster.Base;
using Monster;
using UnityEngine;

namespace BehaviourTreeGraph.Runtime.Node.Monster.Action.Move
{
    public class TurnNode : MonsterActionNode
    {
        public TargetMode targetMode;
        public Transform target;
        
        protected override void OnStart()
        {
            // Debug.Log($"Turning Start {targetMode}");
        }

        protected override void OnStop()
        {
            // Debug.Log($"Turning Stop {targetMode}");
        }

        protected override NodeState OnUpdate()
        {
            target = MonsterBlackBoard.monsterManager.GetTarget(targetMode);
            if (MonsterBlackBoard.MonsterContext.MonsterController.TryTurn(target.position))
            {
                return NodeState.Running;
            }
            else
            {
                return NodeState.Success;
            }
        }
    }
}