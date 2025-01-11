namespace BehaviourTreeGraph.Runtime.Node.Composite
{
    /// <summary>
    /// And Node
    /// </summary>
    public class SequenceNode : CompositeNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdate()
        {
            if (children == null || children.Count == 0)
                return NodeState.Failure;

            for (var index = 0; index < children.Count; index++)
            {
                var child = children[index];
                switch (child.Update())
                {
                    case NodeState.Running:
                        InitializeSiblingNodeState(index + 1);
                        return NodeState.Running;
                    case NodeState.Failure:
                        InitializeSiblingNodeState(index + 1);
                        return NodeState.Failure;
                }
            }

            return NodeState.Success;
        }
    }
}