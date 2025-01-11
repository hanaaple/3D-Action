using System;
using Monster;
using Monster.State.Base;

namespace BehaviourTreeGraph.Runtime
{
    [Serializable]
    public abstract class BlackBoard
    {
    }
    
    [Serializable]
    public class MonsterBlackBoard : BlackBoard
    {
        public MonsterManager monsterManager;
        public MonsterContext MonsterContext;
    }
}
