using System;
using System.Text;
using Data.Item.Data;
using UI.View.Entity;
using UnityEngine;

namespace Data.Item.Base
{
    public enum ItemType
    {
        Weapon,
        Armor,
        Accessory,
        Tool,
        //Valuable
    }
    
    /// <summary>
    /// 아이템 데이터, ItemData를 기반으로 Clone하여 아이템을 얻는다.
    /// </summary>
    [Serializable]
    public abstract class BaseItem
    {
        // For Debug
        [HideInInspector] public string scriptableObjectName;
        [HideInInspector] public string instanceId;     // Clone()으로 생성 시 할당되는 Instance Id -> 사용하질 않음?

        // Scriptable Object인 ItemData는 저장할 수 없어 대신 ID를 저장하여 ScriptableObjectManager를 통해 ItemStaticData를 부른다.
        // 아이템 고유 Id, 같은 아이템은 다른 인스턴스여도 동일한 Id를 공유한다.
        public string id { get; private set; }
        
        public abstract ItemType itemType { get; }  // 가장 대표적인 타입
        public abstract DescribeViewType describeType { get; }

        // 여기서 의존성이 발생하지는 않음.
        // 단 GetItem을 사용하느 곳에서 Item의 Type에 대한 의존성이 발생함. 그렇다고, 모든 ItemType을 하나의 Enum에서 관리할 수 없음.
        public abstract string GetItemDetailType();

        // ItemType -> (Class에 따라 다름) -> ItemDetailType
        // 결국 중요한건 Equal 인지 아닌지
        public virtual bool Equals(string comparison)
        {
            if (comparison == itemType.ToString())
                return true;
            
            if (comparison == GetItemDetailType())
                return true;

            return false;
        }
        
        // Constructor로 하고 싶지만, ItemData를 미리 넣어주기 위해 생성자 -> 초기화로 변경
        protected void Initialize()
        {
            instanceId = Guid.NewGuid().ToString();
            
            var itemData = GetItemData();
            if (itemData != null)
            {
                scriptableObjectName = itemData.name;
                if (string.IsNullOrEmpty(id))
                {
                    id = itemData.id;
                }
            }
            else
            {
                Debug.LogError("ItemData is null");
            }

            // TODO: - Load 시에 Constructor가 되나?
            Debug.Log($"Constructor Item - {scriptableObjectName}, {instanceId}, {id}");
        }
        
        public abstract ItemStaticData GetItemData();
        public abstract BaseItem Clone();
        public abstract bool GetIsDuplicable();
        public abstract void SetItemData(ItemStaticData itemStaticData);
        
        // 중복이 가능한 경우 구현 X
        public virtual void TryAddCount()
        {
            if (GetIsDuplicable())
            {
                Debug.LogError("중복 가능한 아이템을 추가 시도를 하고 있습니다.");
            }
        }

        // 수식어 + 이름 + 강화 수치
        public virtual string GetItemDisplayName()
        {
            var itemData = GetItemData();
            return itemData != null ? itemData.itemName : "-";
        }

        public virtual string GetItemDisplayData()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("ScriptableObject Name: ");
            stringBuilder.Append(scriptableObjectName);
            stringBuilder.Append("\n");
            stringBuilder.Append("Item Name: ");
            stringBuilder.Append(GetItemDisplayName());
            stringBuilder.Append("\n");
            stringBuilder.Append("Id: ");
            stringBuilder.Append(id);
            stringBuilder.Append("\n");
            stringBuilder.Append("Instance Id: ");
            stringBuilder.Append(instanceId);
            stringBuilder.Append("\n");
            stringBuilder.Append(itemType);
            stringBuilder.Append("  ");
            stringBuilder.Append(GetItemDetailType());
            return stringBuilder.ToString();
        }
    }

    public static class ItemExtensions
    {
        // Inspector View에 노출되며 자동으로 인스턴스화 될 수도 있다.
        // TODO: IsNullOrEmpty인데 Default Item도 겸해서 체크하고 있음 전반적으로 확인 필요
        // -> 맨손, 맨몸만 체크하면 될듯 ㅇㅇ -> 맨몸은 그런데 왜 넣어둔겨? 진짜 궁금해서 왜?
        
        // 진짜 Null인지 확인하는거랑 맨손인지 확인하는 2가지가 같이 사용됨.
        public static bool IsNullOrEmpty(this BaseItem baseItem)
        {
            return baseItem == null || baseItem.GetItemData() == null;
        }
        
        public static bool IsNullOrBare(this Weapon weapon)
        {
            if (weapon.IsNullOrEmpty()) return true;
            
            return weapon.GetWeaponData().isBare;
        }
        
        public static bool IsNullOrBare(this Armor armor)
        {
            if (armor.IsNullOrEmpty()) return true;
            
            return armor.GetArmorData().isBare;
        }
        
        public static bool IsNullOrBare(this BaseItem item)
        {
            if (item.IsNullOrEmpty()) return true;

            if (item is Armor armor) return armor.IsBare();
            if (item is Weapon weapon) return weapon.IsBare();

            return false;
        }
        
        public static bool IsBare(this BaseItem item)
        {
            if (item is Armor armor) return armor.IsBare();
            if (item is Weapon weapon) return weapon.IsBare();

            return false;
        }
        
        public static bool IsBare(this Weapon weapon)
        {
            return weapon.GetWeaponData().isBare;
        }
        
        public static bool IsBare(this Armor armor)
        {
            return armor.GetArmorData().isBare;
        }
    }
}