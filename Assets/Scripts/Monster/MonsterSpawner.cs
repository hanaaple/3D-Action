using UnityEngine;

namespace Monster
{
    public enum MonsterType
    {
        Mob,
        Elite,
        Boss
    }
    public abstract class MonsterSpawner : MonoBehaviour
    {
        // 세이브에 따른 몬스터 인스턴스화
        
        // 잡몹 -> 맵 변경 시에만 생성
        // 엘리트 -> 1회만
        // 보스 -> 1회만
        
        // MonsterPrefab _monsterPrefab
        // MonsterType

        [SerializeField] private MonsterType monsterType;
        
        // public Reward reward;
    }
}