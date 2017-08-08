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

    public bool isMoving = false;

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

    public void PlayerDoneMoving()
    {
        isMoving = false;
        MoveState = CurrentMove.ENEMIES;
    }

    public void EnemyDoneMoving()
    {
        isMoving = false;
        MoveState = CurrentMove.PLAYER;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSettings.Instance.isTransitioning)
        {
            EnemyDoneMoving();
            return;
        }
        if (isMoving)
        {
            if (MoveState == CurrentMove.PLAYER)
                foreach (PlayerController p in FindObjectsOfType<PlayerController>())
                    p.CheckForPreMoves();
            return;
        }
        Instance.ShouldSpawnParticlesOnDeath = true;
        switch (MoveState)
        {
            case CurrentMove.PLAYER:
                foreach (PlayerController p in FindObjectsOfType<PlayerController>())
                {
                    if (p.TryMove())
                    {
                        isMoving = true;
                        ChargeBar.Instance.UseCharge();
                        EnergyExpenseNotification.Show(p.transform.position, p.lastMove);
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
                bool isAttack = false;
                foreach (EnemyController e in FindObjectsOfType<EnemyController>())
                {
                    e.GetInitialPos();
                    if (e.TryIdealMove())
                    {
                        isMoving = true;
                        isAttack &= e.thingToKill != null;  //If we've got an attacker this turn
                    }
                }
                foreach (EnemyController e in FindObjectsOfType<EnemyController>())
                {
                    if (!e.hasMoved)
                        if (e.TrySecondaryMoves())
                        {
                            isMoving = true;
                            isAttack &= e.thingToKill != null;
                        }
                }
                if (!isMoving)
                {
                    MoveState = CurrentMove.PLAYER;
                    foreach (EnemyController e in FindObjectsOfType<EnemyController>())
                        e.hasMoved = false;
                }
                else
                {
                    foreach (EnemyController e in FindObjectsOfType<EnemyController>())
                        e.StartMoveTween();
                    StartCoroutine(EaseFunctions.DelayAction(isAttack ? GameSettings.Instance.AttackTime : GameSettings.Instance.MoveTime, () => { isMoving = false; MoveState = CurrentMove.PLAYER; }));
                }
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
    public bool ShouldSpawnParticlesOnDeath = true;
    public static void LoadLevel(GameObject level)
    {
        if (GameSettings.Instance.isTransitioning)
            return;
        

        GameSettings.Instance.TransitionLevelSelectOut(null);
        if (level == Instance.currentLevelPrefab)
        {
            Instance.ShouldSpawnParticlesOnDeath = level == Instance.currentLevelPrefab;

            Instance.currentLevelPrefab = level;
            if (Instance.currentLevel != null)
                Destroy(Instance.currentLevel.gameObject);
            foreach (Transform t in GameSettings.Instance.ParticleContainer)
                Destroy(t.gameObject);
            GameSettings.Instance.LevelSelect.SetActive(false);
            ChargeBar.Instance.Recharge();
            Instance.Reset();
            Instance.currentLevel = Instantiate(level);
            Instance.currentLevel.gameObject.SetActive(true);
        }
        else
        {
            Follow f = FindObjectOfType<Follow>();
            f.enabled = false;
            Vector3 startPos = f.transform.position;
            Vector3 disp = Vector3.down * 5;
            GameSettings.Instance.isTransitioning = true;

            Instance.StartCoroutine(EaseFunctions.GenericTween(EaseFunctions.Type.BackIn, GameSettings.Instance.SceneTransitionTime, (t) =>
          {
              f.transform.position = startPos + disp * t;
          }, null, () =>
          {
              if (Instance.currentLevelPrefab != null)
                  GameSettings.Instance.IncreaseLevel();
              GameSettings.Instance.isTransitioning = false;
              f.enabled = true;
              f.transform.position = Vector3.up * 6;
              Instance.ShouldSpawnParticlesOnDeath = level == Instance.currentLevelPrefab;
              if (level == null)
              {
                  Instance.ShowLevelSelect();
                  return;
              }
              Instance.currentLevelPrefab = level;
              if (Instance.currentLevel != null)
                  Destroy(Instance.currentLevel.gameObject);
              foreach (Transform t in GameSettings.Instance.ParticleContainer)
                  Destroy(t.gameObject);
              GameSettings.Instance.LevelSelect.SetActive(false);
              ChargeBar.Instance.Recharge();
              Instance.Reset();
              Instance.currentLevel = Instantiate(level);
              Instance.currentLevel.gameObject.SetActive(true);
              f.transform.position = GetPlayer().transform.position + Vector3.up * 6;
          }));
        }
    }

    public void ShowLevelSelect()
    {
        if (GameSettings.Instance.isTransitioning)
            return;
        Follow f = FindObjectOfType<Follow>();
        f.enabled = false;
        Vector3 startPos = f.transform.position;
        Vector3 disp = Vector3.up * 5;
        GameSettings.Instance.isTransitioning = true;
        currentLevelPrefab = null;

        GameSettings.Instance.TransitionLevelSelectIn();
        GameSettings.Instance.LevelSelect.SetActive(true);
        Instance.StartCoroutine(EaseFunctions.GenericTween(EaseFunctions.Type.CircIn, GameSettings.Instance.SceneTransitionTime, (t) =>
        {

            f.transform.position = startPos + disp * t;
        }, null, () =>
        {
            f.enabled = true;
            if (Instance.currentLevel != null)
                Destroy(Instance.currentLevel.gameObject);
        }));
    }

    public static void ReloadLevel()
    {
        Instance.StartCoroutine(EaseFunctions.DelayAction(GameSettings.Instance.RespawnTime, () => { LoadLevel(Instance.currentLevelPrefab); }));

    }

    private void Reset()
    {
        MoveState = CurrentMove.PLAYER;
        isMoving = false;
    }
}
