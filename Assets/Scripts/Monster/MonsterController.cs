using Character;
using UnityEngine;
using UnityEngine.AI;

namespace Monster
{
    public class MonsterController : MonoBehaviour
    {
        // 기본 설정값 Field
        [Header("Move")] [SerializeField] private float walkSpeed = 2.0f; // 실제 움직이는 속도 및 애니메이션 속도
        [SerializeField] private float runSpeed = 6f; // 실제 움직이는 속도 및 애니메이션 속도
        
        [SerializeField] private float navMeshCornerRadius;

        [Header("For Debug")]
        public Transform lookAtTarget;
        
        // Ground
        private bool _isRunning;
        internal Quaternion DesiredRotation;

        private Animator _animator;
        private CharacterAnimationManager _characterAnimationManager;
        private NavMeshAgent _navMeshAgent;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            _characterAnimationManager = GetComponent<CharacterAnimationManager>();
        }

        // Gravity, GroundCheck 등을 Custom 하게 되는 경우 StateUpdate으로 위치 변경
        private void Update()
        {
            UpdateSpeed();
        }

        private void UpdateSpeed()
        {
            _navMeshAgent.speed = _isRunning ? runSpeed : walkSpeed;

            _characterAnimationManager.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
        }

        public bool TryTurn(Vector3 targetPosition)
        {
            if (_characterAnimationManager.isBusy)
            {
                return true;
            }

            var from = _animator.transform.forward;
            from.y = 0;

            // var path = new NavMeshPath();
            // _navMeshAgent.CalculatePath(targetPosition, path);
            // _navMeshAgent.

            var to = targetPosition - _animator.transform.position;
            to.y = 0;

            var angle = Vector3.SignedAngle(from, to, Vector3.up);

            if (Mathf.Abs(angle) <= 15f)
                return false;

            // Debug.LogError(angle);

            angle = Mathf.Clamp(angle, -90, 90);


            _characterAnimationManager.isBusy = true;

            // _characterAnimationManager.isBusy == true -> navMeshAgent.enable = false;
            // _characterAnimationManager.isBusy == false -> navMeshAgent.enable = true;

            if (angle > 0) _characterAnimationManager.SetFloat("TurnType", 0);
            else _characterAnimationManager.SetFloat("TurnType", 1);

            _characterAnimationManager.SetFloat("TurnAngle", angle);
            _characterAnimationManager.SetTrigger("Turn");

            _animator.applyRootMotion = true;

            DesiredRotation = Quaternion.Euler(_animator.rootRotation.eulerAngles + new Vector3(0, angle, 0));

            return true;
        }

        private float GetMoveSpeedClamp01()
        {
            if (_isRunning)
            {
                return _navMeshAgent.velocity.magnitude / runSpeed;
            }
            else
            {
                return _navMeshAgent.velocity.magnitude / walkSpeed;
            }
        }

        public void Teleport(Transform targetTransform)
        {
            transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
            _animator.rootRotation = Quaternion.identity;

            InitMoveState();
        }

        private void InitMoveState()
        {
            _characterAnimationManager.SetFloat("Speed", 0);
        }

        public void Move(Vector3 targetPosition, bool isRun)
        {
            if (!_characterAnimationManager.isBusy)
            {
                _isRunning = isRun;
                MoveStart(targetPosition);
            }
            else
            {
                Stop();
            }
        }

        public float GetRemainingDistance()
        {
            var remainingDistance = _navMeshAgent.remainingDistance - _navMeshAgent.stoppingDistance;
            return remainingDistance;
        }

        /// <summary>
        /// just calculate distance, not store path
        /// </summary>
        public float GetRemainingDistance(Vector3 targetPosition)
        {
            var oldPath = _navMeshAgent.path;
            var path = new NavMeshPath();
            var bo = _navMeshAgent.CalculatePath(targetPosition, path);
            var remainingDistance = _navMeshAgent.remainingDistance - _navMeshAgent.stoppingDistance;
            Debug.Log(
                $"{bo},   {path.status}      {_navMeshAgent.remainingDistance},   {_navMeshAgent.stoppingDistance}");
            _navMeshAgent.ResetPath();
            _navMeshAgent.SetPath(oldPath);
            return remainingDistance;
        }

        private void MoveStart(Vector3 targetPosition)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.updatePosition = true;
            _navMeshAgent.updateRotation = true;
            _navMeshAgent.SetDestination(targetPosition);
        }

        public void Stop()
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity = Vector3.zero;
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = false;
        }

        public void StopInPlace()
        {
            _navMeshAgent.SetDestination(transform.position + transform.forward * (_navMeshAgent.stoppingDistance * 0.7f));
        }

        public bool IsStopped()
        {
            return _navMeshAgent.velocity.magnitude < 1f;
        }

        public float GetStoppingDistance()
        {
            return _navMeshAgent.stoppingDistance;
        }

        // public void InitializeNav()
        // {
        //     _navMeshAgent.isStopped = false;
        //     _navMeshAgent.ResetPath();
        //     _navMeshAgent.updatePosition = true;
        //     _navMeshAgent.updateRotation = true;
        // }

        // public void SetDestinationTarget(Transform moveTarget)
        // {
        //     InitializeNav();
        //     _navMeshAgent.SetDestination(moveTarget.position);
        // }
        private void OnDrawGizmos()
        {
            if (_navMeshAgent != null && _navMeshAgent.path != null && _navMeshAgent.path.status == NavMeshPathStatus.PathComplete)
            {
                Gizmos.color = Color.red;
                
                for (var index = 0; index < _navMeshAgent.path.corners.Length - 1; index++)
                {
                    var cornerA = _navMeshAgent.path.corners[index];
                    var cornerB = _navMeshAgent.path.corners[index + 1];
                    
                    Gizmos.DrawLine(cornerA, cornerB);
                }

                foreach (var cornerA in _navMeshAgent.path.corners)
                {
                    Gizmos.DrawSphere(cornerA, navMeshCornerRadius);
                }
            }
        }
    }
}