namespace BehaviourTreeGraph.Runtime.Node.Decorator
{
    public class NotNode : DecoratorNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdate()
        {
            var childState = child.Update();
            if (childState == NodeState.Success)
            {
                return NodeState.Failure;
            }
            
            if (childState == NodeState.Failure)
            {
                return NodeState.Success;
            }

            return childState;
        }
    }
}