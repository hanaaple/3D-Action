using UnityEngine;

namespace UI.Entity.Base
{
    /// <summary>
    /// UI View
    /// ViewModel에 의해 전해진 Data로 View를 Display한다
    /// </summary>
    public abstract class UIEntity : MonoBehaviour
    {
        protected virtual void UpdateView()
        {
        }

        private void OnEnable()
        {
            UpdateView();
        }

        protected bool IsUpdateEnable()
        {
            return gameObject.activeSelf;
        }
    }
}