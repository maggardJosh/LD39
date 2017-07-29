using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeSection : MonoBehaviour {

    public Sprite ChargedSprite;

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCharge(bool isCharged)
    {
        GetComponent<Image>().sprite = isCharged ? ChargedSprite : GameSettings.Instance.EmptyChargeSprite;
    }
}
