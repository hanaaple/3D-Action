using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Util
{
    public static class SaveManager
    {
        private static readonly string SaveFileDirectoryPath = $"{Application.persistentDataPath}/SaveData";
        private static readonly string SaveFilePath = $"{SaveFileDirectoryPath}/saveData.save";

        public static bool IsLoadEnable()
        {
            if (!File.Exists(SaveFilePath))
            {
                return false;
            }
            
            if (!Directory.Exists(SaveFileDirectoryPath))
            {
                return false;
            }
            
            return true;
        }
        
        public static void Save(SaveData saveData)
        {
            if (!Directory.Exists(SaveFileDirectoryPath))
            {
                Directory.CreateDirectory(SaveFileDirectoryPath);
            }
            
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
            
            SaveData saveData;
            
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

                saveData = (SaveData)new BinaryFormatter().Deserialize(fileStream);

                fileStream.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(SaveFileDirectoryPath);
                Debug.LogError(e);
                throw;
            }

            return saveData;
        }
    }
}