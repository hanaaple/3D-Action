using System;
using Data.Item.Data;
using Data.Play;
using Manager;
using Player.State.Base;
using UnityEngine;

namespace CharacterControl.State
{
    public class RightAttackState : AttackState
    {
        private readonly int _animIdRightAttack = Animator.StringToHash("RightAttack");

        public RightAttackState(PlayerContext playerContext, Enum state) : base(playerContext, state)
        {
        }

        protected override int GetAttackHash()
        {
            return _animIdRightAttack;
        }

        protected override Weapon GetCurrentWeapon()
        {
            return PlaySceneManager.instance.playerDataManager.equipViewModel.GetCurrentWeapon(WeaponEquipType.Right);
        }

        protected override PlayerStateMode GetPlayerStateMode()
        {
            return PlayerStateMode.RightAttack;
        }

        protected override bool StateChangeEnable(PlayerStateMachine stateMachine)
        {
            if (
                // !PlayerContext.PlayerController.IsTired &&
                stateMachine.CurrentStateEquals(PlayerStateMode.Idle) 
                // ||
                // stateMachine.CurrentStateEquals(PlayerStateMode.RightAttack) ||
                // stateMachine.CurrentStateEquals(PlayerStateMode.StrongRightAttack)
                )
            {
                return true;
            }

            return false;
        }
    }
}