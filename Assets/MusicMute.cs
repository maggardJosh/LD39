using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicMute : MonoBehaviour {

    public Image buttonImage;
    public Color mutedColor;
	// Use this for initialization
	void Start () {
        Toggle t = GetComponent<Toggle>();
        t.isOn = PlayerPrefs.GetInt("Muted") == 1;
        UpdateMute(t.isOn);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateMute(bool value)
    {
        PlayerPrefs.SetInt("Muted", value ? 1 : 0);
        buttonImage.color = value ? Color.white : mutedColor;
    }
}
