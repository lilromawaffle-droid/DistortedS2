using UnityEngine;

public class MovementAnimatorHandler : MonoBehaviour
{
    [SerializeField] private Animator movementAnimator;
    [SerializeField] private  MovementHandler movementHandler;
    [SerializeField ]private bool isWalkingHash;
    [SerializeField] private bool isRunningHash;

    void Awake()
    {
        movementAnimator = GetComponent <Animator>();
    }

    void Start()
    {
        //isWalkingHash = Animator.StringToHash("isWalking");
        //isRunningHash = Animator.StringToHash("isRunning");
    }

    void Update()
    {
        isWalkingHash = movementHandler.isWalking;
        movementAnimator.SetBool("isWalking",movementHandler.isWalking);
        movementAnimator.SetBool("isRunning",movementHandler.isRunning);
    }

}
