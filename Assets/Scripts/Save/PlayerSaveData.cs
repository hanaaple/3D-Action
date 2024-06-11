using System;
using Data.Play;

namespace Save
{
    [Serializable]
    public class PlayerSaveData
    {
        public StatusData statusData;
        public EquippedItemData equippedItemData;
        public PlayerData playerData;
        public OwnedItemData ownedItemData;
    }
}