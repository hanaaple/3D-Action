using System;
using UI.Entity.Base;
using UI.View.Equip;
using UI.View.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    /// <summary>
    /// Main Menu View
    /// </summary>
    public class MainMenuUI : UIContainerEntity
    {
        [Space(10)]
        [SerializeField] private Button equipment;
        [SerializeField] private Button inventory;
        [SerializeField] private Button status;
        [SerializeField] private Button system;

        [SerializeField] private EquipView equipmentUi;
        [SerializeField] private InventoryView inventoryUi;
        [SerializeField] private StatusView statusUi;
        [SerializeField] private SystemView systemUi;

        private void Start()
        {
            equipment.onClick.AddListener(equipmentUi.Push);
            inventory.onClick.AddListener(inventoryUi.Push);
            status.onClick.AddListener(statusUi.Push);
            system.onClick.AddListener(systemUi.Push);
        }

        public override void Travel(Action<UIContainerEntity> action)
        {
            base.Travel(action);
            equipmentUi.Travel(action);
            inventoryUi.Travel(action);
            statusUi.Travel(action);
            systemUi.Travel(action);
        }
    }
}