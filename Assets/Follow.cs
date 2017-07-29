using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Follow : MonoBehaviour
{

    public GameObject followObj;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (followObj != null)
            transform.position = followObj.transform.position + Vector3.back * 10;
    }
}
