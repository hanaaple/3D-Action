using System;

namespace BehaviourTreeGraph.Runtime.Attributes
{
    public class NodeInfoAttribute : Attribute
    {
        public string actionType { get; }

        public NodeInfoAttribute(string actionType)
        {
            this.actionType = actionType;
        }
    }
}