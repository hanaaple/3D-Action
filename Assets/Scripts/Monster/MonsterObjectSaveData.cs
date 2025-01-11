using System;
using Data;
using Data.Play;
using Save;

namespace Monster
{
    [Serializable]
    public class MonsterObjectSaveData : IObjectSaveData
    {
        // public SerializableVector3 position;
        // public SerializableQuaternion rotation;
        public MonsterData monsterData;
        // public bool isDead;
    }
}