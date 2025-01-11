using BehaviourTreeGraph.Runtime.Attributes;

namespace BehaviourTreeGraph.Runtime.Node.Action.Monster.Base
{
    [NodeInfo("Monster")]
    public abstract class MonsterActionNode : ActionNode 
    {
        protected MonsterBlackBoard MonsterBlackBoard;
        
        public override void InitializeBlackBoard()
        {
            MonsterBlackBoard = blackboard as MonsterBlackBoard;
        }
    }
}