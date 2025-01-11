using Data.Item.Base;
using UnityEngine;

namespace Data.Item.Scriptable
{
    public enum ToolType
    {
        Expendable,
        Charging
    }
    
    [CreateAssetMenu(fileName = "Tool", menuName = "Item/Tool")]
    public class ToolStaticData : ItemStaticData
    {
        public ToolType toolType;
        public string toolEffect;
        
        public bool GetIsDuplicable()
        {
            return false;
        }
    }
}