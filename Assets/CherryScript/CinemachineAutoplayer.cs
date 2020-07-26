using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineAutoplayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cinemachine.CinemachineVirtualCamera cvc = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>(); 
        GameObject player_object = GameObject.FindGameObjectWithTag("Player");
        cvc.Follow = player_object.transform;
        cvc.LookAt = player_object.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
