using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

namespace BEngineSample
{
    public class InputTestPlayer : MonoBehaviour
    {
        InputSystem _inputSystem = null;

        PlayerInput _playerInput = null;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            _inputSystem = InputSystem.Instance;
            if (_inputSystem != null)
            {
                InputSystem.Key<bool> key = _inputSystem.CancleButton;
                if (key != null)
                    key.OnUp += Escape;
            }

            if (_playerInput != null)
                IsUImode = _playerInput.currentActionMap.name == "UI";
        }

        bool IsUImode { get => _inputSystem.UIMode; set { _inputSystem.UIMode = value; } }

        void Escape(bool value)
        {
            IsUImode = !IsUImode;

            _playerInput.currentActionMap.Disable();

            if (IsUImode)
            {
                _playerInput.SwitchCurrentActionMap("UI");
            }
            else
            {
                _playerInput.SwitchCurrentActionMap("Player");
            }
        }
    }

}