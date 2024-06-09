using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Entity.Selectable.Slot
{
    public class SelectableSlot : Button, IPointerMoveHandler
    {
        private Animator _animator;
        private readonly int _animIdSelected = Animator.StringToHash("Selected");

        public Action<SelectableSlot> SelectAction;
        // private Action<SelectableSlot> OnDeSelectAction;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        public override void Select()
        {
            base.Select();
         
            if (EventSystem.current == null || EventSystem.current.alreadySelecting)
                return;

            _animator.SetBool(_animIdSelected, true);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SelectAction?.Invoke(this);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            Select();
        }

        // Submit 막기
        public override void OnSubmit(BaseEventData eventData)
        {
        }

        public void Click()
        {
            onClick?.Invoke();
        }
    }
}