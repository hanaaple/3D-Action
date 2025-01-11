using BehaviourTreeGraph.Runtime.Attributes;
using BehaviourTreeGraph.Runtime.Node.Decorator;

namespace BehaviourTreeGraph.Runtime.Node.Monster.Decorator
{
    [NodeInfo("Monster")]
    public abstract class MonsterDecoratorNode : DecoratorNode
    {
        protected MonsterBlackBoard MonsterBlackBoard;

        public override void InitializeBlackBoard()
        {
            MonsterBlackBoard = blackboard as MonsterBlackBoard;
        }
    }
}