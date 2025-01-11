using BehaviourTreeGraph.Runtime.Node.Action.Monster.Base;
using Monster;

namespace BehaviourTreeGraph.Runtime.Node.Monster.Action
{
    public class SetStateNode : MonsterActionNode
    {
        public MonsterStateMode monsterState;
        
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdate()
        {
            if (MonsterBlackBoard.monsterManager.StateChangeEnable(monsterState))
            {
                MonsterBlackBoard.monsterManager.ChangeState(monsterState);
                return NodeState.Success;
            }
            else
            {
                return NodeState.Running;
            }
        }
    }
}
