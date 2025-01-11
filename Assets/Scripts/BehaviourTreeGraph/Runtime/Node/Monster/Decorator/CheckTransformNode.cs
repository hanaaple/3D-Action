using Monster;

namespace BehaviourTreeGraph.Runtime.Node.Monster.Decorator
{
    public class CheckTransformNode : MonsterDecoratorNode
    {
        public TargetMode targetMode;

        public bool isNegative;
        
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdate()
        {
            var isIn = MonsterBlackBoard.monsterManager.IsIn(targetMode);
            if (isNegative && !isIn)
            {
                return child.Update();
            }
            else if (!isNegative && isIn)
            {
                return child.Update();
            }

            return NodeState.Failure;
        }
    }
}