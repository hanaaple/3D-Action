// 경직 당한 경우

// 강인도에 따라 

// 상태 (Idle, 포션 먹는 중)에 따라    그로기 수치에 따른 변화가 있다.

// Idle
// 0 ~ 5 -> 반응
// 5 ~ 20 -> 경직 (행동 불가)
// 20 ~ -> 넘어짐

// 포션 (먹기 시작했을 때부터, 손 드는 건 X)
// 0 ~ 10 -> 반응
// 10 ~ 20 -> 경직 (행동 불가)
// 20 ~ -> 넘어짐

// 넘어짐 조건 - 특수 (스킬, 보스, 잡기 등)

using System;
using BehaviourTreeGraph.Runtime;
using Character;
using Data;
using Monster.State.Base;
using Save;
using UnityEngine;
using Util;

namespace Monster
{
    
    public enum TargetMode
    {
        Area,
        CenterOfArea,
        AggroDistance, // 어그로 거리 (경계)
        AggroDangerDistance, // 어그로 위험 거리 (즉시 추적)
        AggroTarget,
        Patrol,
        NextPatrol
    }

    [Flags]
    public enum MonsterStateMode
    {
        Dead = 1,
        Idle = 2,
        Chase = 4,
        Alert = 8,
        Attack = 16,
        Return = 32,
    }

// 피격, 그로기, 넘어짐, 떨어짐, 뒤잡, 앞잡 등

    public class MonsterManager : BehaviourTreeRunner
    {
        [UniqueId] public string id;
        
        [SerializeField] private MonsterArea monsterArea;

        [SerializeField] private float attackCoolTime;
        [SerializeField] private float attackAggroAmount;
        [SerializeField] private float alertAggroAmount;

        [SerializeField] private GameObject[] attacks;
        
        public Transform lockOnTransform;
        
        private PatrolManager _patrolManager;
        private MonsterStateMachine _monsterStateMachine;
        private MonsterController _controller;
        private MonsterDataManager _monsterDataManager;
        private MonsterAggroManager _monsterAggroManager;
        
        private void Awake()
        {
            _monsterDataManager = GetComponent<MonsterDataManager>();
        }

        // public string GetLabel()
        // {
        // }
        
        public void GenerateNewId()
        {
            id = Guid.NewGuid().ToString();
        }


        public void Start()
        {
            var animationManager = GetComponent<CharacterAnimationManager>();
            _controller = GetComponent<MonsterController>();
            _patrolManager = GetComponent<PatrolManager>();
            _monsterAggroManager = GetComponent<MonsterAggroManager>();

            var monsterActionData = new MonsterActionData
            {
                AttackCoolTime = attackCoolTime
            };
            
            MonsterContext monsterContext = new MonsterContext();
            monsterContext.MonsterController = _controller;
            monsterContext.CharacterAnimationManager = animationManager;
            monsterContext.MonsterActionData = monsterActionData;
            monsterContext.MonsterManager = this;
            
            MonsterBlackBoard monsterBlackBoard = new MonsterBlackBoard();
            monsterBlackBoard.monsterManager = this;
            monsterBlackBoard.MonsterContext = monsterContext;

            _monsterStateMachine = new MonsterStateMachine();
            _monsterStateMachine.Initialize(monsterContext);

            _monsterAggroManager.OnAggroUpdate += () =>
            {
                if (_monsterStateMachine.GetCurrentState().Equals(MonsterStateMode.Idle))
                {
                    if (_monsterAggroManager.GetAggroTarget().aggroTarget == null) return;

                    if (_monsterAggroManager.GetAggroTarget().aggroAmount > attackAggroAmount)
                    {
                        _monsterStateMachine.ChangeState(MonsterStateMode.Chase);
                    }
                    else if (_monsterAggroManager.GetAggroTarget().aggroAmount > alertAggroAmount)
                    {
                        _monsterStateMachine.ChangeState(MonsterStateMode.Alert);
                    }
                }
                else
                {
                    if (_monsterAggroManager.GetAggroTarget().aggroTarget == null) return;

                    if (_monsterAggroManager.GetAggroTarget().aggroAmount < alertAggroAmount)
                    {
                        _monsterStateMachine.ChangeState(MonsterStateMode.Idle);
                    }
                }
            };

            InitializeBtTree(monsterBlackBoard);
        }

