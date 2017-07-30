using Assets.Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Follow : MonoBehaviour
{
    public float tweenValue = .2f;
    public GameObject followObj;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        if (followObj == null)
        {
            PlayerController pc = FindObjectOfType<PlayerController>();
            if (pc != null)
                followObj = pc.gameObject;
        }
        if (followObj != null)
        {
            if (!followObj.activeInHierarchy)
            {
                PlayerController pc = FindObjectOfType<PlayerController>();
                if (pc != null)
                    followObj = pc.gameObject;
            }
            transform.position = EaseFunctions.Ease(EaseFunctions.Type.CircOut, tweenValue, transform.position, followObj.transform.position + Vector3.back * 10 - transform.position, 1f);
        }

    }
}
