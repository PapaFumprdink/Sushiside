using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[DisallowMultipleComponent]
public class PlayerControls : MonoBehaviour
{
    [SerializeField] private int gamepadIndex;

    new Camera camera;
    Dictionary<InputButton, bool> previousStateMap;

    public Vector2 LookPosition { get; private set; }
    public Vector2 LookDirection => (LookPosition - (Vector2)transform.position).normalized;

    private void Start()
    {
        camera = Camera.main;

        previousStateMap = new Dictionary<InputButton, bool>();
        foreach (var button in (InputButton[])Enum.GetValues(typeof(InputButton)))
        {
            previousStateMap.Add(button, false);
        }
    }

    private void LateUpdate()
    {
        foreach (var button in (InputButton[])Enum.GetValues(typeof(InputButton)))
        {
            previousStateMap[button] = GetButtonState(button);
        }
    }

    public InputPhase GetButtonPhase(InputButton button)
    {
        var state = GetButtonState(button);
        var previousState = previousStateMap[button];

        if (state)
        {
            if (previousState) return InputPhase.Held;
            else return InputPhase.Down;
        }
        else
        {
            if (previousState) return InputPhase.Up;
            else return InputPhase.Idle;
        }
    }

    public bool GetButtonState(InputButton button) => GetButtonValue(button) > 0.5f;

    public float GetButtonValue (InputButton button)
    {
        if (Gamepad.all.Count > gamepadIndex)
        {
            var gamepad = Gamepad.all[gamepadIndex];

            LookPosition = (Vector2)transform.position + gamepad.rightStick.ReadValue();

            switch (button)
            {
                case InputButton.Horizontal:
                    if (gamepad.dpad.left.isPressed) return -1f;
                    if (gamepad.dpad.right.isPressed) return 1f;
                    return gamepad.leftStick.ReadValue().x;

                case InputButton.Vertical:
                    if (gamepad.dpad.down.isPressed) return -1f;
                    if (gamepad.dpad.up.isPressed) return 1f;
                    return gamepad.leftStick.ReadValue().y;

                case InputButton.Jump:
                    if (gamepad.dpad.up.isPressed) return 1f;
                    return gamepad.aButton.isPressed ? 1f : 0f;
                case InputButton.Fire:
                    return gamepad.rightShoulder.isPressed ? 1f : 0f;
                case InputButton.Throw:
                    return gamepad.leftShoulder.isPressed ? 1f : 0f;
                default:
                    return 0f;
            }
            
        }
        else if (gamepadIndex == 0)
        {
            LookPosition = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            switch (button)
            {
                case InputButton.Horizontal:
                    return (Keyboard.current.aKey.isPressed ? -1f : 0f) + (Keyboard.current.dKey.isPressed ? 1f : 0f);
                case InputButton.Vertical:
                    return (Keyboard.current.wKey.isPressed ? 1f : 0f) + (Keyboard.current.sKey.isPressed ? -1f : 0f);
                case InputButton.Jump:
                    return Keyboard.current.spaceKey.isPressed ? 1f : 0f;
                case InputButton.Fire:
                    return Mouse.current.leftButton.isPressed ? 1f : 0f;
                case InputButton.Throw:
                    return Mouse.current.rightButton.isPressed ? 1f : 0f;
                default:
                    return 0f;
            }
        }
        else return 0f;
    }
}

public enum InputPhase { Down, Held, Up, Idle }
public enum InputButton
{
    Horizontal, Vertical,
    Jump,
    Fire, Throw
}
