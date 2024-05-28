using UnityEngine;
using Random = UnityEngine.Random;

namespace CharacterControl
{
    [RequireComponent(typeof(CharacterController), typeof(Animator), typeof(InputStateHandler))]
    public class ThirdPlayerController : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private float moveSpeed = 2.0f;
        [SerializeField] private float runSpeed = 5.335f;
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
        [SerializeField] private float rollSpeed = 5f;
        [SerializeField] private float rollMotionSpeed = 1f;
        [SerializeField] private float rollThreshold = 0f;

        [Header("Jump")]
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float jumpThreshold = 0.2f;

        [Header("Gravity")]
        [SerializeField] private float gravity = -15.0f;
        [SerializeField] private float fallThreshold = 0.15f;

        [Header("Player Grounded")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private float groundedOffset = -0.14f;
        [SerializeField] private float groundedRadius = 0.28f;
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

        private float _moveSpeed;
        private float _verticalVelocity;
        private float _rotationVelocity;
        private float _animationBlend;
        private float _targetRotation;

        // Time after landing when the action cannot be performed
        private float _jumpTimeoutDelta;
        private float _rollTimeoutDelta;

        // Time taken to perform falling action
        private float _fallTimeoutDelta;

        private CharacterController _characterController;
        private Animator _animator;
        private InputStateHandler _inputStateHandler;
        private GameObject _mainCamera;

        private const float MaxVerticalVelocity = 53.0f;
        private const float MouseMovementThreshold = 0.01f;

        private readonly int _animIdSpeed = Animator.StringToHash("Speed");
        private readonly int _animIdGrounded = Animator.StringToHash("Grounded");
        private readonly int _animIdJump = Animator.StringToHash("Jump");
        private readonly int _animIdRoll = Animator.StringToHash("Roll");
        private readonly int _animIdRollMotionSpeed = Animator.StringToHash("RollMotionSpeed");
        private readonly int _animIdFall = Animator.StringToHash("FreeFall");
        private readonly int _animIdMotionSpeed = Animator.StringToHash("RunMotionSpeed");


        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _inputStateHandler = GetComponent<InputStateHandler>();
            if (Camera.main != null) _mainCamera = Camera.main.gameObject;

            _animator.SetFloat(_animIdRollMotionSpeed, rollMotionSpeed);

            _fallTimeoutDelta = fallThreshold;
            _jumpTimeoutDelta = jumpThreshold;
            _rollTimeoutDelta = rollThreshold;
        }

        private void Update()
        {
            JumpAndRollAndGravity();
            GroundedCheck();
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void Move()
        {
            float targetSpeed = _inputStateHandler.run ? runSpeed : moveSpeed;

            // Vector == operation uses approximation so it is not error prone.
            if (_inputStateHandler.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed =
                new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _inputStateHandler.analogMovement ? _inputStateHandler.move.magnitude : 1f;

            var isRolling = _animator.GetBool(_animIdRoll);

            // Set Translate Speed
            if (isRolling)
            {
                _moveSpeed = rollSpeed;
            }
            else if (Mathf.Abs(currentHorizontalSpeed - targetSpeed) > speedOffset)
            {
                _moveSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);
                _moveSpeed = Mathf.Round(_moveSpeed * 1000f) / 1000f;
            }
            else
            {
                _moveSpeed = targetSpeed;
            }

            // Set BlendTree Speed
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // Set Direction of Move
            if (!isRolling && _inputStateHandler.move != Vector2.zero)
            {
                // Get degree of y-axis
                Vector3 inputDirection = new Vector3(_inputStateHandler.move.x, 0.0f, _inputStateHandler.move.y)
                    .normalized;
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;

                // Mathf.SmoothDampAngle() ensures interpolation
                var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation,
                    ref _rotationVelocity, rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            var targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // Translate position
            _characterController.Move(targetDirection.normalized * (_moveSpeed * Time.deltaTime) +
                                      new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // Set Animation Parameters
            _animator.SetFloat(_animIdSpeed, _animationBlend);
            _animator.SetFloat(_animIdMotionSpeed, inputMagnitude);
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            var spherePosition =
                new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
            isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);

            _animator?.SetBool(_animIdGrounded, isGrounded);
        }

        private void JumpAndRollAndGravity()
        {
            if (isGrounded)
            {
                _fallTimeoutDelta = fallThreshold;

                _animator.SetBool(_animIdFall, false);
                _animator.SetBool(_animIdJump, false);

                var isRolling = _animator.GetBool(_animIdRoll);

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Roll
                if (!isRolling && _inputStateHandler.roll && _rollTimeoutDelta <= 0.0f)
                {
                    // Get Direction & Rotate Character 
                    Vector3 inputDirection = new Vector3(_inputStateHandler.move.x, 0.0f, _inputStateHandler.move.y)
                        .normalized;
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      _mainCamera.transform.eulerAngles.y;
                    transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);

                    // without rotation, velocity converges to 0.
                    _rotationVelocity = 0f;

                    // Set MoveDisable while rolling

                    _animator?.SetBool(_animIdRoll, true);
                }
                else if (!isRolling && _inputStateHandler.jump && _jumpTimeoutDelta <= 0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                    // update animator if using character
                    _animator?.SetBool(_animIdJump, true);
                }

                if (_rollTimeoutDelta >= 0.0f)
                {
                    _rollTimeoutDelta -= Time.deltaTime;
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the timeout timer
                _rollTimeoutDelta = rollThreshold;
                _jumpTimeoutDelta = jumpThreshold;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    _animator?.SetBool(_animIdFall, true);
                }

                _inputStateHandler.jump = false;
            }

            if (_verticalVelocity < MaxVerticalVelocity)
            {
                _verticalVelocity += gravity * Time.deltaTime;
            }
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

        private static float ClampAngle(float value, float min, float max)
        {
            if (value <= -360f) value += 360f;
            if (value >= 360f) value -= 360f;
            return Mathf.Clamp(value, min, max);
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

        public float GetMoveSpeed()
        {
            return _moveSpeed;
        }
    }
}