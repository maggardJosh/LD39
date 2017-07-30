using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryNotification : MonoBehaviour
{

    public Sprite low;
    public Sprite med;
    public Sprite full;
    SpriteRenderer sRend;

    public enum PowerLevel
    {
        LOW, MED, HIGH
    }

    public PowerLevel pLevel = PowerLevel.HIGH;
    // Use this for initialization
    void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
    }

    private float count = 0;
    public float lowFlashTimeOff = .3f;
    public float lowFlashTimeOn = .5f;
    public float medFlashTimeOff = 2.0f;
    public float medFlashTimeOn = 1.0f;

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        switch (pLevel)
        {
            case PowerLevel.LOW:
                bool toggle = false;
                while ((count >= lowFlashTimeOff && !sRend.enabled) ||
                    (count >= lowFlashTimeOn && sRend.enabled))
                {
                    toggle = true;
                    count -= sRend.enabled ? lowFlashTimeOn : lowFlashTimeOff;
                    sRend.enabled = !sRend.enabled;
                }
                if (toggle && sRend.enabled)
                    SoundManager.Play(GameSettings.Instance.WarningSound);
                sRend.sprite = low;
                break;
            case PowerLevel.MED:
                toggle = false;
                while ((count >= medFlashTimeOff && !sRend.enabled) ||
                    (count >= medFlashTimeOn && sRend.enabled))
                {
                    toggle = true;

                    count -= sRend.enabled ? medFlashTimeOn : medFlashTimeOff;
                    sRend.enabled = !sRend.enabled;
                }
                if (toggle && sRend.enabled)
                    SoundManager.Play(GameSettings.Instance.WarningSound);
                sRend.sprite = med;
                break;
            case PowerLevel.HIGH:
                sRend.enabled = false;
                sRend.sprite = full;
                break;
        }
    }

    public void SetLevel(PowerLevel p)
    {
        this.pLevel = p;
    }
}
