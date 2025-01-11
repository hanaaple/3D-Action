using Data.Item.Base;
using Data.Item.Data;
using TMPro;

namespace UI.View.Describe
{
    public class AccessoryDescribeView : BaseViewState
    {
        public TMP_Text weight;
        
        public TMP_Text itemEffect;

        public override void UpdateSelect(BaseItem item)
        {
            if (item.IsNullOrEmpty())
            {
                // 전부 -로 넣기
                
                itemName.text = "-";
                weight.text = "-";
                
                itemImage.enabled = false;
                itemImage.sprite = null;

                itemEffect.text = "-";
            }
            else
            {
                var accessory = item as Accessory;
                var accessoryData = accessory.GetAccessoryData();
                if (item.IsBare())
                {
                    itemImage.enabled = false;
                    itemImage.sprite = null;
                }
                else
                {
                    itemImage.enabled = true;
                    itemImage.sprite = accessoryData.slotSprite;    
                }
                
                itemName.text = accessory.GetItemDisplayName();
                weight.text = accessoryData.weight.ToString();

                itemEffect.text = accessoryData.itemEffect;
            }
        }
    }
}