namespace BehaviourTreeGraph.Runtime.Node.Monster.Decorator
{
    public class AggroCheckNode : MonsterDecoratorNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdate()
        {
            if (MonsterBlackBoard.monsterManager.GetAggroTarget().aggroTarget == null)
                return NodeState.Failure;

            return child.Update();
        }
    }
}
