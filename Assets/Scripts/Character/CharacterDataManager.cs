using System;
using UnityEngine;
using Util;

namespace Character
{
    // Npc, Player, Monster ...
    public abstract class CharacterDataManager : MonoBehaviour
    {
        // public abstract void LoadData(SaveData saveData);
        // public abstract ISaveData GetSaveData();
        [UniqueId] public string id;
        public string characterName;
        
        // public abstract void LoadOrCreateData();
        // public abstract void LoadData(ISaveData saveData);
        // public abstract void CreateData();
        // public abstract string GetLabel();
        // public abstract ISaveData GetSaveData();
        
        public string GetName()
        {
            return characterName;
        }
        public string GetId()
        {
            return id;
        }
        
        public void GenerateNewId()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}