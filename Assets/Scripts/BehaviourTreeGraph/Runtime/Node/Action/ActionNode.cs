using BehaviourTreeGraph.Runtime.Attributes;

namespace BehaviourTreeGraph.Runtime.Node.Action
{
    [NodeInfo("Base")]
    public abstract class ActionNode : BehaviourTreeGraphNode
    {
        public override BehaviourTreeGraphNode Clone()
        {
            ActionNode node = Instantiate(this);
            node.Initialize();
            return node;
        }
    }
}