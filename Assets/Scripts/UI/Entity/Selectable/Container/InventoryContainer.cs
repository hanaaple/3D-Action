using UI.Entity.Selectable.Slot;

namespace UI.Entity.Selectable.Container
{
    /// <summary>
    /// BaseInventoryView가 사용하는 Container
    /// </summary>
    public class InventoryContainer : SelectableSlotContainer
    {
        public EquipmentType slotType;
    }
}