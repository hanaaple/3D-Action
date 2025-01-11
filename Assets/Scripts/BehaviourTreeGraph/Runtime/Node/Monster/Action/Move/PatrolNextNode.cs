using BehaviourTreeGraph.Runtime.Node.Action.Monster.Base;

namespace BehaviourTreeGraph.Runtime.Node.Monster.Action.Move
{
    public class PatrolNextNode : MonsterActionNode
    {
        protected override void OnStart()
        {
            MonsterBlackBoard.monsterManager.ChangePatrolTarget();
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdate()
        {
            return NodeState.Success;
        }
    }
}