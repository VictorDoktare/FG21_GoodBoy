using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogProximity : MonoBehaviour
{
    [SerializeField]
    private Animator hobomator;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DogControllerRedux _))
        {
            hobomator.SetTrigger("DogNear");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out DogControllerRedux _))
        {
            hobomator.SetTrigger("DogAway");
        }
    }
}
