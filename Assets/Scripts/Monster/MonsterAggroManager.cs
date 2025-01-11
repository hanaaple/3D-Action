using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monster
{
    public struct AggroObjectData
    {
        public GameObject aggroTarget;
        public float aggroAmount;
    }
    
    public class AggroAmountData
    {
        public float aggroAmount;
        public float time;
    }

    public enum AggroType
    {
        FirstLook,
        Look,
        Damage,
    }

    public class MonsterAggroManager : MonoBehaviour
    {
        public float aggroDistance;
        public float aggroDangerDistance;
        public float attackDistance;


        private Sight _sight;

        private GameObject _firstAggro;
        private GameObject _firstAttacked;

        private readonly Dictionary<GameObject, Dictionary<AggroType, AggroAmountData>> _aggroList = new();

        public event Action OnAggroUpdate;

        // stateMachine을 어떻게 변경시킬 것인가
        // 공격 당한 경우 -> 어그로 업데이트 
        // 어그로

        // 어그로에 따라 상태가 변해야 한다.
        // Hp에 따라 State가 변해야 한다.


        // 개체간 거리에 따른 어그로 변화 (공격 범위 내, 어그로 범위, 어그로 위험 거리 등)

        // 최초 발견 -> 생명력 * 최초 공격 목표 어그로 보정치
        // 최초 공격 -> 생명력 * 최초 공격자 어그로 보정치
        // 공격 -> 데미지 * (최초 공격자 보정 + 공격자 종류 보정 + default)

        private void Start()
        {
            _sight = GetComponent<Sight>();
        }

        private void Update()
        {
            foreach (var aggroData in _aggroList)
            {
                foreach (var aggroObjectData in aggroData.Value)
                {
                    if (Time.time - aggroObjectData.Value.time > 15f)
                    {
                        aggroObjectData.Value.aggroAmount =
                            Mathf.Clamp(aggroObjectData.Value.aggroAmount - Time.deltaTime, 0, float.MaxValue);
                    }
                }
            }
        }

        public void UpdateAggroRank()
        {
            // _aggroList.Sort((item1, item2) => item1.aggroAmount.CompareTo(item2.aggroAmount));

            // -> 어그로 수치에 따른 State 변화
            OnAggroUpdate?.Invoke();
        }
        
        public void CheckEnemy()
        {
            if (_sight.TryGetTargetGameObject(out var targets))
            {
                if (_firstAggro == null)
                {
                    _firstAggro = targets[0];
                }

                if (_firstAggro != null)
                    AddOrRenewAggroList(_firstAggro, AggroType.FirstLook);

                foreach (var target in targets)
                {
                    AddOrRenewAggroList(target, AggroType.Look);
                }
            }
        }

        private void AddOrRenewAggroList(GameObject aggroEntity, AggroType aggroType)
        {
            
            if (!_aggroList.TryGetValue(aggroEntity, out var aggroData))
            {
                aggroData = new Dictionary<AggroType, AggroAmountData>();
                _aggroList.Add(aggroEntity, aggroData);
            }
            
            var aggroAmount = GetAggroAmount(aggroType);

            if (!aggroData.TryGetValue(aggroType, out var aggroObjectData))
            {
                Debug.Log($"Add Aggro {aggroEntity}, {aggroType}");
                
                aggroObjectData = new AggroAmountData();
                aggroData.Add(aggroType, aggroObjectData);
            }
            else
            {
                // Debug.Log($"Renew Aggro {aggroEntity}, {aggroType}");
            }

            aggroObjectData.aggroAmount = aggroAmount;
            aggroObjectData.time = Time.time;

            UpdateAggroRank();
        }

        private void AddOrUpdateAggroList(GameObject aggroEntity, AggroType aggroType)
        {
            Debug.Log($"Add Aggro {aggroEntity}, {aggroType}");
            
            if (!_aggroList.TryGetValue(aggroEntity, out var aggroData))
            {
                aggroData = new Dictionary<AggroType, AggroAmountData>();
                _aggroList.Add(aggroEntity, aggroData);
            }
            
            var aggroAmount = GetAggroAmount(aggroType);


            if (!aggroData.TryGetValue(aggroType, out var aggroObjectData))
            {
                aggroObjectData = new AggroAmountData();
                aggroData.Add(aggroType, aggroObjectData);
            }
            
            aggroObjectData.aggroAmount += aggroAmount;
            
            aggroObjectData.time = Time.time;

            UpdateAggroRank();
        }
        
        
        
        private float GetAggroAmount(AggroType aggroType)
        {
            if (aggroType == AggroType.FirstLook)
            {
                return 10f;
            }
            else if (aggroType == AggroType.Damage)
            {
                return 20f;
            }
            else if (aggroType == AggroType.Look)
            {
                return 5f;
            }

            return 0f;
        }

        public AggroObjectData GetAggroTarget()
        {
            if (_aggroList.Count == 0)
            {
                return default;
            }
            else
            {
                return GetRandomAggroTarget();
            }
        }

        private AggroObjectData GetRandomAggroTarget()
        {
            var randomIndex = Random.Range(0, _aggroList.Count);

            var target = _aggroList.Keys.ElementAt(randomIndex);
            var aggroObjectData = new AggroObjectData
            {
                aggroTarget = target,
                aggroAmount = _aggroList[target].Values.Sum(item => item.aggroAmount)
            };

            return aggroObjectData;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, aggroDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroDangerDistance);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
}