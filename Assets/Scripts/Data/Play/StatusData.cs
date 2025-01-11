using System;
using UnityEngine.Serialization;

namespace Data.Play
{
    /// <summary>
    /// 저장되는 데이터
    /// 근력, 지구력, 체력, 정신력, 레벨 및 exp
    /// 추가 체력, 추가 mp 등
    /// </summary>
    [Serializable]
    public struct StatusData
    {
        public int level;
        public int experiencePoint;
        
        public int vitality;
        public int spirit;
        public int strength;
        public int endurance;
        public int workmanship;
        public int intellect;
    }
}