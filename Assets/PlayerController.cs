using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Extensions;

public class PlayerController : BaseMover
{

    public LayerMask TileLayer;
    public LayerMask EnemyLayer;


    public override bool TryMove()
    {
        Vector2 tryMove = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.S))
            tryMove += Vector2.down;
        else if (Input.GetKeyDown(KeyCode.W))
            tryMove += Vector2.up;
        else if (Input.GetKeyDown(KeyCode.A))
            tryMove += Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D))
            tryMove += Vector2.right;

        if (tryMove == Vector2.zero)
            return false;
        Vector3 startPos = transform.position;
        Vector3 posDisp = new Vector3(tryMove.x * GameSettings.Instance.TileSize / 100f, tryMove.y * GameSettings.Instance.TileSize / 100f, 0);
        Vector3 finalPos = SnapTile.Snap(startPos + posDisp);
        if (!MoveManager.ValidTile(finalPos))
            return false;
        EnemyController e = MoveManager.ObjectInTile<EnemyController>(finalPos);
        if (e != null)
        {
            EaseToAttack(startPos, e);
        }
        else
        {
            EaseToPos(startPos, finalPos);
        }



        return true;
    }

    protected override void HandleMoveDone()
    {
        ChargeStation c = MoveManager.ObjectInTile<ChargeStation>(transform.position);
        if (c != null)
            ChargeBar.Instance.Recharge();
        if (ChargeBar.Instance.charges <= 0)
            Kill();
        base.HandleMoveDone();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
