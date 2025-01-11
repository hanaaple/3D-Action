using System;
using Player;
using Player.State.Base;
using TMPro;
using UnityEngine;
using Util;

namespace UI.Entity
{
    public class DebugInfoView : MonoBehaviour
    {
        private ActionPlayer _actionPlayer;
        private PlayerController _controller;

        [SerializeField] private TMP_Text stateText;
        [SerializeField] private TMP_Text moveText;
        [SerializeField] private TMP_Text attackText;

        private bool _isRotateEnable;
        private bool _isComboEnable;

        private void Start()
        {
            _actionPlayer = FindObjectOfType<ActionPlayer>();
            _controller = FindObjectOfType<PlayerController>();
            var animationEventHandler = FindObjectOfType<AnimationEventHandler>();
            
            animationEventHandler.OnRotationEnableChanged += (isRotateEnable) => { _isRotateEnable = isRotateEnable; };
            animationEventHandler.OnComboEnableChanged += (isComboEnable) => { _isComboEnable = isComboEnable; };
        }


        private void Update()
        {
            var type = _actionPlayer.StateMachine.GetCurrentState();
            stateText.text = $"State: {type.ToString()}\n";

            moveText.text = $"Ground: {_controller.IsGrounded}";
            attackText.text = type.Equals(PlayerStateMode.RightAttack) || type.Equals(PlayerStateMode.LeftAttack)
                ? $"Combo: {_isComboEnable}\n" + $"Rotate: {_isRotateEnable}\n" 
                : "";
        }
    }
}
