using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{   
    public bool isDashDown;
    public  Vector2 movementInputVector {get;private set;}

    void Awake()
    {
        isDashDown = false;
    }
    //onMove = nama di input setting
    public void OnMove(InputValue inputValue)
    {
        movementInputVector = inputValue.Get<Vector2>();
        //Debug.Log(inputValue.Get<Vector2>());
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame){isDashDown = true;}
    }
}
