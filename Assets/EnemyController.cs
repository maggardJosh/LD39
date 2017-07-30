using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EnemyController : BaseMover
{

    public bool hasMoved = false;
    protected Vector3 startPos;
    protected Vector3 finalPos;
    protected Vector3 initialPos;
    public bool canMove = false;

    public List<EnemyController> blockedMovers = new List<EnemyController>(); //List of pieces that have been blocked by me

    public void StartMoveTween()
    {
        if (canMove)
            if (thingToKill != null)
                EaseToAttack(startPos, thingToKill);
            else
                EaseToPos(startPos, finalPos);
        canMove = false;
        hasMoved = false;
    }

    public void GetInitialPos()
    {
        initialPos = transform.position;
    }

    public abstract bool TryIdealMove();

    public abstract bool TrySecondaryMoves();
}