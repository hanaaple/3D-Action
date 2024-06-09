using UnityEngine;

namespace UI.Entity.Base
{
    // UI View
    // 오직 입력 (버튼)에 대한 결과만을 책임진다.
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