using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace UI.Selectable.Slot
{
    public enum NavigationType
    {
        Explicit,
        Implicit,
    }

    /// <summary>
    /// Selectable 오브젝트
    /// 마우스 관련 (Press, Select, Click) 기능 구현
    /// Container에 의해 관리됩니다.
    /// </summary>
    public class SelectableSlot : MonoBehaviour
    {
        [FormerlySerializedAs("slotType")] [SerializeField] public NavigationType navigationType;
        
        [ConditionalHideInInspector("navigationType", NavigationType.Explicit)]
        public SelectableSlot left;
        [ConditionalHideInInspector("navigationType", NavigationType.Explicit)]
        public SelectableSlot right;
        [ConditionalHideInInspector("navigationType", NavigationType.Explicit)]
        public SelectableSlot down;
        [ConditionalHideInInspector("navigationType", NavigationType.Explicit)]
        public SelectableSlot up;

        [SerializeField] private Button button;
        [SerializeField] private Animator animator;

        private event UnityAction OnClickActions;
        
        private Action<SelectableSlot> _onBeginSelectAction;
        // public Action OnHighlight;
        // public Action OnDeHighlight;
        
        private readonly int _animIdSelected = Animator.StringToHash("Selected");
        private readonly int _highlighted = Animator.StringToHash("Highlighted");
        // Normal
        // Pressed
        // Disabled
        
        // Temp
        private delegate bool OnClickDelegate();

        protected void Awake()
        {
            button = GetComponent<Button>();
            animator = GetComponent<Animator>();
         
            if(OnClickActions != null)
                button.onClick.AddListener(OnClickActions);
            // 항상 Trigger를 쓰진 않음. 크아아아악 -> 쓰던데?
            AddEventTrigger(EventTriggerType.PointerEnter, Select);
        }

        public void Clear()
        {
            if (button == null)
            {
                // Lazy binding
                OnClickActions = null;
            }
            else
            {
                button.onClick.RemoveAllListeners();
            }
        }
        
        public void AddListener(UnityAction action)
        {
            if (button == null)
            {
                // Lazy binding
                OnClickActions += action;
            }
            else
            {
                button.onClick.AddListener(action);
            }
        }

        public virtual void Initialize(Action<SelectableSlot> onBeginSelect)
        {
            _onBeginSelectAction = onBeginSelect;
        }
        
        public void Decision()
        {
            // if (!IsInteractable()) return;
            button.onClick?.Invoke();
        }

        public virtual bool Select()
        {
            if(!SelectEnable()) return false;
            
            // Awake 이전에 animator를 하네요..
            // GameObject가 꺼져있는데 Animator를 사용하네요
            //Debug.LogWarning($"Select {_onBeginSelectAction == null}   {animator == null}");
            _onBeginSelectAction?.Invoke(this);
            animator?.SetBool(_animIdSelected, true);

            return true;
        }

        public void DeSelect()
        {
            animator?.SetBool(_animIdSelected, false);
        }
        
        public SelectableSlot GetSelectNext(Direction direction)
        {
            var slot = direction switch
            {
                Direction.Left => left,
                Direction.Right => right,
                Direction.Up => up,
                Direction.Down => down,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
            
            return slot;
        }

        // private void OnHighlight()
        // {
        //     _animator?.SetBool(_highlighted, true);
        // }
        //
        // private void OnDeHighlight()
        // {
        //     _animator?.SetBool(_highlighted, false);
        // }

        private void AddEventTrigger(EventTriggerType eventTriggerType, OnClickDelegate action)
        {
            var eventTrigger = button.GetComponent<EventTrigger>();
            var entry = eventTrigger.triggers.Find(item => item.eventID == eventTriggerType);
        
            if (entry == null)
            {
                entry = new EventTrigger.Entry
                {
                    eventID = eventTriggerType
                };
                eventTrigger.triggers.Add(entry);
            }
        
            entry.callback.AddListener(_ => { action?.Invoke(); });
        }

        public void SetEnable(bool isEnable)
        {
            // Enable이 False인 경우 Arrow를 통한 것도 안되야됨.
            
            button.GetComponent<EventTrigger>().enabled = isEnable;
            button.enabled = isEnable;
        }

        public bool SelectEnable()
        {
            return button.enabled;
        }
    }
}