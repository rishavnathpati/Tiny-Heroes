using System.Collections;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace Platformer
{
    public class CameraManager : ValidatedMonoBehaviour
    {
        [Header("References")] [SerializeField] [Anywhere]
        private InputReader inputReader;

        [SerializeField] [Anywhere] private CinemachineFreeLook freeLookVCam;

        [Header("Settings")] [SerializeField] [Range(0.5f, 3)]
        private float speedMultiplier = 0.1f;

        private bool _cameraMovementLock;
        private bool _isRmbPressed;

        private void OnEnable()
        {
            inputReader.Look += OnLook;
            inputReader.EnableMouseControlCamera += OnEnableMouseControlCamera;
            inputReader.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        private void OnDisable()
        {
            inputReader.Look -= OnLook;
            inputReader.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            inputReader.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        private void OnDisableMouseControlCamera()
        {
            _isRmbPressed = false;

            //unlock mouse cursor and show
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Reset the camera movement lock
            freeLookVCam.m_XAxis.m_InputAxisValue = 0;
            freeLookVCam.m_YAxis.m_InputAxisValue = 0;
        }

        private void OnEnableMouseControlCamera()
        {
            _isRmbPressed = true;

            //lock mouse cursor to center of screen and hide
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseForFrame());
        }

        private IEnumerator DisableMouseForFrame()
        {
            _cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            _cameraMovementLock = false;
        }

        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (_cameraMovementLock)
                return;

            if (isDeviceMouse && !_isRmbPressed) return;

            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

            freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplier * deviceMultiplier;
            freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplier * deviceMultiplier;
        }
    }
}