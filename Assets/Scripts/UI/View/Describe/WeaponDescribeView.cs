using Data.Item.Base;
using Data.Item.Data;
using TMPro;

namespace UI.View.Describe
{
    public class WeaponDescribeView : BaseViewState
    {
        public TMP_Text weaponType;
        public TMP_Text attackType;
        public TMP_Text weight;
        
        public TMP_Text physics;
        public TMP_Text ignoreDefense;
        
        public TMP_Text strengthWeight;
        public TMP_Text workmanshipWeight;
        public TMP_Text intellectWeight;

        public override void UpdateSelect(BaseItem item)
        {
            if (item.IsNullOrEmpty())
            {
                // 전부 -로 넣기
                
                itemName.text = "-";
                weaponType.text = "-";
                attackType.text = "-";
                weight.text = "-";
                
                itemImage.enabled = false;
                itemImage.sprite = null;

                physics.text = "-";
                ignoreDefense.text = "-";
                strengthWeight.text = "-";
                workmanshipWeight.text = "-";
                intellectWeight.text = "-";
            }
            else
            {
                var weapon = item as Weapon;
                var weaponData = weapon.GetWeaponData();
                if (item.IsBare())
                {
                    itemImage.enabled = false;
                    itemImage.sprite = null;
                }
                else
                {
                    itemImage.enabled = true;
                    itemImage.sprite = weaponData.slotSprite;    
                }
                
                itemName.text = weapon.GetItemDisplayName();
                weaponType.text = $"{weaponData.weaponType.ToString()}  임시";    // 추후 한글로 변경 가능하도록
                attackType.text = $"{weaponData.attackType.ToString()} 임시";
                weight.text = weaponData.weight.ToString();

                physics.text = weaponData.damage.ToString();
                ignoreDefense.text = "임시";
                strengthWeight.text = $"{weaponData.strengthWeight.ToString()} 임시";
                workmanshipWeight.text = $"{weaponData.workmanshipWeight.ToString()} 임시";
                intellectWeight.text = $"{weaponData.intellectWeight.ToString()} 임시";
            }
        }
    }
}
