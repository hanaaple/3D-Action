using Interaction;
using Interaction.Base;
using TMPro;
using UI.Base;
using UnityEngine;

namespace UI.View.Play
{
    // InteractionUIController에 의해 작동한다.
    
    // 타 View들은 View에서 직접 ViewModel에 Observe하지만, 해당 클래스에서는 Controller에 의해 제어된다.
    // View를 추가하거나 변경하는 과정에서 유지보수에 단점으로 느껴진다.
    public sealed class InteractionBaseUIView : BaseUIEntity
    {
        [SerializeField] private GameObject viewGameObject;

        [SerializeField] private RectTransform interactionUI;
        [SerializeField] private TMP_Text interactionUIText;
        [SerializeField] private GameObject focusIndexingText;

        [SerializeField] private Color interactableColor;
        [SerializeField] private Color unInteractableColor;
        
        private UIInput _uiInput;
        private RectTransform _rectTransform;
        private float _uiHeight;
        
        public void Initialize(InteractionUIController interactionUIController)
        {
            _rectTransform = GetComponent<RectTransform>();
            _uiHeight = _rectTransform.sizeDelta.y;
            // Decision은 InputBuffer에 의해 작동한다.
            // Controller를 모르는 상태이면 좋겠음.
            _uiInput = new UIInput
            {
                LeftArrow = () => interactionUIController.SetFocusIndex(IndexMoveType.Previous),
                RightArrow = () => interactionUIController.SetFocusIndex(IndexMoveType.Next),
                IsRightArrowActive = () => focusIndexingText.activeSelf,
                IsLeftArrowActive = () => focusIndexingText.activeSelf
            };
        }

        protected override UIInput GetUIInput()
        {
            return _uiInput;
        }

        // onEnable, onDisable
        // On Add or Remove
        public void UpdateUI(bool isEnable, bool indexingEnable)
        {
            //Debug.Log($"UpdateUI({isEnable}, {indexingEnable})");
            viewGameObject.SetActive(isEnable);
            focusIndexingText.gameObject.SetActive(indexingEnable);

            if (indexingEnable)
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _uiHeight);
                
                interactionUI.anchorMin = new Vector2(0, 0.5f);
                interactionUI.anchorMax = new Vector2(1, 1f);
            }
            else
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _uiHeight * 0.5f);
                
                interactionUI.anchorMin = new Vector2(0, 0f);
                interactionUI.anchorMax = new Vector2(1, 1f);
            }

            interactionUI.offsetMin = new Vector2(interactionUI.offsetMin.x, 0);
            interactionUI.offsetMax = new Vector2(interactionUI.offsetMax.x, 0);
        }

        // OnViewEnable, OnUpdateIndexing, OnSort
        public void UpdateTextContext(IInteractable interactable)
        {
            if(interactable == null) return;
            
            interactionUIText.text = $"E: {interactable.GetName()} {interactable.GetUIContext()}";
        }

        // On Interactable Change
        public void UpdateTextColor(bool isInteractable)
        {
            interactionUIText.color = isInteractable ? interactableColor : unInteractableColor;
        }
    }
}
