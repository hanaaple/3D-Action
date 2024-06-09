using System;
using Data.Play;

namespace Util
{
    [Serializable]
    public class SaveData
    {
        public StatusData statusData;
        public EquippedItemData equippedItemData;
        public PlayerData playerData;
        public OwnedItemData ownedItemData;
    }
}