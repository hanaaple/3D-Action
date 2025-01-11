using BehaviourTreeGraph.Runtime.Attributes;
using UnityEngine;

namespace BehaviourTreeGraph.Runtime.Node.Decorator
{
    [NodeInfo("Base")]
    public abstract class DecoratorNode : BehaviourTreeGraphNode
    {
        [HideInInspector] public BehaviourTreeGraphNode child;
        
        public override BehaviourTreeGraphNode Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.Initialize();
            node.child = child.Clone();
            return node;
        }
    }
}