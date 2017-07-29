using Assets.Scripts.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeParticle : ScaleOut
{

    Vector2 vel;
    public Vector2 minVelValues;
    public Vector2 maxVelValues;
    public Vector3 startPos;
    public bool returning = false;
    // Use this for initialization
    void Start()
    {
        count = Random.Range(-.4f, 0);
        vel = new Vector2(Random.Range(minVelValues.x, maxVelValues.x), Random.Range(minVelValues.y, maxVelValues.y));
        if (Random.value <= .5f)
            vel.x *= -1;
    }

    public PlayerController p;
    // Update is called once per frame
    protected override void HandleUpdate()
    {
        if (p == null)
            p = MoveManager.GetPlayer();
        if (!returning)
        {

            if (count > time / 4f)
            {
                returning = true;
                startPos = transform.position;
            }
            else
                transform.position = transform.position + new Vector3(vel.x, vel.y) * Time.deltaTime;
        }
        else
        {
            transform.position = EaseFunctions.Ease(EaseFunctions.Type.CircOut, count - time / 4f, startPos,  p.transform.position - startPos, time * 3f / 4f);
        }
    }
}
