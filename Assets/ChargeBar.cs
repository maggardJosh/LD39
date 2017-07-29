using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBar : MonoBehaviour {

    public static ChargeBar Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ChargeBar>();
            return _instance;
        }
    }
    private static ChargeBar _instance;
    public ChargeSection[] chargeSections;
    public int charges = 8;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
