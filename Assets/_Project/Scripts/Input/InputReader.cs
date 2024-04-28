using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Platformer
{
    [CreateAssetMenu(menuName = "Platformer/Input Reader")]
    public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
    {
        private PlayerInputActions _inputActions;

        public Vector3 Direction => _inputActions.Player.Move.ReadValue<Vector2>();

        private void OnEnable()
        {
            if (_inputActions == null)
            {
                _inputActions = new PlayerInputActions();
                _inputActions.Player.SetCallbacks(this);
            }

            EnablePlayerActions();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context.control.device));
        }

        public void OnFire(InputAction.CallbackContext context)
        {
        }

        public void OnMouseControlCamera(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    EnableMouseControlCamera.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DisableMouseControlCamera.Invoke();
                    break;
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
        }

        public void OnJump(InputAction.CallbackContext context)
        {
        }

        public void EnablePlayerActions()
        {
            _inputActions.Enable();
        }

        private static bool IsDeviceMouse(InputDevice device)
        {
            return device is Mouse or Touchscreen;
        }

        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2, bool> Look = delegate { };
        public event UnityAction EnableMouseControlCamera = delegate { };
        public event UnityAction DisableMouseControlCamera = delegate { };
    }
}