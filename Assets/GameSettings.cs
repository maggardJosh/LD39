using Assets.Scripts.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{

    public float TileSize = 20f;
    public float MoveTime = .3f;
    public float AttackTime = .6f;
    public AnimationCurve MoveCurve;
    public AnimationCurve AttackCurve;
    public Sprite EmptyChargeSprite;
    public GameObject DustPrefab;
    public float DustParticleChance = .3f;
    public float DustDisp = .2f;
    public Vector3 DustStartDisp;
    public static bool IsShuttingDown = false;
    public ChargeBar ChargeBar;
    public GameObject LevelSelect;
    public GameObject LevelIndicator;
    public float RespawnTime = .5f;
    public GameObject LevelSelectButton;
    public GameObject ChargeParticlePrefab;
    public int ChargeParticles = 20;
    public Sprite GoalOn;
    public Sprite GoalOff;
    public GameObject batteryNotifPrefab;
    public GameObject energyNotifPrefab;
    public Transform ParticleContainer;
    public SoundGroup PlayerMoveSound;
    public SoundGroup PlayerMove1Sound;
    public SoundGroup DeathSound;
    public SoundGroup ChargeSound;
    public SoundGroup WarningSound;
    public SoundGroup LevelEndSound;
    public SoundGroup LevelEndOpenSound;
    public SoundGroup Music;
    public GameObject starPrefab;
    public Vector2 starDisp;
    public float MinSwipeDist = 35;

    public bool isTransitioning = true;
    public float SceneTransitionTime = 1.0f;

    public int currentLevel = 1;

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

    public void OnApplicationQuit()
    {
        IsShuttingDown = true;
    }

    private static GameSettings _instance;
    // Use this for initialization
    void Start()
    {
        TransitionLevelSelectIn(true);

    }

    public void TransitionLevelSelectIn(bool delay = true)
    {
        isTransitioning = true;
        Vector3 startPos = Vector3.up * 450;
        Vector3 endPos = Vector3.zero;
        LevelSelect.transform.localPosition = startPos;
        float cBarX = Mathf.Min(10, ((RectTransform)ChargeBar.transform).anchoredPosition.x);
        float cBarTargetX = -70;
        StartCoroutine(EaseFunctions.DelayAction(delay ? .3f : 0, () =>
        {
            StartCoroutine(EaseFunctions.GenericTween(EaseFunctions.Type.BackOut, SceneTransitionTime, (t) =>
            {
                ((RectTransform)LevelSelect.transform).localPosition = startPos * (1 - t);
                Vector3 chargeBarPos = ((RectTransform)ChargeBar.transform).anchoredPosition;
                chargeBarPos.x = cBarX * (1 - t) + (cBarTargetX) * t;
                ((RectTransform)ChargeBar.transform).anchoredPosition = chargeBarPos;
                chargeBarPos.y = ((RectTransform)LevelSelectButton.transform).anchoredPosition.y;
                ((RectTransform)LevelSelectButton.transform).anchoredPosition = chargeBarPos;
                Vector3 levelInd = ((RectTransform)LevelIndicator.transform).anchoredPosition;
                levelInd.y = -chargeBarPos.x;
                ((RectTransform)LevelIndicator.transform).anchoredPosition = levelInd;

            }, null, () => { isTransitioning = false; }));
        }));
    }

    public void TransitionLevelSelectOut(Action endAction)
    {
        if (!LevelSelect.activeInHierarchy)
            return;
        isTransitioning = true;
        Vector3 startPos = Vector3.zero;
        Vector3 endPos = Vector3.up * 450;
        LevelSelect.transform.localPosition = startPos;
        float cBarX = -70;
        float cBarTargetX = 10;
        StartCoroutine(EaseFunctions.GenericTween(EaseFunctions.Type.BackOut, SceneTransitionTime, (t) =>
        {
            ((RectTransform)LevelSelect.transform).localPosition = endPos * t;
            Vector3 chargeBarPos = ((RectTransform)ChargeBar.transform).anchoredPosition;
            chargeBarPos.x = cBarX * (1 - t) + (cBarTargetX) * t;
            ((RectTransform)ChargeBar.transform).anchoredPosition = chargeBarPos;
            chargeBarPos.y = ((RectTransform)LevelSelectButton.transform).anchoredPosition.y;
            ((RectTransform)LevelSelectButton.transform).anchoredPosition = chargeBarPos;
            Vector3 levelInd = ((RectTransform)LevelIndicator.transform).anchoredPosition;
            levelInd.y = -chargeBarPos.x;
            ((RectTransform)LevelIndicator.transform).anchoredPosition = levelInd;
        }, null, endAction));

    }

    void OnAwake()
    {
        _instance = this;
    }

    private void UpdateLevelText()
    {
        foreach (Text t in LevelIndicator.GetComponentsInChildren<Text>())
            t.text = currentLevel.ToString();
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        UpdateLevelText();
    }

    public void IncreaseLevel()
    {
        currentLevel++;
        UpdateLevelText();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
