﻿using Assets.Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{

    public static MoveManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<MoveManager>();
            if (_instance == null)
                throw new System.Exception("Need Move Manager instance in scene");
            return _instance;
        }
    }
    private static MoveManager _instance;

    private bool isMoving = false;

    public enum CurrentMove
    {
        PLAYER,
        ENEMIES
    }

    public CurrentMove MoveState = CurrentMove.PLAYER;

    // Use this for initialization
    void Start()
    {

    }

    void OnAwake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            return;
        switch (MoveState)
        {
            case CurrentMove.PLAYER:
                foreach (PlayerController p in FindObjectsOfType<PlayerController>())
                {
                    if (p.TryMove())
                    {
                        isMoving = true;
                        StartCoroutine(EaseFunctions.DelayAction(GameSettings.Instance.MoveTime, () => { isMoving = false; MoveState = CurrentMove.ENEMIES; }));
                        return;
                    }
                }
                break;
            case CurrentMove.ENEMIES:
                foreach (EnemyController e in FindObjectsOfType<EnemyController>())
                {
                    if (e.TryMove())
                    {
                        isMoving = true;
                        StartCoroutine(EaseFunctions.DelayAction(GameSettings.Instance.MoveTime, () => { isMoving = false; MoveState = CurrentMove.PLAYER; }));
                        return;
                    }
                }
                if (!isMoving)
                    MoveState = CurrentMove.PLAYER;
                break;
        }
    }

    public static bool ValidTile(Vector3 pos)
    {
        foreach (Tile t in FindObjectsOfType<Tile>())
        {
            if (SnapTile.Snap(t.transform.position) == SnapTile.Snap(pos))
                return true;
        }
        return false;
    }

    public static EnemyController EnemyInTile(Vector3 pos)
    {
        foreach (EnemyController e in FindObjectsOfType<EnemyController>())
        {
            if (SnapTile.Snap(e.transform.position) == SnapTile.Snap(pos))
                return e;
        }
        return null;
    }

    public static PlayerController PlayerInTile(Vector3 pos)
    {
        foreach (PlayerController e in FindObjectsOfType<PlayerController>())
        {
            if (SnapTile.Snap(e.transform.position) == SnapTile.Snap(pos))
                return e;
        }
        return null;
    }
}
