using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuView : MonoBehaviour
    { 
        [SerializeField] private Button equipment;
        [SerializeField] private Button inventory;
        [SerializeField] private Button status;
        // [SerializeField] private Button system;

        [SerializeField] private GameObject equipmentUi;
        [SerializeField] private GameObject inventoryUi;
        [SerializeField] private GameObject statusUi;
        
        [SerializeField] private PlayerInput playerInput;
        
        
        private void Start()
        {
            equipment.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                equipmentUi.SetActive(true);
            });
            
            inventory.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                inventoryUi.SetActive(true);
            });
            
            status.onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
                statusUi.SetActive(true);
            });
        }

        private void OnEnable()
        {
            playerInput.SwitchCurrentActionMap("Move");
        }

        private void OnDisable()
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
    }
}