using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineAutoplayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cinemachine.CinemachineVirtualCamera[] cvc = GetComponentsInChildren<Cinemachine.CinemachineVirtualCamera>(); 
        GameObject player_object = GameObject.FindGameObjectWithTag("Player");
        foreach (Cinemachine.CinemachineVirtualCamera c in cvc)
        {
            c.Follow = player_object.transform;
            c.LookAt = player_object.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
