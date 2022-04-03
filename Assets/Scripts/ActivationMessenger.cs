using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationMessenger : MonoBehaviour
{
    public GameObject reciever;

    private void OnEnable()
    {
        reciever.SetActive(true);
    }
}
