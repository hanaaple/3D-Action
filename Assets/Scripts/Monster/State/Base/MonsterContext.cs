using Character;

namespace Monster.State.Base
{
    public class MonsterContext
    {
        public MonsterController MonsterController;
        public CharacterAnimationManager CharacterAnimationManager;
        public MonsterActionData MonsterActionData;
        public MonsterManager MonsterManager;
    }
    
    public class MonsterActionData
    {
        public float AttackCoolTime;
    }
}