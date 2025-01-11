using System;
using Monster.State.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monster.State
{
    public class MonsterAttackState : MonsterActionState
    {
        private float _timer;
        
        public MonsterAttackState(MonsterContext playerContext, Enum state) : base(playerContext, state)
        {
        }
    
        protected override void OnEnterState(MonsterStateMachine stateMachine)
        {
            _timer = 0;
        }

        protected override void OnExitState(MonsterStateMachine stateMachine)
        {
        }

        protected override void Update(MonsterStateMachine stateMachine, bool isOnChange = false)
        {
            if(stateMachine.IsDebug)
                Debug.Log($"{_timer},   {MonsterContext.CharacterAnimationManager.isBusy}");
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                if (MonsterContext.CharacterAnimationManager.isBusy)
                    return;

                var attackType = Random.Range(0, 2);
                if(stateMachine.IsDebug)
                    Debug.Log($"Attack! {attackType}");

                _timer = MonsterContext.MonsterActionData.AttackCoolTime;

                MonsterContext.MonsterManager.SetAttackEnable(attackType, true);

                MonsterContext.CharacterAnimationManager.isBusy = true;
                MonsterContext.CharacterAnimationManager.SetInteger("AttackType", attackType);
                MonsterContext.CharacterAnimationManager.SetTrigger("Attack");

                MonsterContext.CharacterAnimationManager.OnAnimationEnd = () =>
                {
                    MonsterContext.MonsterManager.SetAttackEnable(attackType, false);
                    MonsterContext.CharacterAnimationManager.isBusy = false;
                };
            }
        }

        protected override void LateUpdate(MonsterStateMachine stateMachine)
        {
        }

        protected override void FixedUpdate(MonsterStateMachine stateMachine, bool isOnChange = false)
        {
        }
        
        protected override bool StateChangeEnable(MonsterStateMachine stateMachine)
        {
            return true;
        }
    }
}