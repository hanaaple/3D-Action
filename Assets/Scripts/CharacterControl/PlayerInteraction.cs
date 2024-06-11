using System.Collections.Generic;
using System.Linq;
using CharacterControl.State;
using Interaction;
using TMPro;
using UnityEngine;

namespace CharacterControl
{
    /// <summary>
    /// 플레이어 인터랙션 관리
    /// </summary>
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject interactionUIPanel;

        [SerializeField] private TMP_Text interactionUIText;

        [SerializeField] private Color interactableColor;
        [SerializeField] private Color unInteractableColor;

        internal Animator Animator;
        internal Player Player;

        private ThirdPlayerController _controller;
        private IInteractable _interactable;

        private readonly List<IInteractable> _closeInteractionItems = new();

        private void Start()
        {
            Player = GetComponent<Player>();
            Animator = GetComponent<Animator>();
            _controller = GetComponent<ThirdPlayerController>();
        }

        private void Update()
        {
            if (!IsInteractionExist())
                return;

            interactionUIText.color = IsInteractionEnable() ? interactableColor : unInteractableColor;
            interactionUIText.text = $"E: {GetCloseInteraction().GetUIContext()}";
        }

        private void UpdateInteractionView()
        {
            interactionUIPanel.SetActive(IsInteractionExist());
        }

        public void AddCloseItem(IInteractable interactable)
        {
            _closeInteractionItems.Add(interactable);
            UpdateInteractionView();
        }

        public void TryRemoveCloseItem(IInteractable interactable)
        {
            if (!_closeInteractionItems.Contains(interactable)) return;
            _closeInteractionItems.Remove(interactable);
            UpdateInteractionView();
        }

        public bool IsInteractionExist()
        {
            return _closeInteractionItems.Count > 0;
        }

        private bool IsInteractionEnable()
        {
            return Player.StateMachine.ChangeStateEnable(typeof(InteractionState));
        }

        public void Interaction()
        {
            var closeInteraction = GetCloseInteraction();

            TryRemoveCloseItem(closeInteraction);

            _interactable = closeInteraction;
            _interactable.Interact(this);
        }

        public void EndInteraction()
        {
            _interactable.OnInteractionEnd();
        }

        private IInteractable GetCloseInteraction()
        {
            var closeInteraction = _closeInteractionItems
                .OrderBy(item => Vector3.Distance(transform.position, item.GetPosition()))
                .First();

            return closeInteraction;
        }

        public void Teleport(Transform targetTransform)
        {
            _controller.Teleport(targetTransform);
        }
        
        public void InitMoveState()
        {
            _controller.InitMoveState();
        }

        public void AnimationEvent()
        {
            _interactable.OnAnimationEvent();
        }
    }
}