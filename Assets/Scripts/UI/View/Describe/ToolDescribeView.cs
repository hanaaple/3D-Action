using Data.Item.Base;
using Data.Item.Data;
using TMPro;

namespace UI.View.Describe
{
    public class ToolDescribeView : BaseViewState
    {
        public TMP_Text itemType;
        
        public TMP_Text possession;
        public TMP_Text maxPossession;
        
        public TMP_Text itemEffect;

        public override void UpdateSelect(BaseItem item)
        {
            if (item.IsNullOrEmpty())
            {
                // 전부 -로 넣기
                itemName.text = "-";
                itemType.text = "-";
                
                itemImage.enabled = false;
                itemImage.sprite = null;

                possession.text = "-";
                maxPossession.text = "-";
                
                itemEffect.text = "-";
            }
            else
            {
                var tool = item as Tool;
                var toolData = tool.GetToolData();
                if (item.IsBare())
                {
                    itemImage.enabled = false;
                    itemImage.sprite = null;
                }
                else
                {
                    itemImage.enabled = true;
                    itemImage.sprite = toolData.slotSprite;    
                }

                itemName.text = tool.GetItemDisplayName();
                itemType.text = toolData.toolType.ToString();   // 소모품, 재사용 가능
                
                possession.text = tool.possessionCount.ToString();
                maxPossession.text = tool.maximumPossessionCount.ToString();
                
                itemEffect.text = toolData.toolEffect;
            }
        }
    }
}