using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnstartScene1 : MonoBehaviour
{
    [SerializeField] float minBlink,Maxblnk;
    [SerializeField] Animation animBlink;
    

    void Start()
    {
        
    }
    IEnumerator blink()
    {
        yield return new WaitForSeconds (Random.Range(minBlink,Maxblnk));
        //animBlink.
    }
}
