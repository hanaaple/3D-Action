using System;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Title
{
    public class TitlePressToStart : BaseUIEntity
    {
        [Header("Press To Start")]
        [SerializeField] private Button pressToStartButton;
        [SerializeField] private Animator pressToStartAnimator;
    
        [Header("Title Menu")]
        [SerializeField] private TitleMenu menuContainer;

        private UIInput _uiInput;
        
        private static readonly int Selected = Animator.StringToHash("Selected");
        
        private void Start()
        {
            pressToStartAnimator.SetBool(Selected, true);
            
            _uiInput = new UIInput
            {
                Decision = () => { pressToStartButton.onClick.Invoke(); },
                IsDecisionActive = () => gameObject.activeSelf,
            };

            pressToStartButton.onClick.AddListener(() =>
            {
                Pop();
                menuContainer.Push();
            });
        }

        public override void Travel(Action<BaseUIEntity> action)
        {
            base.Travel(action);
            menuContainer.Travel(action);
        }

        protected override UIInput GetUIInput()
        {
            return _uiInput;
        }
    }
}
