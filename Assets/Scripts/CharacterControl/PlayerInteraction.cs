using System.Collections.Generic;
using System.Linq;
using CharacterControl.State;
using Interaction;
using TMPro;
using UnityEngine;

namespace CharacterControl
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject interactionUIPanel;

        [SerializeField] private TMP_Text interactionUIText;

        [SerializeField] private Color interactableColor;
        [SerializeField] private Color unInteractableColor;

        private Player _player;
        
        private readonly List<IInteractable> _closeInteractionItems = new();

        private void Start()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            if (!IsInteractionExist())
                return;

            interactionUIText.color = IsInteractionEnable() ? interactableColor : unInteractableColor;
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
            return _player.StateMachine.ChangeStateEnable(typeof(InteractionState));
        }

        public void Loot()
        {
            var closeInteraction = _closeInteractionItems
                .OrderBy(item => Vector3.Distance(transform.position, item.GetPosition()))
                .First();

            TryRemoveCloseItem(closeInteraction);

            closeInteraction.Interact(this);
        }
    }
}