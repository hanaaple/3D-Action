using Data.Item.Base;
using TMPro;
using UI.View.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Describe
{
    public abstract class BaseViewState : MonoBehaviour
    {
        public DescribeViewType describeViewType;
        
        public TMP_Text itemName;
        public Image itemImage;
        
        // virtual or abstract
        public virtual void OnStateEnter()
        {
            // OnEnable
            gameObject.SetActive(true);
        }

        public virtual void OnStateExit()
        {
            // OnDisable
            gameObject.SetActive(false);
        }

        public abstract void UpdateSelect(BaseItem item);
    }
}