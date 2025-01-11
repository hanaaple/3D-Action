using System;
using Data.Play;
using Save;
using Util.SerializableProperty;

namespace Player
{
    [Serializable]
    public class PlayerSaveData : IObjectSaveData
    {
        public SerializableVector3 position;
        public SerializableQuaternion rotation;
        
        public StatusData statusData;
        public CharacterData playerData;
        public EquippedItemData equippedItemData;
        public OwnedItemData ownedItemData;
    }
}