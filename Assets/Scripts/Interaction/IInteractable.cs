using CharacterControl;
using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        public Vector3 GetPosition();
        public void Interact(PlayerInteraction playerInteraction);
    }
}