        protected override void Update()
        {
            base.Update();

            _monsterAggroManager.CheckEnemy();
            _monsterAggroManager.UpdateAggroRank();

            _monsterStateMachine.UpdateState();
        }

        private void LateUpdate()
        {
            _monsterStateMachine.LateUpdateState();
        }

        private void FixedUpdate()
        {
            _monsterStateMachine?.FixedUpdateState();
        }
        
        public bool IsIn(TargetMode targetMode)
        {
            if (targetMode == TargetMode.Area)
            {
                return monsterArea.IsInArea(transform.position);
            }
            else if (targetMode == TargetMode.CenterOfArea)
            {
                if (Vector3.Distance(transform.position, monsterArea.transform.position) >
                    _controller.GetStoppingDistance())
                {
                    return false;
                }

                return true;
            }
            else if (targetMode == TargetMode.AggroTarget)
            {
                var target = GetTarget(targetMode);
                var distance = Vector3.Distance(target.position, transform.position);
                if (distance <= _monsterAggroManager.attackDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (targetMode == TargetMode.AggroDistance)
            {
                if (Vector3.Distance(transform.position, GetAggroTarget().aggroTarget.transform.position) <=
                    _monsterAggroManager.aggroDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (targetMode == TargetMode.AggroDangerDistance)
            {
                if (Vector3.Distance(transform.position, GetAggroTarget().aggroTarget.transform.position) <=
                    _monsterAggroManager.aggroDangerDistance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (targetMode == TargetMode.Patrol || targetMode == TargetMode.NextPatrol)
            {
                var target = GetTarget(targetMode);
                var distance = Vector3.Distance(target.position, transform.position);

                // Debug.Log(distance);

                if (distance <= _controller.GetStoppingDistance())
                {
                    return true;
                }
            }

            return false;
        }

        public void ChangePatrolTarget()
        {
            _patrolManager.ChangePatrolIndex();
        }
        
        public void SetAttackEnable(int attackType, bool enable)
        {
            attacks[attackType].SetActive(enable);
        }

        public Transform GetTarget(TargetMode targetMode)
        {
            Transform target = null;
            switch (targetMode)
            {
                case TargetMode.AggroTarget:
                    target = GetAggroTarget().aggroTarget.transform;
                    break;
                case TargetMode.CenterOfArea:
                case TargetMode.Area:
                    target = monsterArea.transform;
                    break;
                case TargetMode.Patrol:
                    target = _patrolManager.GetPatrolTransform();
                    break;
                case TargetMode.NextPatrol:
                    target = _patrolManager.GetNextPatrolTransform();
                    break;
            }

            return target;
        }

        public AggroObjectData GetAggroTarget()
        {
            return _monsterAggroManager.GetAggroTarget();
        }

        public void MoveTo(TargetMode targetMode, bool isRun)
        {
            var target = GetTarget(targetMode);
            _controller.Move(target.position, isRun);
        }

        public void ChangeState(MonsterStateMode monsterState)
        {
            _monsterStateMachine.ChangeState(monsterState);
        }

        public bool StateChangeEnable(MonsterStateMode monsterState)
        {
            return _monsterStateMachine.ChangeStateEnable(monsterState);
        }

        public MonsterStateMode GetState()
        {
            return _monsterStateMachine.GetCurrentState() is MonsterStateMode
                ? (MonsterStateMode)_monsterStateMachine.GetCurrentState()
                : 0;
        }
    }
}