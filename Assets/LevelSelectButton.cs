using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelSelectButton : MonoBehaviour {

    public GameObject levelPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.isPlaying)
            return;
        GetComponent<Button>().interactable = levelPrefab != null;
	}

    public void LoadLevel()
    {
        MoveManager.LoadLevel(levelPrefab);
    }
}
