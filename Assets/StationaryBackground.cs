using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryBackground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,5);
        transform.localScale = Camera.main.transform.localScale;
    }
}
