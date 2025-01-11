using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Save
{
    /// <summary>
    /// File Data를 읽어오는 클래스.
    /// 1개의 File에 대해서만 지원한다. (확장 가능성)
    /// </summary>
    public static class SaveManager
    {
        private static readonly string SaveFileDirectoryPath = $"{Application.persistentDataPath}/SaveData";
        private static readonly string SaveFilePath = $"{SaveFileDirectoryPath}/saveData.save";

        private static SaveData _loadedSaveData;
        
        public static bool IsLoadEnable()
        {
            if (!Directory.Exists(SaveFileDirectoryPath))
            {
                return false;
            }
            
            if (!File.Exists(SaveFilePath))
            {
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// TODO: Serialize 과정에서 렉이 발생할 수 있다. 비동기를 사용하는 것을 고려.
        /// </summary>
        public static void Save(SaveData saveData, bool isClear = true)
        {
            if(isClear)
                Clear();
            
            if (!Directory.Exists(SaveFileDirectoryPath))
                Directory.CreateDirectory(SaveFileDirectoryPath);
            
            try
            {
                var fileStream = File.Open(SaveFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                new BinaryFormatter().Serialize(fileStream, saveData);
                fileStream.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(SaveFileDirectoryPath);
                Debug.LogError(e);
                throw;
            }
        }
        
        public static SaveData Load()
        {
            Debug.Log(SaveFileDirectoryPath);
            
            if (IsAlreadyLoaded())
            {
                return _loadedSaveData;
            }
            
            if (!IsLoadEnable())
            {
                Debug.LogError($"{SaveFilePath}에 파일이 존재하지 않습니다.");
                return default;
            }

            try
            {
                var fileStream = File.Open(SaveFilePath, FileMode.Open);

                if (fileStream.Length <= 0)
                {
                    Debug.Log("Load 오류 발생");
                    fileStream.Close();
                    return default;
                }

                _loadedSaveData = (SaveData)new BinaryFormatter().Deserialize(fileStream);
                fileStream.Close();
            }
            catch (Exception e)
            {
                Clear();
                Debug.LogError(SaveFileDirectoryPath);
                Debug.LogError(e);
                throw;
            }

            return _loadedSaveData;
        }

        // public static SaveData LoadAsync()

        private static bool IsAlreadyLoaded()
        {
            return _loadedSaveData != null;
        }
        
        private static void Clear()
        {
            _loadedSaveData = null;
        }
    }
}