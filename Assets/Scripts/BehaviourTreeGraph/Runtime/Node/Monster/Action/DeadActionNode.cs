using BehaviourTreeGraph.Runtime.Node.Action.Monster.Base;
using Monster;

namespace BehaviourTreeGraph.Runtime.Node.Monster.Action.Dead
{
    public class DeadActionNode : MonsterActionNode
    {
        protected override void OnStart()
        {
            if (MonsterBlackBoard.monsterManager.GetState() == MonsterStateMode.Dead)
            {
                MonsterBlackBoard.MonsterContext.CharacterAnimationManager.SetBool("Dead", true);
            }
        }

        protected override void OnStop()
        {
            MonsterBlackBoard.MonsterContext.CharacterAnimationManager.SetBool("Dead", false);
        }

        protected override NodeState OnUpdate()
        {
            if (MonsterBlackBoard.monsterManager.GetState() != MonsterStateMode.Dead)
            {
                return NodeState.Failure;
            }

            return NodeState.Running;
        }
    }
}