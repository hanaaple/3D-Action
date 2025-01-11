using System;
using System.Collections.Generic;
using Save;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// 데이터를 관리
    /// ViewModel을 초기화
    /// 단일 플레이 씬에 대해서만 구현되어있다. 
    /// </summary>
    [Serializable]
    public class PlayDataManager : MonoBehaviour
    {
        private static PlayDataManager _instance;
        public static PlayDataManager instance => _instance;

        [NonSerialized] public bool IsLoad;
        
        // Label, Id
        private Dictionary<string, Dictionary<string, ISavableObject>> _registedSavable = new();


        // Add Item -> Item 중에서 선택해서 EquipData.Equip 인데

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
            else
            {
                DestroyImmediate(_instance);
            }
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        /// <summary>
        /// Include Clear
        /// </summary>
        public void Save(bool isClear = true)
        {
            // SaveData가 없는 상태라면?
            // Scene 변경이 된 상태라면?
            var saveData = IsLoad ? SaveManager.Load() : new SaveData();
            
            foreach (var (_, savableList) in _registedSavable)
            {
                foreach (var (_, savable) in savableList)
                {
                    saveData.AddOrUpdate(savable);
                }
            }

            // 그렇다면 LoadData도 유지되어야한다.
            SaveManager.Save(saveData, isClear);
            
            if(isClear) Clear();
        }
        
        public bool TryLoad(out SaveData saveData)
        {
            // SaveData가 없다면? -> 정확히는 NewGame이라 Save가 아니라면.
            saveData = IsLoad ? SaveManager.Load() : null;

            return saveData != null;
        }
        
        public void RegistSavable(ISavableObject savableObject)
        {
            var label = savableObject.GetLabel();
            if (!_registedSavable.ContainsKey(label))
            {
                _registedSavable.Add(label, new Dictionary<string, ISavableObject>());
            }
            
            if (_registedSavable[label].ContainsKey(savableObject.GetId()))
            {
                Debug.LogWarning($"새로운 Id 생성, {savableObject.GetName()}");
                savableObject.GenerateNewId();
            }
            
            _registedSavable[label].Add(savableObject.GetId(), savableObject);
        }
        
        private void Clear()
        {
            _registedSavable.Clear();
        }
    }
}