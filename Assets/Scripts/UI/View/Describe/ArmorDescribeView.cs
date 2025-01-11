using Data.Item.Base;
using Data.Item.Data;
using TMPro;

namespace UI.View.Describe
{
    public class ArmorDescribeView : BaseViewState
    {
        public TMP_Text weight;
        
        public TMP_Text physics;
        public TMP_Text antiStrike;
        public TMP_Text antiPierce;
        public TMP_Text antiSlash;

        public override void UpdateSelect(BaseItem item)
        {
            if (item.IsNullOrEmpty())
            {
                // 전부 -로 넣기
                
                itemName.text = "-";
                weight.text = "-";
                
                itemImage.enabled = false;
                itemImage.sprite = null;

                physics.text = "-";
                antiStrike.text = "-";
                antiPierce.text = "-";
                antiSlash.text = "-";
            }
            else
            {
                var armor = item as Armor;
                var armorData = armor.GetArmorData();
                if (item.IsBare())
                {
                    itemImage.enabled = false;
                    itemImage.sprite = null;
                }
                else
                {
                    itemImage.enabled = true;
                    itemImage.sprite = armorData.slotSprite;    
                }
                
                itemName.text = armor.GetItemDisplayName();
                
                weight.text = armorData.weight.ToString();

                physics.text = armorData.defense.ToString();
                antiStrike.text = armorData.antiStrike.ToString();
                antiPierce.text = armorData.antiPierce.ToString();
                antiSlash.text = armorData.antiSlash.ToString();
            }
        }
    }
}