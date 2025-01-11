using System;
using System.Collections.Generic;

namespace Save
{
    /// <summary>
    /// Label, Id를 나눈 이유?
    /// </summary>
    [Serializable]
    public class SaveData
    {
        // TODO: Check private saveMap is serialized?
        // Label(Scene - (씬의 상태 - 몬스터, 루팅 등 ), Player, Story - MainStream, Quest, Npc) & Id
        private Dictionary<string, Dictionary<string, IObjectSaveData>> _saveMap = new();

        // public void Add(string label, Dictionary<string, ISaveData> interactions)
        // {
        //     _saveMap[label] = interactions;
        // }
        
        public void AddOrUpdate(ISavableObject savableObject)
        {
            var label = savableObject.GetLabel();
            
            if (!_saveMap.ContainsKey(label))
            {
                _saveMap[label] = new Dictionary<string, IObjectSaveData>();
            }

            var id = savableObject.GetId();
            var saveData = savableObject.GetSaveData();
            
            _saveMap[label][id] = saveData;
        }
        
        public T GetSaveData<T>(string label, string id) where T : class, IObjectSaveData
        {
            if (_saveMap.TryGetValue(label, out var saveDataDictionary))
            {
                if (saveDataDictionary.TryGetValue(id, out var saveData))
                {
                    return (T)saveData;
                }
            }
            
            return null;
        }
        
        public IObjectSaveData GetSaveData(string label, string id)
        {
            if (_saveMap.TryGetValue(label, out var saveDataDictionary))
            {
                if (saveDataDictionary.TryGetValue(id, out var saveData))
                {
                    return saveData;
                }
            }
            
            return null;
        }
    }
}