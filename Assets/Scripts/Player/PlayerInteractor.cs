using Interaction;
using Interaction.Base;
using Player.State.Base;
using UI.View.Play;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 플레이어 인터랙션 관리 (ex - 줍기, 열기, 메시지 읽기 등)
    /// 제약조건 - Scene내 세팅시, GameObject는 꺼져있어야 된다.
    /// </summary>
    public class PlayerInteractor : MonoBehaviour
    {
        private Animator _playerAnimator;   // 고민!
        private ActionPlayer _actionPlayer;   
        private PlayerController _playerController;

        private InteractionUIController _interactionUIController;
        private InteractionBaseUIView _interactionBaseUIView;

        private IInteractable _playingInteractable;
        
        // Interaction View
        // 활성-비활성화시키며
        // 입력에 따른 기능
        
        // Interaction Controller
        // 인터랙션 관련 내부 로직
        // 여러 API 및 기능 제공
        
        public void Initialize(ActionPlayer actionPlayer, PlayerController playerController, InteractionBaseUIView interactionBaseUIView)
        {
            _actionPlayer = actionPlayer;
            _playerController = playerController;
            _interactionBaseUIView = interactionBaseUIView;
            _playerAnimator = GetComponentInChildren<Animator>();   // 기분 나쁨.
            
            _interactionUIController = new InteractionUIController(actionPlayer.transform, _interactionBaseUIView);
            _interactionBaseUIView.Initialize(_interactionUIController);
            
            // TODO: Why OnEnable???
            OnEnable();
        }
        
        // Player가 있을때 -> UI 활성화 & Controller 활성화
        private void OnEnable()
        {
            _interactionUIController?.OnEnable();
        }

        private void OnDisable()
        {
            _interactionUIController.Clear();
        }
        
        private void Update()
        {
            if (!_interactionUIController.GetIsInteractableExist()) return;
            
            // Update -> Player State가 변할때마다
            _interactionUIController.SetInteractionEnable(GetInteractionEnable());
            _interactionUIController.UpdateInteractableOrderAndIndex();
        }

        // 외부에서 PlayerInteractor.Interact -> Inteaction.Interact -> PlayerInteractor.Something()
        public void Interact()
        {
            var displayingInteractable = _interactionUIController.GetFocusedInteractable();

            // TryRemoveCloseItem(displayingInteractable);
            
            _playingInteractable = displayingInteractable;
            _playingInteractable.Interact(this);
        }

        public void AddCloseItem(IInteractable interactable)
        {
            _interactionUIController.AddInteractable(interactable);
        }

        public void TryRemoveCloseItem(IInteractable interactable)
        {
            _interactionUIController.RemoveInteractable(interactable);
        }
        
        public void Teleport(Transform targetTransform)
        {
            _playerController.Teleport(targetTransform);
        }
        
        public void InitMoveState()
        {
            _playerController.InitMoveState();
        }

        public void OnAnimationEvent()
        {
            _playingInteractable.OnAnimationEvent();
        }
        
        public void EndInteraction()
        {
            _playingInteractable.OnInteractionEnd();
            _playingInteractable = null;
        }
        
        public void ChangePlayerStateByInputOrIdle()
        {
            _actionPlayer.StateMachine.ChangeStateByInputOrIdle();
        }
        
        public void AnimatePlayer(int triggerAnimationHash)
        {
            _playerAnimator.SetTrigger(triggerAnimationHash);
        }
        
        
        public bool IsInteractionExist()
        {
            return _interactionUIController.GetIsInteractableExist();
        }

        private bool GetInteractionEnable()
        {
            return _actionPlayer.StateMachine.ChangeStateEnable(PlayerStateMode.Interaction);
        }
    }
}