using System;
using Save;

namespace Interaction.Base
{
    [Serializable]
    public class InteractableObjectSaveData : IObjectSaveData
    {
        public bool isInteracted;
    }
}