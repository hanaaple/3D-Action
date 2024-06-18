using CharacterControl.State;
using Interaction;
using Interaction.Base;
using UI.View;
using UnityEngine;

namespace CharacterControl
{
    /// <summary>
    /// 플레이어 인터랙션 관리
    /// </summary>
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private InteractionUIView interactionUIView;
        
        internal Animator Animator;
        internal Player Player;

        private ThirdPlayerController _controller;
        private IInteractable _playingInteractable;

        private InteractionUIViewModel _interactionUIViewModel;
        
        private void Start()
        {
            Player = GetComponent<Player>();
            Animator = GetComponent<Animator>();
            _controller = GetComponent<ThirdPlayerController>();

            _interactionUIViewModel = new InteractionUIViewModel();
            _interactionUIViewModel.Initialize(Player.transform);
            
            interactionUIView.Initialize(_interactionUIViewModel);
        }

        private void Update()
        {
            if (!_interactionUIViewModel.IsInteractableExist()) return;
            
            _interactionUIViewModel.SetInteractionEnable(IsInteractionEnable());
            
            _interactionUIViewModel.UpdateInteractableOrderAndIndex();
        }

        public void AddCloseItem(IInteractable interactable)
        {
            _interactionUIViewModel.AddInteractable(interactable);
        }

        public void TryRemoveCloseItem(IInteractable interactable)
        {
            _interactionUIViewModel.TryRemove(interactable);
        }

        public bool IsInteractionExist()
        {
            return _interactionUIViewModel.IsInteractableExist();
        }

        private bool IsInteractionEnable()
        {
            return Player.StateMachine.ChangeStateEnable(typeof(InteractionState));
        }

        public void Interaction()
        {
            // 현재 display 중인 Interaction
            var displayingInteractable = GetDisplayingInteractable();

            TryRemoveCloseItem(displayingInteractable);

            _playingInteractable = displayingInteractable;
            _playingInteractable.Interact(this);
        }

        public void EndInteraction()
        {
            _playingInteractable.OnInteractionEnd();
            _playingInteractable = null;
        }

        private IInteractable GetDisplayingInteractable()
        {
            return _interactionUIViewModel.GetFocusedInteractable();
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
            _playingInteractable.OnAnimationEvent();
        }
    }
}