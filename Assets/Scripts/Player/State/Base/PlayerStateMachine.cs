using System;
using System.Collections.Generic;
using CharacterControl.State;
using State;
using UnityEngine;

namespace Player.State.Base
{
    public enum PlayerStateMode
    {
        Idle,
        Jump,
        Roll,
        LeftAttack,
        RightAttack,
        StrongRightAttack,
        LeftHandChange,
        RightHandChange,
        Interaction
    }
    
    /// <summary>
    /// State Pattern - Context
    /// </summary>
    public class PlayerStateMachine : BaseStateMachine<BasePlayerActionState>
    {
        private PlayerContext _playerContext;

        public void Initialize(PlayerContext playerContext, bool isStateMachineDebug)
        {
            _playerContext = playerContext;
            IsDebug = isStateMachineDebug;

            States = new Dictionary<Enum, BasePlayerActionState>();

            States.Add(PlayerStateMode.Idle, new IdleState(playerContext, PlayerStateMode.Idle));
            States.Add(PlayerStateMode.Jump, new JumpState(playerContext, PlayerStateMode.Jump));
            States.Add(PlayerStateMode.Roll, new RollState(playerContext, PlayerStateMode.Roll));
            States.Add(PlayerStateMode.LeftAttack, new LeftAttackState(playerContext, PlayerStateMode.LeftAttack));
            States.Add(PlayerStateMode.RightAttack, new RightAttackState(playerContext, PlayerStateMode.RightAttack));
            States.Add(PlayerStateMode.StrongRightAttack, new StrongAttackState(playerContext, PlayerStateMode.StrongRightAttack));
            States.Add(PlayerStateMode.LeftHandChange, new LeftHandChangeState(playerContext, PlayerStateMode.LeftHandChange));
            States.Add(PlayerStateMode.RightHandChange, new RightHandChangeState(playerContext, PlayerStateMode.RightHandChange));
            States.Add(PlayerStateMode.Interaction, new InteractionState(playerContext, PlayerStateMode.Interaction));

            ChangeState(PlayerStateMode.Idle);
        }

        public void ChangeStateByInputOrIdle()
        {
            var inputStateHandler = _playerContext.PlayerInputHandler;
            if (ChangeStateEnableByInput(inputStateHandler))
            {
                var inputBufferData = inputStateHandler.DeQueue();
                ChangeState(inputBufferData.Type, true);
            }
            else
            {
                ChangeState(PlayerStateMode.Idle, true);
            }
        }

        private bool ChangeStateEnableByInput(InputStateHandler inputStateHandler)
        {
            if (!inputStateHandler.TryPeek(out var bufferData))
            {
                return false;
            }

            Debug.Log($"State Change Enable {bufferData.Type} -> {GetState(bufferData.Type).StateChangeEnable(this)}");
            return ChangeStateEnable(bufferData.Type);
        }

        public bool TryChangeStateByInput()
        {
            var inputStateHandler = _playerContext.PlayerInputHandler;

            if (!ChangeStateEnableByInput(inputStateHandler))
            {
                return false;
            }

            var bufferData = inputStateHandler.DeQueue();

            ChangeState(bufferData.Type, true);

            return true;
        }
    }
}