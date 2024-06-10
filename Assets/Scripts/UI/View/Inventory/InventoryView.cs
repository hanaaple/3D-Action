namespace UI.View.Inventory
{
    /// <summary>
    /// 인벤토리 View
    /// 슬롯 슬라이더에 의해 선택된 슬롯 타입에 따라 Container를 보여준다.
    /// </summary>
    public class InventoryView : BaseInventoryView
    {
        // Slot Slide
        // [SerializeField] private EventTrigger[] slotIcons;

        protected override void Awake()
        {
            base.Awake();
            
            // foreach (var eventTrigger in slotIcons)
            // {
            //     var onMouseHover = new EventTrigger.Entry
            //     {
            //         eventID = EventTriggerType.PointerEnter
            //     };
            //     onMouseHover.callback.AddListener(baseEventData =>
            //     {
            //         // 
            //     });
            //     
            //     eventTrigger.triggers.Add(onMouseHover);
            // }
        }
        
        

        // Z
        public void OnSlotLeftt()
        {
        }
        
        // X
        public void OnSlotRightt()
        {
        }
    }
}