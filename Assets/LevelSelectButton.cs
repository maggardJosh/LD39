using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelSelectButton : MonoBehaviour
{

    public GameObject levelPrefab;
    public GameObject starObj;

    // Use this for initialization
    void Start()
    {
        if (Application.isPlaying)
        {

            starObj = Instantiate(GameSettings.Instance.starPrefab);
            starObj.transform.SetParent(transform);
            ((RectTransform)starObj.transform).anchoredPosition = GameSettings.Instance.starDisp;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Application.isPlaying)
        {
            starObj.SetActive(PlayerPrefs.GetInt(gameObject.name, 0) == 1);
            return;
        }

        foreach (Text t in GetComponentsInChildren<Text>())
            t.text = gameObject.name;
        GetComponent<Button>().interactable = levelPrefab != null;
    }

    public void LoadLevel()
    {
        GameSettings.Instance.SetLevel(int.Parse(gameObject.name));

        MoveManager.LoadLevel(levelPrefab);
    }

}
