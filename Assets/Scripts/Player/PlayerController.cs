using System;
using Cinemachine;
using Data.ViewModel;
using Manager;
using Monster;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(CharacterController), typeof(InputStateHandler))]
    public class PlayerController : MonoBehaviour
    {
        // 스탯 or 무게에 따라 변화하고 싶으면 모션 스피드, 이동 속도를 변화시켜야된다.

        // 기본 설정값 Field
        [FormerlySerializedAs("moveSpeed")] [Header("Move")] [SerializeField]
        private float walkSpeed = 2.0f; // 실제 움직이는 속도 및 애니메이션 속도
        [SerializeField] private float runSpeed = 5.335f; // 실제 움직이는 속도 및 애니메이션 속도
        
        
        [Header("스태미나")]
        [SerializeField] private float staminaDecreaseSpeed = 3f;
        [SerializeField] private float staminaIncreaseSpeed = 3f;
        [SerializeField] private float poiseIncreaseSpeed = 1f;
        [SerializeField] private float runTiredSecond = 2f;
        [SerializeField] private float poiseTimer = 5f;
        [SerializeField] private float runEnablePoint = 3f;
        public float rollStaminaUseAmount = 3f;
        
        
        [Range(0.0f, 0.3f)] [SerializeField] private float rotationSmoothTime = 0.12f;
        [SerializeField] private float speedChangeRate = 10.0f;

        [Header("Roll")] [SerializeField] private float rollMotionSpeed = 1f;
        [SerializeField] private float rollThreshold = 0f;

        [Header("Jump")] public float jumpHeight = 1.2f;
        [SerializeField] private float jumpThreshold = 0.2f;

        [Header("Gravity")] public float gravity = -15.0f;
        [SerializeField] private float fallThreshold = 0.15f;

        [Header("Player Grounded")] [SerializeField]
        private LayerMask groundLayers;

        [Header("Cinemachine Camera")] [SerializeField] private Transform cinemachineCameraTarget;
        [SerializeField] private float topClamp = 70.0f;
        [SerializeField] private float bottomClamp = -30.0f;
        [Range(0.1f, 20)] [SerializeField] private float xAxisSensitivity = 1f;
        [Range(0.1f, 20)] [SerializeField] private float yAxisSensitivity = 1f;
        [SerializeField] private float cameraAngleOverride = 0f;

        [Header("Lock")]
        [Range(0, 1)] [SerializeField] private float trackingWidth;
        [Range(0, 1)] [SerializeField] private float trackingHeight;
        [SerializeField] private float lockRotationSpeed = 1f;
        
        // Lock
        private GameObject _lockOnPoint;
        
        // cinemachine Camera
        private float _cameraTargetYAxis;
        private float _cameraTargetXAxis;

        // Move
        private bool _isRunning;
        internal bool IsTired;
        
        private float _rotationVelocity;
        private float _animationBlend;

        private Vector2 _moveDirection;
        private Vector2 _moveVelocity;
        private const float MoveSmoothTime = 0.12f;

        // 스태미나
        private float _tiredTime;
        private float _poisedTime;
        private bool _isCompletelyPoised;
        private float _tiredTimer;
        
        /// <summary>
        /// 현재 캐릭터 Forward 각도
        /// </summary>
        private float _targetRotation;

        internal float VerticalVelocity;
        internal float MoveSpeed;

        // Time after landing when the action cannot be performed
        internal float JumpTimeoutDelta;
        internal float RollTimeoutDelta;

        // Time taken to perform falling action
        private float _fallTimeoutDelta;

        // Ground
        internal bool IsGrounded;
        
        // LockOn
        [SerializeField] private Transform _lockOnTarget;

        private CharacterController _characterController;
        private InputStateHandler _inputStateHandler;
        private PlayerSight _sight;
        private Camera _mainCamera;
        internal Animator animator;
        internal RuntimeAnimatorController OriginalAnimatorController;
        
        private const float MouseMovementThreshold = 0.01f;

        private static readonly int AnimIdSpeed = Animator.StringToHash("Speed");
        private static readonly int AnimIdGrounded = Animator.StringToHash("Grounded");
        private static readonly int AnimIdRollMotionSpeed = Animator.StringToHash("RollMotionSpeed");
        private static readonly int AnimIdFall = Animator.StringToHash("FreeFall");
        private static readonly int AnimIdMoveDirectionX = Animator.StringToHash("MoveDirection X");
        private static readonly int AnimIdMoveDirectionZ = Animator.StringToHash("MoveDirection Z");
        private static readonly int Turn = Animator.StringToHash("Turn");

        public void Initialize(GameObject lockOnTarget, CinemachineVirtualCamera virtualCamera)
        {
            _lockOnPoint = lockOnTarget;
            virtualCamera.Follow = cinemachineCameraTarget;
        }
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            _inputStateHandler = GetComponent<InputStateHandler>();
            _sight = GetComponentInChildren<PlayerSight>();
            if (Camera.main != null) _mainCamera = Camera.main;

            animator.SetFloat(AnimIdRollMotionSpeed, rollMotionSpeed);

            _fallTimeoutDelta = fallThreshold;
            JumpTimeoutDelta = jumpThreshold;
            RollTimeoutDelta = rollThreshold;

            // 만약 캐릭터가 순간이동한다면 업데이트 해줘야됨
            _targetRotation = transform.eulerAngles.y;

            OriginalAnimatorController = animator.runtimeAnimatorController;


            var playerDataViewModel = PlaySceneManager.instance.playerDataManager.playerDataViewModel;
            
            playerDataViewModel.PropertyChanged += (_, _) =>
            {
                // if (playerDataViewModel.StaminaPoint <= 0)
                {
                    Debug.Log($"Tired!   {Time.time}");
                    _tiredTime = Time.time;
                    IsTired = true;
                }
            };
            
            playerDataViewModel.PropertyChanged += (_, _) =>
            {
                if (playerDataViewModel.PoiseHealthPoint <= 0)
                {
                    _poisedTime = Time.time;
                }
            };
        }

        // Gravity, GroundCheck 등을 Custom 하게 되는 경우 StateUpdate으로 위치 변경
        private void FixedUpdate()
        {
            TryLockByInput();
            UpdateLockOnUI();
            Gravity();
            GroundedCheck();

            // 공격 시, 달리기, 구르기 할 때마다 줄어듬
            // 위 상태에서는 안늘어남
            
            RecoveryStamina();
            RecoveryPoise();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void RecoveryStamina()
        {
            // 달리기, 구르기, 공격
            
            // 구르는 도중에는 --> 구른 이후 타이머로 안되게 만들기
            //               --> 스태미나 전부 사용 이후 타이머로 안되게 만들기
            //               --> 공격 시 일정 시간 동안 타이머로 안되게 만들기
            //               --> 
            
            // 스태미나 사용하는 능력을 사용
            
            // 포션, 아이템 등 사용 중인 경우 스태미나 회복 속도가 일시감소
            
            
            // 공격 시, 달리기, 구르기 할 때마다 줄어듬
            // 위 상태에서는 안늘어남
            if (_isRunning)
                return;
            
            // if (staminaTimer < Time.time - _tiredTime)
            {
                // DataManager.instance.playerDataViewModel.StaminaPoint += DataManager.instance.playerDataViewModel.StaminaRecoveryWeight * staminaIncreaseSpeed * Time.fixedDeltaTime;   
            }
        }
        
        private void RecoveryPoise()
        {
            // 공격 시, 달리기, 구르기 할 때마다 줄어듬
            // 위 상태에서는 안늘어남
            
            // 스태미너가 0이 되면 일정 시간 동안 스태미너가 안참
            
            if (poiseTimer < Time.time - _poisedTime)
            {
                if (_isCompletelyPoised)
                {
                    // 완전히 자세가 무너진 경우
                    // 바로 회복
                    
                    // 애니메이션?
                    // 작동?
                    
                    var playerDataViewModel = PlaySceneManager.instance.playerDataManager.playerDataViewModel;
                    playerDataViewModel.PoiseHealthPoint = playerDataViewModel.MaxPoiseHealthPoint;
                }
                else
                {
                    var playerDataViewModel = PlaySceneManager.instance.playerDataManager.playerDataViewModel;
                    playerDataViewModel.PoiseHealthPoint += playerDataViewModel.PoiseHealthRecoveryWeight * poiseIncreaseSpeed * Time.fixedDeltaTime;
                }
            }
        }
        
        private void GroundedCheck()
        {
            // set sphere position, with offset
            var spherePosition = new Vector3(transform.position.x + _characterController.center.x,
                transform.position.y + _characterController.center.y - (_characterController.height / 2) -
                _characterController.radius,
                transform.position.z + _characterController.center.z);
            IsGrounded = Physics.CheckSphere(spherePosition, _characterController.radius, groundLayers,
                QueryTriggerInteraction.Ignore);

            animator?.SetBool(AnimIdGrounded, IsGrounded);
        }

        private void Gravity()
        {
            if (IsGrounded)
            {
                if (VerticalVelocity < 0.0f)
                {
                    VerticalVelocity = -2f;
                }

                // reset the timeout timer
                _fallTimeoutDelta = fallThreshold;

                animator.SetBool(AnimIdFall, false);

                if (RollTimeoutDelta >= 0.0f)
                {
                    RollTimeoutDelta -= Time.fixedDeltaTime;
                }

                if (JumpTimeoutDelta >= 0.0f)
                {
                    JumpTimeoutDelta -= Time.fixedDeltaTime;
                }
            }
            else
            {
                VerticalVelocity += gravity * Time.fixedDeltaTime;

                // reset the timeout timer
                RollTimeoutDelta = rollThreshold;
                JumpTimeoutDelta = jumpThreshold;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.fixedDeltaTime;
                }
                else
                {
                    animator?.SetBool(AnimIdFall, true);
                }
            }
        }

        private void CameraRotation()
        {
            if (_lockOnTarget != null)
            {
                Vector3 viewportPosition = _mainCamera.WorldToViewportPoint(_lockOnTarget.position);

                // Target이 카메라로부터 얼마나 멀리 있는가
                var distanceX = Mathf.Clamp(Mathf.Abs((viewportPosition.x - .5f) * 2f), trackingWidth, 1) - trackingWidth;
                var distanceY = Mathf.Clamp(Mathf.Abs((viewportPosition.y - .5f) * 2f), trackingHeight, 1) - trackingHeight;
                
                if (distanceX > float.Epsilon || distanceY > float.Epsilon)
                {
                    // 트래킹 범위를 벗어난 경우
                    Vector3 direction = _lockOnTarget.position - cinemachineCameraTarget.position;
                    var targetRotation = Quaternion.LookRotation(direction);
                    
                    targetRotation = Quaternion.Slerp(Quaternion.Euler(_cameraTargetXAxis, _cameraTargetYAxis, 0.0f), targetRotation, lockRotationSpeed * Time.deltaTime);

                    var targetEulerAngle = targetRotation.eulerAngles;
                    targetEulerAngle = ClampAngleHalf(targetEulerAngle);

                    // Debug.Log(targetEulerAngle);

                    _cameraTargetYAxis = targetEulerAngle.y;
                    _cameraTargetXAxis = targetEulerAngle.x;
                }
                
                if (_inputStateHandler.look.sqrMagnitude >= MouseMovementThreshold)
                {
                    const float deltaTimeMultiplier = 1.0f;
                    _cameraTargetYAxis += _inputStateHandler.look.x * deltaTimeMultiplier * yAxisSensitivity;
                    _cameraTargetXAxis += _inputStateHandler.look.y * deltaTimeMultiplier * xAxisSensitivity;
                }
            }
            // if there is an input and camera position is not fixed
            else if (_inputStateHandler.look.sqrMagnitude >= MouseMovementThreshold)
            {
                //Don't multiply mouse input by Time.deltaTime;
                const float deltaTimeMultiplier = 1.0f;
                // float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cameraTargetYAxis += _inputStateHandler.look.x * deltaTimeMultiplier * yAxisSensitivity;
                _cameraTargetXAxis += _inputStateHandler.look.y * deltaTimeMultiplier * xAxisSensitivity;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cameraTargetYAxis = ClampAngle(_cameraTargetYAxis, float.MinValue, float.MaxValue);
            _cameraTargetXAxis = ClampAngle(_cameraTargetXAxis, bottomClamp, topClamp);
            
            // Cinemachine will follow this target  
            cinemachineCameraTarget.rotation = Quaternion.Euler(_cameraTargetXAxis + cameraAngleOverride, _cameraTargetYAxis, 0.0f);
        }

        private void TryLockByInput()
        {
            if(!_inputStateHandler.lockOn) return;
            
            _inputStateHandler.lockOn = false;

            if (_lockOnTarget)
            {
                LockOn(false);
            }
            else
            {
                LockOn(true);
            }
        }

        private void UpdateLockOnUI()
        {
            if (_lockOnPoint.activeSelf)
            {
                // 도중에 다른 거에 의해 Target이 죽거나 사라진 경우
                if (_lockOnTarget == null)
                {
                    LockOn(false);
                }
                else
                {
                    _lockOnPoint.transform.position = _mainCamera.WorldToScreenPoint(_lockOnTarget.position);
                }
            }
        }

        private void LockOn(bool isLock)
        {
            if (isLock)
            {
                if (_sight.TryGetNearestTargetInSight(out var target))
                {
                    Debug.Log("Lock On");
                    _lockOnPoint.SetActive(true);

                    if (target.TryGetComponent<MonsterManager>(out var monster) && monster.lockOnTransform != null)
                    {
                        _lockOnTarget = monster.lockOnTransform;
                    }
                    else
                    {
                        _lockOnTarget = target.transform;
                    }

                    _lockOnPoint.transform.position = _mainCamera.WorldToScreenPoint(_lockOnTarget.position);
                }
            }
            else
            {
                Debug.Log("Lock Off");

                _lockOnPoint.SetActive(false);
                _lockOnTarget = null;
            }
        }
        
        private void UpdateAnimationSpeed(float tick)
        {
            float targetSpeed = _isRunning ? runSpeed : walkSpeed;

            // Vector == operation uses approximation so it is not error prone.
            if (_inputStateHandler.move == Vector2.zero) targetSpeed = 0.0f;

            // float inputMagnitude = _inputStateHandler.analogMovement ? _inputStateHandler.move.magnitude : 1f;

            // Set BlendTree Speed
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, tick * speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // Set Animation Parameters
            animator.SetFloat(AnimIdSpeed, _animationBlend);
        }

        private void UpdateSpeed(float tick)
        {
            float targetSpeed = _isRunning ? runSpeed : walkSpeed;

            // Vector == operation uses approximation so it is not error prone.
            if (_inputStateHandler.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed =
                new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _inputStateHandler.analogMovement ? _inputStateHandler.move.magnitude : 1f;

            if (Mathf.Abs(currentHorizontalSpeed - targetSpeed) > speedOffset)
            {
                MoveSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    tick * speedChangeRate);

                MoveSpeed = Mathf.Round(MoveSpeed * 1000f) / 1000f;
            }
            else
            {
                MoveSpeed = targetSpeed;
            }

            Vector2 moveTarget;
            if (_inputStateHandler.move != Vector2.zero)
            {
                if (_lockOnTarget != null && !_isRunning)
                {
                    moveTarget = new Vector2(_inputStateHandler.move.x, _inputStateHandler.move.y);
                    if (Mathf.Abs(moveTarget.x) > 0.01f && Mathf.Abs(moveTarget.y) > 0.01f)
                        moveTarget *= Mathf.Sqrt(2);
                }
                else
                {
                    moveTarget = new Vector2(0, 1);
                }
            }
            else
            {
                moveTarget = Vector2.zero;
            }

            _moveDirection.x = Mathf.SmoothDamp(_moveDirection.x, moveTarget.x, ref _moveVelocity.x, MoveSmoothTime);
            _moveDirection.y = Mathf.SmoothDamp(_moveDirection.y, moveTarget.y, ref _moveVelocity.y, MoveSmoothTime);

            if (Mathf.Abs(_moveDirection.x) < 0.01f)
                _moveDirection.x = 0;

            if (Mathf.Abs(_moveDirection.y) < 0.01f)
                _moveDirection.y = 0;

            animator.SetFloat(AnimIdMoveDirectionX, _moveDirection.x);
            animator.SetFloat(AnimIdMoveDirectionZ, _moveDirection.y);
        }

        public void Translate(float tick)
        {
            // 기존의 이동방향
            var targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // Translate position
            _characterController.Move(targetDirection.normalized * (MoveSpeed * tick) +
                                      new Vector3(0.0f, VerticalVelocity, 0.0f) * tick);
        }

        public void RotateImmediately()
        {
            // without rotation, velocity converges to 0.
            // _targetRotation = 키 입력 방향
            // TODO
            _rotationVelocity = 0f;
            transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
        }

        public void Rotate()
        {
            if (_inputStateHandler.move == Vector2.zero)
                return;

            var inputDirection = new Vector3(_inputStateHandler.move.x, 0.0f, _inputStateHandler.move.y).normalized;
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;

            float targetRotation;
            if (_lockOnTarget != null && !_isRunning)
            {
                var targetPosition = _lockOnTarget.position;
                targetPosition.y = transform.position.y;

                targetRotation = Quaternion.LookRotation(targetPosition - transform.position).eulerAngles.y;
            }
            else
            {
                targetRotation = _targetRotation;
            }

            // Mathf.SmoothDampAngle() ensures interpolation
            var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationVelocity,
                rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        public void SetStaminaTimer(float timerSecond)
        {
        }

        private bool TryTurn()
        {
            var targetAngleY = GetTargetRotation();
            var targetRotation = new Vector3(0, targetAngleY, 0);

            float turnAngle = Mathf.Abs(Vector3.SignedAngle(transform.forward, targetRotation, Vector3.up));

            if (turnAngle >= 165f && !_inputStateHandler.crouch)
            {
                Debug.Log("Turn !");
                var desiredRotation = animator.rootRotation.eulerAngles + new Vector3(0, turnAngle, 0);
                animator.applyRootMotion = true;
                animator.SetTrigger(Turn);
                // on end ->
                // Animator.rootRotation = desiredRotation
                // Animator.applyRootMotion = false;

                return true;
            }

            return false;
        }

        public void Teleport(Transform targetTransform)
        {
            transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);

            InitMoveState();
        }

        public void InitMoveState()
        {
            MoveSpeed = 0;
            _rotationVelocity = 0;
            _animationBlend = 0;
            animator.SetFloat(AnimIdSpeed, 0);
            animator.SetFloat(AnimIdMoveDirectionX, 0);
            animator.SetFloat(AnimIdMoveDirectionZ, 0);
        }

        public void Move(float tick, bool isRun)
        {
            CheckRun(isRun);
            
            UpdateAnimationSpeed(tick);
            UpdateSpeed(tick);

            // if (TryTurn())
            // {
            // }
            // else
            // {
            //     Rotate();
            // }
            Rotate();

            
            Translate(tick);
        }

        private void CheckRun(bool isRun)
        {
            var playerDataViewModel = PlaySceneManager.instance.playerDataManager.playerDataViewModel;
            
            // 처음 누른 경우, 계속 누른 경우 구분을 위해 사용
            // TODO: 스태미나 관련 구현
            
            if (isRun 
                // && playerDataViewModel.StaminaPoint > 0 && !IsTired
                )
            {
                _isRunning = true;
                playerDataViewModel.DecreaseStaminaPoint(staminaDecreaseSpeed * Time.fixedDeltaTime, StaminaUseType.Run);
                // playerDataViewModel.StaminaPoint -= staminaDecreaseSpeed * Time.fixedDeltaTime;
            }
            else
            {
                // 스태미나 사용 시 사용된 종류(run, roll) 등에 따라 다른 타이머로 작동.
                
                // TODO: 스태미나 매커니즘 변경, 
                // if (IsTired && playerDataViewModel.StaminaPoint > runEnablePoint)
                // {
                //     IsTired = false;
                // }
                _isRunning = false;
            }
        }
        
        private float GetTargetRotation()
        {
            var inputDirection = new Vector3(_inputStateHandler.move.x, 0.0f, _inputStateHandler.move.y).normalized;
            return Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                   _mainCamera.transform.eulerAngles.y;
        }

        public bool TryGetInput(Enum stateMode)
        {
            return _inputStateHandler.TryRemove(stateMode);
        }

        public float GetMoveSpeed()
        {
            return MoveSpeed;
        }

        private static float ClampAngle(float value, float min, float max)
        {
            if (value <= -360f) value += 360f;
            if (value >= 360f) value -= 360f;
            return Mathf.Clamp(value, min, max);
        }
        
        private static Vector3 ClampAngleHalf(Vector3 vector)
        {
            while (Mathf.Abs(vector.x) > 180f)
            {
                vector.x += vector.x > 0 ? -360 : 360;
            }
            
            while (Mathf.Abs(vector.y) > 180f)
            {
                vector.y += vector.y > 0 ? -360 : 360;
            }
            
            while (Mathf.Abs(vector.z) > 180f)
            {
                vector.z += vector.z > 0 ? -360 : 360;
            }

            return vector;
        }
    }
}