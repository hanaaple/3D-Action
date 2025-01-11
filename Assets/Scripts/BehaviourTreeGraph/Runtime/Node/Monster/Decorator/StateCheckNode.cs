using Monster;
using UnityEngine;
using UnityEngine.Serialization;

namespace BehaviourTreeGraph.Runtime.Node.Monster.Decorator
{
    public class StateCheckNode : MonsterDecoratorNode
    {
        [FormerlySerializedAs("monsterState")] [FormerlySerializedAs("stateMode")] [SerializeField] private MonsterStateMode monsterStateMode;
        [SerializeField] private bool isNot;
    
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdate()
        {
            if (!isNot && monsterStateMode == MonsterBlackBoard.monsterManager.GetState())
            {
                return child.Update();
            }
        
            if (isNot && monsterStateMode != MonsterBlackBoard.monsterManager.GetState())
            {
                return child.Update();
            }

            return NodeState.Failure;
        }
    }
}
