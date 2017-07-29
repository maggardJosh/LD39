using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOut : MonoBehaviour {

    public float time = 1.0f;
    private float startAngle;
    public AnimationCurve scaleCurve;
	// Use this for initialization
	void Start () {

        startAngle = UnityEngine.Random.Range(0, 360f);
    }

    protected float count = 0;
	// Update is called once per frame
	void Update () {
        HandleUpdate();
        count += Time.deltaTime;
        float value = scaleCurve.Evaluate(count / time);
        transform.localScale = new Vector3(value, value, value);
        transform.localRotation = Quaternion.AngleAxis(startAngle + count * 360f, Vector3.back);

        if (count >= time)
            Destroy(gameObject);

    }

    protected virtual void HandleUpdate()
    {

    }
}
