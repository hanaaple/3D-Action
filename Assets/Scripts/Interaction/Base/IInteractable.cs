using Player;
using UnityEngine;

namespace Interaction.Base
{
    /// <summary>
    /// 인터랙션 가능한 인터페이스
    /// </summary>
    public interface IInteractable
    {
        public Vector3 GetPosition();
        public void Interact(PlayerInteractor playerInteractor);
        public void OnAnimationEvent();
        public void OnInteractionEnd();
        public string GetUIContext();
        public string GetName();
    }
}