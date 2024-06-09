using CharacterControl;
using CharacterControl.State;
using TMPro;
using UnityEngine;
using Util;

namespace UI.Entity
{
    public class DebugInfo : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private ThirdPlayerController controller;

        [SerializeField] private TMP_Text stateText;
        [SerializeField] private TMP_Text moveText;
        [SerializeField] private TMP_Text attackText;

        [SerializeField] private AnimationEventHandler animationEventHandler;

        private bool _isRotateEnable;
        private bool _isComboEnable;

        private void Start()
        {
            animationEventHandler.OnRotationEnableChanged += (isRotateEnable) => { _isRotateEnable = isRotateEnable; };
            animationEventHandler.OnComboEnableChanged += (isComboEnable) => { _isComboEnable = isComboEnable; };
        }


        void Update()
        {
            var type = player.StateMachine.GetCurrentStateType();
            stateText.text = $"State: {type.Name}\n";

            moveText.text = $"Ground: {controller.IsGrounded}";
            attackText.text = type == typeof(RightAttackState) || type == typeof(LeftAttackState)
                ? $"Combo: {_isComboEnable}\n" + $"Rotate: {_isRotateEnable}\n" 
                : "";
        }
    }
}
