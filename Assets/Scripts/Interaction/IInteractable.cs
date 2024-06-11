using CharacterControl;
using UnityEngine;

namespace Interaction
{
    /// <summary>
    /// 인터랙션 가능한 인터페이스
    /// </summary>
    public interface IInteractable
    {
        public Vector3 GetPosition();
        public void Interact(PlayerInteraction playerInteraction);
        public void OnAnimationEvent();
        public void OnInteractionEnd();
        public string GetUIContext();
    }
}