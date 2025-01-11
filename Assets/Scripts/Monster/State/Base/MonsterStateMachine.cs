using System;
using System.Collections.Generic;
using State;
using Util;

namespace Monster.State.Base
{
    public class MonsterStateMachine : BaseStateMachine<MonsterActionState>
    {
        private MonsterContext _monsterContext;
        
        public void Initialize(MonsterContext monsterContext)
        {
            _monsterContext = monsterContext;
            
            States = new Dictionary<Enum, MonsterActionState>();
            
            // Fall, Jump, RightAttack, LeftAttack, Roll
            
            States.Add(MonsterStateMode.Idle, new MonsterIdleState(monsterContext, MonsterStateMode.Idle));
            States.Add(MonsterStateMode.Alert, new MonsterIdleState(monsterContext, MonsterStateMode.Alert));
            States.Add(MonsterStateMode.Attack, new MonsterAttackState(monsterContext, MonsterStateMode.Attack));
            States.Add(MonsterStateMode.Chase, new MonsterIdleState(monsterContext, MonsterStateMode.Chase));
            States.Add(MonsterStateMode.Dead, new MonsterIdleState(monsterContext, MonsterStateMode.Dead));
            States.Add(MonsterStateMode.Return, new MonsterIdleState(monsterContext, MonsterStateMode.Return));
            
            ChangeState(MonsterStateMode.Idle);
        }
    }
}