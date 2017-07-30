using Assets.Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyExpenseNotification : MonoBehaviour {

    public AnimationCurve yCurve;
    public AnimationCurve scaleCurve;
    public float count = 0;
    public float notifTime = 1.0f;
    public float yDist = .1f;

    public Vector3 startPos;
    public Vector3 direction = Vector3.up;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        count += Time.deltaTime;
        if (count > notifTime)
            Destroy(gameObject);
        else
        {
            Vector3 newPos = transform.position;
            newPos = startPos + Vector3.up * (yCurve.Evaluate(count / notifTime) * yDist);
            transform.position = newPos;
            float scaleValue = scaleCurve.Evaluate(count / notifTime);
            transform.localScale = new Vector3(scaleValue, scaleValue,scaleValue);
        }
	}

    public static void Show(Vector3 pos, Vector3 dir)
    {
        GameObject go = Instantiate(GameSettings.Instance.energyNotifPrefab);
        go.transform.position = pos;
        go.GetComponent<EnergyExpenseNotification>().startPos = pos;
        go.GetComponent<EnergyExpenseNotification>().direction = dir;

    }
}
