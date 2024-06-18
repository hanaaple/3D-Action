using System.ComponentModel;
using Interaction;
using TMPro;
using UI.Entity.Base;
using UnityEngine;

namespace UI.View
{
    public class InteractionUIView : UIContainerEntity
    {
        [SerializeField] private TMP_Text interactionUIText;
        
        [SerializeField] private GameObject focusIndexingText;

        [SerializeField] private Color interactableColor;
        [SerializeField] private Color unInteractableColor;

        private InteractionUIViewModel _interactionUIViewModel;
        
        public void Initialize(InteractionUIViewModel interactionUIViewModel)
        {
            _interactionUIViewModel = interactionUIViewModel;
            _interactionUIViewModel.PropertyChanged += UpdateUI;
        }

        protected override void UpdateView()
        {
            UpdateUI(null, null);
        }
        
        private void UpdateUI(object sender, PropertyChangedEventArgs e)
        {
            if (_interactionUIViewModel.IsInteractableExist())
            {
                if (_interactionUIViewModel.GetInteractableCount() > 1)
                {
                    focusIndexingText.gameObject.SetActive(true);
                }
                else
                {
                    focusIndexingText.gameObject.SetActive(false);
                }
                gameObject.SetActive(true);
                UpdateUIText();
            }
            else
            {
                gameObject.SetActive(false);
                focusIndexingText.gameObject.SetActive(false);
            }
        }

        private void UpdateUIText()
        {
            interactionUIText.color = _interactionUIViewModel.GetInteractionEnable() ? interactableColor : unInteractableColor;
            interactionUIText.text = $"E: {_interactionUIViewModel.GetFocusedInteractable().GetName()} {_interactionUIViewModel.GetFocusedInteractable().GetUIContext()}";
        }

        public override void OnRightArrow()
        {
            _interactionUIViewModel.SetFocusIndexNext();
        }

        public override void OnLeftArrow()
        {
            _interactionUIViewModel.SetFocusIndexPrevious();
        }

        public override bool IsDecisionActive()
        {
            return gameObject.activeSelf;
        }

        public override bool IsRightArrowActive()
        {
            return focusIndexingText.activeSelf;
        }

        public override bool IsLeftArrowActive()
        {
            return focusIndexingText.activeSelf;
        }

        public override bool IsDownArrowActive()
        {
            return false;
        }
    }
}