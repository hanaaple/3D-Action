using System;
using UI.Selectable.Container.Item;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.View.Inventory
{
    /// <summary>
    /// 인벤토리 View
    /// 슬롯 슬라이더에 의해 선택된 슬롯 타입에 따라 Container를 보여준다.
    /// </summary>
    
    
    // 단순히 InventoryView라고 하기에는
    // Npc 구매 판매 강화 인벤 등과 겹침 -> 그러나 
    
    public class InventoryView : BaseItemContainerView
    {
        private InventoryContainer[] _inventoryContainers;
        
        // 1. 마우스를 올리면 해당 방향으로 슬라이딩된다
        // 2. Icon에 마우스를 올리면 해당 Icon으로 Event가 실행된다
        // 3. Highlight 시, 자동으로 슬라이딩이 된다.
        
        // 슬롯 Icon의 역할이 다른게 있나? -> 인스펙터에서 컨테이너를 설정하기 위해서는 
        [SerializeField] private InventorySlotIcon[] slotIcons;

        [SerializeField] private RectTransform slideTarget;
        
        protected override void Awake()
        {
            //Debug.LogWarning("Awake");
            // z or x -> Highlight & Event

            // Awake 이전에 Open이 되네요.
            _inventoryContainers = new InventoryContainer[slotIcons.Length];
            // slideIcon.equipType -> 에 따라 Container Open
            // Highlight & Event
            for (var index = 0; index < _inventoryContainers.Length; index++)
            {
                var slideIcon = slotIcons[index];
                var targetIndex = index;
                AddEventTrigger(slideIcon.slotIcon, EventTriggerType.PointerEnter, () =>
                {
                    SetIndex(targetIndex);
                    // slideIcon.equipType -> 에 따라 Container Open
                    // Highlight & Event
                });

                var container = Instantiate(containerPrefab, containerParent).GetComponent<InventoryContainer>();
                container.Initialize(slideIcon.equipType);
                _inventoryContainers[index] = container;
            }

            base.Awake();
        }

        public override void OpenOrLoad()
        {
            base.OpenOrLoad();
            slotIcons[ContainerIndex].Select(true);
            Slide(ContainerIndex);
        }

        public override void Close(bool isSelectClear = true)
        {
            if(isSelectClear)
                slotIcons[ContainerIndex].Select(false);
            base.Close(isSelectClear);
        }

        protected override void SetIndex(int nextIndex)
        {
            slotIcons[ContainerIndex].Select(false);
            base.SetIndex(nextIndex);
            slotIcons[ContainerIndex].Select(true);
            Slide(ContainerIndex);
        }

        private void Slide(int targetIndex)
        {
            var targetIcon = slotIcons[targetIndex];

            var leftX = targetIcon.rectTransform.anchoredPosition.x - targetIcon.rectTransform.rect.width / 2f;
            var rightX = targetIcon.rectTransform.anchoredPosition.x + targetIcon.rectTransform.rect.width / 2f;

            //Debug.LogWarning($"{slideTarget.anchoredPosition.x}    {leftX}  {rightX}");
            
            // Icon의 왼쪽이 화면의 좌측을 넘어가 안보이면
            if (-slideTarget.anchoredPosition.x > leftX)
            {
                slideTarget.anchoredPosition = new Vector2(-leftX, slideTarget.anchoredPosition.y);
            }
            // Icon의 오른쪽이 화면의 우측을 넘어가 안보이면 -> 계산하다가 어쩌다보니 때려 맞춰서 나온 식임 일단 잘 작동함.
            else if (-slideTarget.anchoredPosition.x + slideTarget.rect.width < rightX)
            {
                slideTarget.anchoredPosition = new Vector2(-(rightX - slideTarget.rect.width), slideTarget.anchoredPosition.y);
            }
        }

        protected override BaseItemContainer[] GetItemContainers()
        {
            return _inventoryContainers;
        }
        
        private static void AddEventTrigger(EventTrigger eventTrigger, EventTriggerType eventTriggerType, Action action)
        {
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
    }
}