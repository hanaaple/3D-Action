using UnityEngine;

namespace Util
{
    /// <summary>
    /// 각 Action State에서 관찰하여 작동
    /// </summary>
    public class AnimationEventHandler : MonoBehaviour
    {
        public delegate void RotationHandler(bool isRotateEnable);
        public delegate void ComboHandler(bool isComboEnable);

        public event RotationHandler OnRotationEnableChanged;
        public event ComboHandler OnComboEnableChanged;
        
        public void SetRotationEnable(AnimationEvent animationEvent)
        {
            OnRotationEnableChanged?.Invoke(true);
        }
        
        public void SetRotationDisable(AnimationEvent animationEvent)
        {
            OnRotationEnableChanged?.Invoke(false);
        }

        public void SetComboEnable(AnimationEvent animationEvent)
        {
            OnComboEnableChanged?.Invoke(true);
        }
        
        public void SetComboDisable(AnimationEvent animationEvent)
        {
            OnComboEnableChanged?.Invoke(false);
        }
    }
}
