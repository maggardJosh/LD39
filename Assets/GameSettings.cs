using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public float TileSize = 20f;
    public float MoveTime = .3f;
    public AnimationCurve MoveCurve;
    public Sprite EmptyChargeSprite;

    public static GameSettings Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameSettings>();
            if (_instance == null)
                throw new Exception("Need GameSettings instance in scene");
            return _instance;
        }
    }

    private static GameSettings _instance;
	// Use this for initialization
	void Start () {
		
	}

    void OnAwake()
    {
        _instance = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
