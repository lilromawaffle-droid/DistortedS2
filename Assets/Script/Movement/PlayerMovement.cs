using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour

{
    //refference di PLayer Input event
    public void Dash(InputAction.CallbackContext context) //Down Up Hold
    {
        Debug.Log("asdjkh");
    }
}
/*
    private InputPlayer _inputPlayer;
    [SerializeField] private float moveSpeed;

    void Awake()
    { 
        _inputPlayer = GetComponent<InputPlayer>(); 
    }
    void Update()
    {
        //movement
        Vector3 positionChange = new Vector3(
            _inputPlayer.movementInputVector.x,
            0,
            _inputPlayer.movementInputVector.y)
            *Time.deltaTime * moveSpeed;

        transform.position += positionChange;

        //dash
        if (_inputPlayer.isDashDown)
        {
            StartCoroutine(Dash());
        }        
    }

    IEnumerator Dash()
    {

    }
*/
