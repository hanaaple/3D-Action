using System.ComponentModel;
using Data;
using Data.PlayItem;
using Data.Static.Scriptable;
using TMPro;
using UI.Entity.Base;
using UI.Entity.Selectable.Slot;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Entity
{
    public class DescribeView : UIEntity
    {
        // Only Display Selected Item

        [SerializeField] private Sprite weaponSprite;
        [SerializeField] private Sprite armorSprite;
        [SerializeField] private Sprite infoSprite;

        [Header("기본")] [SerializeField] private TMP_Text itemName;
        [SerializeField] private Image icon;

        [Header("아이템 타입")] [SerializeField] private GameObject itemType;
        [SerializeField] private TMP_Text itemTypeText;

        [Header("소지 개수")] [SerializeField] private GameObject possession;
        [SerializeField] private TMP_Text possessionCurrentText;
        [SerializeField] private TMP_Text possessionMaxText;

        [Header("중량")] [SerializeField] private GameObject weight;
        [SerializeField] private TMP_Text weightText;

        [Space(20)] [SerializeField] private GameObject itemEffect;
        [SerializeField] private Image effectIcon;
        [SerializeField] private TMP_Text effectNameText;
        [SerializeField] private TMP_Text effectContextText;

        // [SerializeField] private TMP_Text itemName;

        private void Awake()
        {
            var describeViewModel = DataManager.instance.selectedUiViewModel;
            describeViewModel.PropertyChanged += UpdateDescribeView;
        }

        protected override void UpdateView()
        {
            UpdateDescribeView(null, null);
        }

        private void UpdateDescribeView(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;
            // Update Text & Image

            var describeViewModel = DataManager.instance.selectedUiViewModel;
            var selectedSlot = describeViewModel.selectedItemSlotData;

            itemType.SetActive(false);
            possession.SetActive(false);
            weight.SetActive(false);
            // itemEffect.SetActive(false);

            possessionCurrentText.text = "-";
            possessionMaxText.text = "-";
            itemName.text = "-";
            itemTypeText.text = "-";
            weightText.text = "-";
            effectIcon.sprite = infoSprite;
            effectNameText.text = "아이템 효과";
            effectContextText.text = "-";
            icon.enabled = false;
            icon.sprite = null;

            if (selectedSlot == null) return;

            if (selectedSlot.equipmentType == EquipmentType.Weapon)
            {
                itemType.SetActive(true);
                weight.SetActive(true);
                effectIcon.sprite = weaponSprite;

                effectNameText.text = "공격력";

                var weapon = selectedSlot.GetItem<Weapon>();
                
                if (!weapon.IsNullOrEmpty())
                {
                    var weaponData = weapon.GetItemData() as WeaponData;

                    itemName.text = weapon.GetItemName();

                    itemTypeText.text = weaponData.weaponType.ToString();
                    weightText.text = weaponData.weight.ToString();

                    // 물리 10
                    // 물리 -
                    // 이런식으로
                    // effectContextText.text = weaponData.damage.ToString();

                    icon.enabled = true;
                    icon.sprite = weaponData.slotSprite;
                }
            }
            else if (selectedSlot.equipmentType == EquipmentType.Armor)
            {
                weight.SetActive(true);
                effectIcon.sprite = armorSprite;

                effectNameText.text = "방어력";

                var armor = selectedSlot.GetItem<Armor>();

                if (!armor.IsNullOrEmpty())
                {
                    var armorData = armor.GetItemData() as ArmorData;
                    
                    itemName.text = armor.GetItemName();

                    weightText.text = armorData.weight.ToString();

                    // 물리 10
                    // 물리 -
                    // 이런식으로
                    // effectContextText.text = armorData.itemDescription;

                    icon.enabled = true;
                    icon.sprite = armorData.slotSprite;
                }
            }
            else if (selectedSlot.equipmentType == EquipmentType.Accessory)
            {
                weight.SetActive(true);
                effectIcon.sprite = infoSprite;

                var accessory = selectedSlot.GetItem<Accessory>();

                if (!accessory.IsNullOrEmpty())
                {
                    var accessoryData = accessory.GetItemData() as AccessoryData;
                    
                    itemName.text = accessory.GetItemName();

                    weightText.text = accessoryData.weight.ToString();

                    effectContextText.text = accessoryData.itemDescription;

                    icon.enabled = true;
                    icon.sprite = accessoryData.slotSprite;
                }
            }
            else if (selectedSlot.equipmentType == EquipmentType.Tool)
            {
                itemType.SetActive(true);
                possession.SetActive(true);
                
                var tool = selectedSlot.GetItem<Tool>();
                
                if (!tool.IsNullOrEmpty())
                {
                    var toolData = tool.GetItemData() as ToolData;

                    itemName.text = tool.GetItemName();

                    itemTypeText.text = tool.toolType.ToString();

                    possessionCurrentText.text = tool.possessionCount.ToString();
                    possessionMaxText.text = tool.maximumPossessionCount.ToString();

                    effectContextText.text = toolData.itemDescription;

                    icon.enabled = true;
                    icon.sprite = toolData.slotSprite;
                }
            }
        }
    }
}