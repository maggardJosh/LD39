using Assets.Scripts.Extensions;
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

    public GameObject currentLevel = null;
    public GameObject currentLevelPrefab;

    public enum CurrentMove
    {
        PLAYER,
        ENEMIES
    }

    public CurrentMove MoveState = CurrentMove.PLAYER;

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(320, 480, false);
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
                        ChargeBar.Instance.UseCharge();
                        StartCoroutine(EaseFunctions.DelayAction(GameSettings.Instance.MoveTime, () => { isMoving = false; MoveState = CurrentMove.ENEMIES; }));
                        return;
                    }
                }
                break;
            case CurrentMove.ENEMIES:
                if (GetPlayer() == null)
                {
                    //Restart thing
                    return;
                }
                foreach (EnemyController e in FindObjectsOfType<EnemyController>())
                {
                    if (e.TryMove())
                    {
                        isMoving = true;
                        StartCoroutine(EaseFunctions.DelayAction(GameSettings.Instance.MoveTime, () => { isMoving = false; MoveState = CurrentMove.PLAYER; }));
                    }
                }
                if (!isMoving)
                    MoveState = CurrentMove.PLAYER;
                else
                    foreach (EnemyController e in FindObjectsOfType<EnemyController>())
                        e.StartMoveTween();
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

    public static T ObjectInTile<T>(Vector3 pos) where T : MonoBehaviour
    {
        foreach (T e in FindObjectsOfType<T>())
        {
            if (SnapTile.Snap(e.transform.position) == SnapTile.Snap(pos))
                return e;
        }
        return null;
    }

    public static PlayerController GetPlayer()
    {
        return FindObjectOfType<PlayerController>();
    }

    public static void LoadLevel(GameObject level)
    {
        Instance.currentLevelPrefab = level;
        if (Instance.currentLevel != null)
            Destroy(Instance.currentLevel.gameObject);
        GameSettings.Instance.LevelSelect.SetActive(false);
        GameSettings.Instance.LevelSelectButton.SetActive(true);
        ChargeBar.Instance.gameObject.SetActive(true);
        ChargeBar.Instance.Recharge();
        Instance.Reset();
        Instance.currentLevel = Instantiate(level);
        Instance.currentLevel.gameObject.SetActive(true);
    }

    public void ShowLevelSelect()
    {
        if (Instance.currentLevel != null)
            Destroy(Instance.currentLevel.gameObject);
        GameSettings.Instance.LevelSelect.SetActive(true);
        GameSettings.Instance.LevelSelectButton.SetActive(false);
        ChargeBar.Instance.gameObject.SetActive(false);
    }

    public static void ReloadLevel()
    {
        Instance.StartCoroutine(EaseFunctions.DelayAction(GameSettings.Instance.RespawnTime, () => { LoadLevel(Instance.currentLevelPrefab); }));
        
    }

    private void Reset()
    {
        MoveState = CurrentMove.PLAYER;
    }
}
