using Assets.Scripts;
using UnityEngine;

public abstract class EnemyController : BaseMover
{
    public void Kill()
    {
        Destroy(gameObject);
    }

    protected Vector3 startPos;
    protected Vector3 finalPos;
    public bool canMove = false;

    public void StartMoveTween()
    {
        if (canMove)
            EaseToPos(startPos, finalPos);
        canMove = false;
    }
}