using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{

    public static ChargeBar Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameSettings.Instance.ChargeBar;
            return _instance;
        }
    }
    private static ChargeBar _instance;
    public ChargeSection[] chargeSections;
    public Sprite[] warningSprites;
    public Sprite normalSprite;
    public Sprite orangeSprite;
    public Image chargeIcon;
    public float count;
    public float warningTime = .2f;
    public int charges = 8;
    // Use this for initialization
    void Start()
    {

    }

    int currentWarningIndex = 0;
    // Update is called once per frame
    void Update()
    {

        if (charges < 3)
        {
            count += Time.deltaTime;
            while (count > warningTime)
            {
                count -= warningTime;
                currentWarningIndex++;
                if (currentWarningIndex >= warningSprites.Length)
                    currentWarningIndex = 0;
            }
            chargeIcon.sprite = warningSprites[currentWarningIndex];
        }
        else if (charges < 5)
        {
            chargeIcon.sprite = orangeSprite;
        }
        else
        {
            chargeIcon.sprite = normalSprite;
        }
    }

    public void Recharge()
    {
        charges = chargeSections.Length;
        UpdateCharge();
    }

    public void UseCharge()
    {
        charges--;
        UpdateCharge();
    }

    public void UpdateCharge()
    {
        for (int i = 0; i < chargeSections.Length; i++)
            chargeSections[i].SetCharge(i < charges);
    }
}
