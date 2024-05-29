using System;
using CharacterControl.State;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CharacterControl
{
    [RequireComponent(typeof(CharacterController), typeof(Animator), typeof(InputStateHandler))]
    public class ThirdPlayerController : MonoBehaviour
    {
        // 스탯 or 무게에 따라 변화하고 싶으면 모션 스피드, 이동 속도를 변화시켜야된다.

        // 기본 설정값 Field
        [Header("Move")]
        [SerializeField] private float moveSpeed = 2.0f; // 실제 움직이는 속도 및 애니메이션 속도
        [SerializeField] private float runSpeed = 5.335f; // 실제 움직이는 속도 및 애니메이션 속도
        [Range(0.0f, 0.3f)] [SerializeField] private float rotationSmoothTime = 0.12f;
        [SerializeField] private float speedChangeRate = 10.0f;

        [Header("Audio")]
        [SerializeField] private AudioClip rollingAudioClip;
        [SerializeField] private AudioClip landingAudioClip;
        [SerializeField] private AudioClip[] footstepAudioClips;
        [Range(0, 1)] [SerializeField] private float footstepAudioVolume = 0.5f;
        [Range(0, 1)] [SerializeField] private float rollingAudioVolume = 0.5f;
        [Range(0, 1)] [SerializeField] private float landingAudioVolume = 0.5f;

        [Header("Roll")]
        public float rollSpeed = 5f;
        [SerializeField] private float rollMotionSpeed = 1f;
        [SerializeField] private float rollThreshold = 0f;

        [Header("Jump")]
        public float jumpHeight = 1.2f;
        [SerializeField] private float jumpThreshold = 0.2f;

        [Header("Gravity")]
        public float gravity = -15.0f;
        [SerializeField] private float fallThreshold = 0.15f;

        [Header("Player Grounded")]
        [SerializeField] private LayerMask groundLayers;

        [Header("Cinemachine")]
        [SerializeField] private GameObject cinemachineCameraTarget;
        [SerializeField] private float topClamp = 70.0f;
        [SerializeField] private float bottomClamp = -30.0f;
        [Range(0.1f, 20)] [SerializeField] private float xAxisSensitivity = 1f;
        [Range(0.1f, 20)] [SerializeField] private float yAxisSensitivity = 1f;
        [SerializeField] private float cameraAngleOverride = 0f;
        [SerializeField] private bool lockCameraPosition = false;

        // cinemachine
        private float _cameraTargetYAxis;
        private float _cameraTargetXAxis;

        // Move
        private float _rotationVelocity;
        private float _animationBlend;
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

        private CharacterController _characterController;
        private InputStateHandler _inputStateHandler;
        private GameObject _mainCamera;
        internal Animator Animator;

        private const float MouseMovementThreshold = 0.01f;

        private readonly int _animIdSpeed = Animator.StringToHash("Speed");
        private readonly int _animIdGrounded = Animator.StringToHash("Grounded");
        private readonly int _animIdRollMotionSpeed = Animator.StringToHash("RollMotionSpeed");
        private readonly int _animIdFall = Animator.StringToHash("FreeFall");
        private readonly int _animIdMotionSpeed = Animator.StringToHash("RunMotionSpeed");

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            Animator = GetComponent<Animator>();
            _inputStateHandler = GetComponent<InputStateHandler>();
            if (Camera.main != null) _mainCamera = Camera.main.gameObject;

            Animator.SetFloat(_animIdRollMotionSpeed, rollMotionSpeed);

            _fallTimeoutDelta = fallThreshold;
            JumpTimeoutDelta = jumpThreshold;
            RollTimeoutDelta = rollThreshold;

            // 만약 캐릭터가 순간이동한다면 업데이트 해줘야됨
            _targetRotation = transform.eulerAngles.y;
        }

        // Gravity, GroundCheck 등을 Custom 하게 되는 경우 StateUpdate으로 위치 변경
        private void Update()
        {
            UpdateAnimationSpeed();
            Gravity();
            GroundedCheck();
        }

        private void LateUpdate()
        {
            CameraRotation();
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

            Animator?.SetBool(_animIdGrounded, IsGrounded);
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

                Animator.SetBool(_animIdFall, false);

                if (RollTimeoutDelta >= 0.0f)
                {
                    RollTimeoutDelta -= Time.deltaTime;
                }

                if (JumpTimeoutDelta >= 0.0f)
                {
                    JumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                VerticalVelocity += gravity * Time.deltaTime;

                // reset the timeout timer
                RollTimeoutDelta = rollThreshold;
                JumpTimeoutDelta = jumpThreshold;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    Animator?.SetBool(_animIdFall, true);
                }
            }
        }

        private void UpdateAnimationSpeed()
        {
            float targetSpeed = _inputStateHandler.run ? runSpeed : moveSpeed;

            // Vector == operation uses approximation so it is not error prone.
            if (_inputStateHandler.move == Vector2.zero) targetSpeed = 0.0f;

            float inputMagnitude = _inputStateHandler.analogMovement ? _inputStateHandler.move.magnitude : 1f;

            // Set BlendTree Speed
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // Set Animation Parameters
            Animator.SetFloat(_animIdSpeed, _animationBlend);
            Animator.SetFloat(_animIdMotionSpeed, inputMagnitude);
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_inputStateHandler.look.sqrMagnitude >= MouseMovementThreshold && !lockCameraPosition)
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
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cameraTargetXAxis + cameraAngleOverride,
                _cameraTargetYAxis, 0.0f);
        }

        public void UpdateSpeed()
        {
            float targetSpeed = _inputStateHandler.run ? runSpeed : moveSpeed;

            // Vector == operation uses approximation so it is not error prone.
            if (_inputStateHandler.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed =
                new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _inputStateHandler.analogMovement ? _inputStateHandler.move.magnitude : 1f;

            if (Mathf.Abs(currentHorizontalSpeed - targetSpeed) > speedOffset)
            {
                MoveSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);
                MoveSpeed = Mathf.Round(MoveSpeed * 1000f) / 1000f;
            }
            else
            {
                MoveSpeed = targetSpeed;
            }
        }

        public void Translate()
        {
            // 기존의 이동방향
            var targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // Translate position
            _characterController.Move(targetDirection.normalized * (MoveSpeed * Time.deltaTime) +
                                      new Vector3(0.0f, VerticalVelocity, 0.0f) * Time.deltaTime);
        }

        public void RotateImmediately()
        {
            // without rotation, velocity converges to 0.
            _rotationVelocity = 0f;
            transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
        }

        public void Rotate()
        {
            if (_inputStateHandler.move != Vector2.zero)
            {
                // Get degree of y-axis
                var inputDirection = new Vector3(_inputStateHandler.move.x, 0.0f, _inputStateHandler.move.y).normalized;
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;

                // Mathf.SmoothDampAngle() ensures interpolation
                var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        public bool TryChangeStateByInput(ActionStateMachine stateMachine)
        {
            if (!ChangeStateEnableByInput(stateMachine))
            {
                return false;
            }

            var bufferData = _inputStateHandler.DeQueue();

            stateMachine.ChangeState(bufferData.Type);

            return true;
        }

        public void ChangeStateByInputOrIdle(ActionStateMachine stateMachine)
        {
            Type stateType;
            if (_inputStateHandler.HasBuffer())
            {
                var bufferData = _inputStateHandler.Peek();
                stateType = bufferData.Type;
            }
            else
            {
                stateType = typeof(IdleState);
            }

            if (ChangeStateEnable(stateMachine, stateType))
            {
                _inputStateHandler.TryDeQueue();
                stateMachine.ChangeState(stateType);
            }
            else
            {
                // 에러는 아니지만, 해당 State를 실행할 수 없음.
                if (stateMachine.IsDebug)
                    Debug.LogWarning($"{stateType}을 실행할 수 없음");

                stateMachine.ChangeState(typeof(IdleState));
            }
        }

        private bool ChangeStateEnableByInput(ActionStateMachine stateMachine)
        {
            if (!_inputStateHandler.HasBuffer()) return false;
            var bufferData = _inputStateHandler.Peek();

            return ChangeStateEnable(stateMachine, bufferData.Type);
        }

        private static bool ChangeStateEnable(ActionStateMachine stateMachine, Type type)
        {
            var state = stateMachine.GetState(type);

            if (state.StateChangeEnable(stateMachine))
            {
                return true;
            }

            return false;
        }

        private static float ClampAngle(float value, float min, float max)
        {
            if (value <= -360f) value += 360f;
            if (value >= 360f) value -= 360f;
            return Mathf.Clamp(value, min, max);
        }

        public float GetMoveSpeed()
        {
            return MoveSpeed;
        }

        // this work by animation event
        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f) return;
            if (footstepAudioClips.Length <= 0) return;

            var index = Random.Range(0, footstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.position, footstepAudioVolume);
        }

        // this work by animation event
        private void OnRoll(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f) return;
            AudioSource.PlayClipAtPoint(rollingAudioClip, transform.position, rollingAudioVolume);
        }

        // this work by animation event
        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight < 0.5f) return;
            AudioSource.PlayClipAtPoint(landingAudioClip, transform.position, landingAudioVolume);
        }

        public void ChangeWeapon(AnimationEvent animationEvent)
        {
            Debug.LogWarning($"ChangeWeapon!");
        }
    }
}