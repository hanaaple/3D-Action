using UI.Selectable.Container;
using UnityEngine;

namespace UI.Base
{
    // 단일 Container

    // View, ContainerView가 있다.
    // View
    public abstract class UIContainerEntity : BaseUIEntity
    {
        private UIInput _uiInput;

        // 기존 여러 Container를 보유 가능하도록 하였으나, 이로 인한 복잡함만 생겨나 단일 Container만 보유 가능하도록 수정.
        // 이후 Container에서 여러 Container를 보유하게 되면서 Container -> containers가 되버림.
        
        protected abstract SelectableSlotContainer GetCurrentContainer();

        protected virtual void Awake()
        {
            _uiInput = new UIInput
            {
                Decision = () => GetCurrentContainer().Decision(),
                UpArrow = () => GetCurrentContainer().SelectNext(Direction.Up),
                LeftArrow = () => GetCurrentContainer().SelectNext(Direction.Left),
                RightArrow = () => GetCurrentContainer().SelectNext(Direction.Right),
                DownArrow = () => GetCurrentContainer().SelectNext(Direction.Down),
                IsDecisionActive = () => panel.activeSelf
                // TODO:
                // && selectableSlotContainer.GetInteractable()
                ,
                IsRightArrowActive = () => panel.activeSelf,
                IsLeftArrowActive = () => panel.activeSelf,
                IsDownArrowActive = () => panel.activeSelf,
                IsUpArrowActive = () => panel.activeSelf,
                IsSlotLeftActive = () => panel.activeSelf,
                IsSlotRightActive = () => panel.activeSelf,
            };
        }


        public override void OpenOrLoad()
        {
            base.OpenOrLoad();
            var container = GetCurrentContainer();
            container.gameObject.SetActive(true);
            
            //Debug.LogWarning($"{container.isSelected}");
            
            if (container.isSelected)
            {
                // Select하여 SelectViewModel Update해줘야됨.
                container.Select();
            }
            else
            {
                container.SelectDefault();
            }
        }

        public override void Close(bool isSelectClear = true)
        {
            if (isSelectClear)
                GetCurrentContainer().ClearSelectState();

            base.Close(isSelectClear);
        }

        // protected void SetInteractable(bool isInteractable)
        // {
        //     selectableSlotContainer.SetInteractable(isInteractable);
        // }

        protected override UIInput GetUIInput()
        {
            return _uiInput;
        }
    }
}