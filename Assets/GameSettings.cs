using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public float TileSize = 20f;
    public float MoveTime = .3f;
    public AnimationCurve MoveCurve;
    public AnimationCurve AttackCurve;
    public Sprite EmptyChargeSprite;
    public GameObject DustPrefab;
    public float DustParticleChance = .3f;
    public float DustDisp = .2f;
    public Vector3 DustStartDisp;

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
