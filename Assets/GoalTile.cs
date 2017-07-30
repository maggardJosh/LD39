using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTile : MonoBehaviour
{

    public GameObject NextLevel;
    public bool enemiesKilled = false;
    // Use this for initialization
    void Start()
    {
        enemiesKilled = (FindObjectOfType<EnemyController>() == null);
        GetComponent<SpriteRenderer>().sprite = enemiesKilled ? GameSettings.Instance.GoalOn : GameSettings.Instance.GoalOff;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemiesKilled)
        {
            enemiesKilled = (FindObjectOfType<EnemyController>() == null);

            if (enemiesKilled)
                SoundManager.Play(GameSettings.Instance.LevelEndOpenSound);
            GetComponent<SpriteRenderer>().sprite = enemiesKilled ? GameSettings.Instance.GoalOn : GameSettings.Instance.GoalOff;
        }

    }
}
