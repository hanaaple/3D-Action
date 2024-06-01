using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class LevelUpTable
    {
        public Dictionary<int, int> levelUpTable = new ();
        
        public bool CanLevelUp(int level)
        {
            return levelUpTable.ContainsKey(level);
        }

        public int GetRequiredExp(int level)
        {
            if(levelUpTable.TryGetValue(level, out int requiredLevel))
            {
                return requiredLevel;
            }
            
            Debug.LogWarning("레벨업 불가능합니다.");

            return -1;
        }
    }
}