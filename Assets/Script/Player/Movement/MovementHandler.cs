using NUnit.Framework;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] CharacterController characterController;

    //walking number
    [SerializeField] float moveSpeed;

    //animation
    [SerializeField] public bool isWalking;
    [SerializeField] public bool isRunning;

    
    void Start()
    {
        
    }
    
    void Update()
    {
        Walking();
        isRunningChecker();
    }

    void Walking()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0f, v);
        characterController.Move(movement * moveSpeed * Time.deltaTime);
        isWalking = movement.magnitude > 0.1f;
        
    }

    void isRunningChecker()
    {
        if (InputManager.Instance.OnPressLeftCtrlkey())
        {
            if(isRunning)
            {
                isRunning= false;
            }
            else
            {
                isRunning = true;
            }
        }
    }
}
