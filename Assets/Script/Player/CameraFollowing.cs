using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] GameObject cameraCharacter;

    void Update()
    {
        transform.position = cameraCharacter.transform.position;
    }

}
