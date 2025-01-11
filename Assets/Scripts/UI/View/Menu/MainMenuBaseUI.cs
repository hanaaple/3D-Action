using System;
using UI.Base;
using UI.Selectable.Container;
using UI.View.Equip;
using UI.View.Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.View.Menu
{
    /// <summary>
    /// Main Menu View
    /// </summary>
    public class MainMenuBaseUI : UIContainerEntity
    {
        [SerializeField] private SelectableSlotContainer selectableSlotContainer;
        
        [Space(10)]
        [SerializeField] private Button equipment;
        [SerializeField] private Button inventory;
        [SerializeField] private Button status;
        [SerializeField] private Button system;

        [SerializeField] private EquipView equipmentUi;
        [FormerlySerializedAs("itemContainerUi")] [SerializeField] private InventoryView inventoryUi;
        [SerializeField] private StatusView statusUi;
        [SerializeField] private SystemView systemUi;

        protected override SelectableSlotContainer GetCurrentContainer()
        {
            return selectableSlotContainer;
        }

        private void Start()
        {
            equipment.onClick.AddListener(equipmentUi.Push);
            inventory.onClick.AddListener(inventoryUi.Push);
            status.onClick.AddListener(statusUi.Push);
            system.onClick.AddListener(systemUi.Push);
        }

        public override void Travel(Action<BaseUIEntity> action)
        {
            base.Travel(action);
            equipmentUi.Travel(action);
            inventoryUi.Travel(action);
            statusUi.Travel(action);
            systemUi.Travel(action);
        }
    }
}