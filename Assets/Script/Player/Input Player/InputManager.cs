using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get;private set;}

    private void Awake()
    {
        Instance = this;
    }
    public bool OnPressLeftMouse()
    {
        return Mouse.current.leftButton.wasPressedThisFrame;
    }
    public bool OnPressRightMouse()
    {
        return Mouse.current.rightButton.wasPressedThisFrame;
    }
    public bool OnPressWkey()
    {
        return Keyboard.current.wKey.wasPressedThisFrame;
    }
    public bool OnPressSkey()
    {
        return Keyboard.current.sKey.wasPressedThisFrame;
    }
    public bool OnPressAkey()
    {
        return Keyboard.current.aKey.wasPressedThisFrame;
    }
    public bool OnPressDkey()
    {
        return Keyboard.current.dKey.wasPressedThisFrame;
    }
    public bool OnPressLeftCtrlkey()
    {
        return Keyboard.current.leftCtrlKey.wasPressedThisFrame;
    }
}
