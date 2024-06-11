using System;
using System.Collections.Generic;

namespace Save
{
    [Serializable]
    public class SaveData
    {
        public PlayerSaveData playerSaveData;

        public Dictionary<string, InteractableSaveData> InteractableSaveData;
    }
}