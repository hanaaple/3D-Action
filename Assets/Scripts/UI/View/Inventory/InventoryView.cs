namespace UI.View.Inventory
{
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