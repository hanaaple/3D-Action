using System;
using UI.Entity.Selectable.Container;

namespace UI.Entity.Base
{
    /// <summary>
    /// PlayUIManager에 의해 Stack으로 관리된다.
    /// 입력 가능한 UI View로 사용된다.
    /// </summary>
    public abstract class UIContainerEntity : UIEntity
    {
        public SelectableSlotContainer[] selectableSlotContainers;

        internal SelectableSlotContainer CurrentSelectableSlotContainer;

        public Action<UIContainerEntity> PushAction;
        public Action PopAction;

        public void Push()
        {
            PushAction?.Invoke(this);
        }
        
        public void Pop()
        {
            PopAction?.Invoke();
        }

        // Container와 Slot들을 Initialize 해줘야됨.
        
        public virtual void Open()
        {
            if (gameObject.activeSelf)
            {
                LoadSelectedItem();
                return;
            }
            
            gameObject.SetActive(true);
            
            InitializeUIState();
        }

        private void InitializeUIState()
        {
            if (selectableSlotContainers.Length > 0)
            {
                CurrentSelectableSlotContainer = selectableSlotContainers[0];
            }
            CurrentSelectableSlotContainer?.SelectDefault();
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        private void LoadSelectedItem()
        {
            CurrentSelectableSlotContainer?.Select();
        }

        /// <summary>
        /// Travel Connected UIEntity, if it's leaf do not implement
        /// </summary>
        public virtual void Travel(Action<UIContainerEntity> action)
        {
            action?.Invoke(this);
        }

        public virtual void OnDecision()
        {
            CurrentSelectableSlotContainer?.Decision();
        }

        public virtual void OnRightArrow()
        {
        }
        
        public virtual void OnLeftArrow()
        {
        }

        public virtual void OnDownArrow()
        {
        }

        public virtual bool IsDecisionActive()
        {
            return gameObject.activeSelf;
        }

        public virtual bool IsRightArrowActive()
        {
            return gameObject.activeSelf;
        }

        public virtual bool IsLeftArrowActive()
        {
            return gameObject.activeSelf;
        }

        public virtual bool IsDownArrowActive()
        {
            return gameObject.activeSelf;
        }
    }
}