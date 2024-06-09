using UI.Entity.Selectable.Slot;

namespace UI.Entity.Selectable.Container
{
    // BaseInventoryView인 경우 사용하는 Container
    public class InventoryContainer : SelectableSlotContainer
    {
        public EquipmentType slotType;
    }
}