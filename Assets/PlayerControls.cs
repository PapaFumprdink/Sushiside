using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerControls : MonoBehaviour
{
    public event System.Action<InputButton, InputPhase, float> OnInputButtonEvent;

    new Camera camera;

    public Vector2 LookPosition => camera.ScreenToWorldPoint(Input.mousePosition);
    public Vector2 LookDirection => (LookPosition - (Vector2)transform.position).normalized;

    private Dictionary<InputButton, KeyCode> InputMap = new Dictionary<InputButton, KeyCode>()
    {
        { InputButton.Up, KeyCode.W }, { InputButton.Down, KeyCode.S },
        { InputButton.Left, KeyCode.A }, { InputButton.Right, KeyCode.D },

        { InputButton.Jump, KeyCode.Space },

        { InputButton.Fire, KeyCode.Mouse0 },
        { InputButton.AltFire, KeyCode.Mouse1 },
        { InputButton.Reload, KeyCode.R }
    };

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        CheckAllButtons();
    }

    private void CheckAllButtons()
    {
        foreach (var mapping in InputMap)
        {
            CheckMapping(mapping);
        }
    }

    private void CheckMapping(KeyValuePair<InputButton, KeyCode> mapping) => CheckButton(mapping.Key, mapping.Value);

    private void CheckButton(InputButton button, KeyCode key)
    {
             if (Input.GetKeyDown(key)) OnInputButtonEvent?.Invoke(button, InputPhase.Down, 2);
        else if (Input.GetKey    (key)) OnInputButtonEvent?.Invoke(button, InputPhase.Held, 1f);
        else if (Input.GetKeyUp  (key)) OnInputButtonEvent?.Invoke(button, InputPhase.Up, -1f);
    }
}

public enum InputPhase { Down, Held, Up }
public enum InputButton
{
    Up, Down,
    Left, Right,
    Jump,
    Fire, AltFire, Reload
}
