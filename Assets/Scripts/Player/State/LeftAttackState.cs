using System;
using Data.Item.Data;
using Data.Play;
using Manager;
using Player.State.Base;
using UnityEngine;

namespace CharacterControl.State
{
    public class LeftAttackState : AttackState
    {
        private readonly int _animIdLeftAttack = Animator.StringToHash("LeftAttack");

        public LeftAttackState(PlayerContext playerContext, Enum state) : base(playerContext, state)
        {
        }

        protected override int GetAttackHash()
        {
            return _animIdLeftAttack;
        }

        protected override Weapon GetCurrentWeapon()
        {
            return PlaySceneManager.instance.playerDataManager.equipViewModel.GetCurrentWeapon(WeaponEquipType.Left);
        }

        protected override PlayerStateMode GetPlayerStateMode()
        {
            return PlayerStateMode.LeftAttack;
        }

        // 공격 중에 다른 State 불가능하도록 추가 구현
        protected override bool StateChangeEnable(PlayerStateMachine stateMachine)
        {
            if (
                // !PlayerContext.PlayerController.IsTired &&
                stateMachine.CurrentStateEquals(PlayerStateMode.Idle) 
                // ||
                // stateMachine.CurrentStateEquals(PlayerStateMode.LeftAttack)
                )
            {
                return true;
            }

            return false;
        }
    }
}