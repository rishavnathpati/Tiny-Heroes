using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        private const float ZEROF = 0f;

        [Header("References")] [SerializeField] [Self]
        private CharacterController characterController;

        [SerializeField] [Self] private Animator animator;
        [SerializeField] [Anywhere] private CinemachineFreeLook freeLookVCam;
        [SerializeField] [Anywhere] private InputReader inputReader;

        [Header("Settings")] [SerializeField] private float moveSpeed = 5f;

        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = .2f;
        private float _currentSpeed;
        private float _currentSpeedVelocity;
        private Transform _mainCamera;
        
        static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            _mainCamera = Camera.main.transform;
            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            freeLookVCam.OnTargetObjectWarped(transform, freeLookVCam.transform.position - Vector3.forward);
        }

        private void Start()
        {
            inputReader.EnablePlayerActions();
        }

        private void Update()
        {
            HandleMovement();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, _currentSpeed);
        }

        private void HandleMovement()
        {
            Vector3 movementDirection = new Vector3(inputReader.Direction.x, ZEROF, inputReader.Direction.y).normalized;

            // Rotate the player movement direction to the direction of the camera rotation
            Vector3 adjustedDirection = Quaternion.AngleAxis(_mainCamera.eulerAngles.y, Vector3.up) * movementDirection;

            if (adjustedDirection.magnitude > ZEROF)
            {
                HandleRotation(adjustedDirection);
                HandleCharacterController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZEROF);
            }
        }

        private void HandleCharacterController(Vector3 adjustedDirection)
        {
            Vector3 adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            characterController.Move(adjustedMovement);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            Quaternion targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
        }

        private void SmoothSpeed(float value)
        {
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, value, ref _currentSpeedVelocity, smoothTime);
        }
    }
}