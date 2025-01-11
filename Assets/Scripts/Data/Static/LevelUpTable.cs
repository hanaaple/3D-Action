using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Data.Static
{
    /// <summary>
    /// 레벨 별 필요한 경험치 테이블
    /// </summary>
    public class LevelUpTable
    {
        // Level, RequiredExp per level
        private readonly Dictionary<int, int> _levelUpTable = new ();
        
        public LevelUpTable(string csvText)
        {
            var list = CsvReader.ReadList<LevelUpData>(csvText);
            foreach (var levelUpData in list)
            {
                _levelUpTable.TryAdd(levelUpData.Level, levelUpData.RequiredExp);
            }
        }
        
        public bool CanLevelUp(int level)
        {
            return _levelUpTable.ContainsKey(level);
        }

        public int GetRequiredExp(int level)
        {
            if(_levelUpTable.TryGetValue(level, out int requiredLevel))
            {
                return requiredLevel;
            }
            
            Debug.LogWarning("레벨업 불가능합니다.");

            return -1;
        }

    }

    public class LevelUpData
    {
        public int Level;
        public int RequiredExp;
    }
}