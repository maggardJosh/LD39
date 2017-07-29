using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;

public class EnemyOneController : EnemyController
{

    bool isReady = false;

    public Sprite normal;
    public Sprite flash;

    public override bool TryMove()
    {

        if (!isReady)
        {
            isReady = true;
            return false;
        }
        isReady = false;
        PlayerController p = MoveManager.GetPlayer();
        Vector3 diff = transform.position - p.transform.position;
        Vector2 tryMove = Vector2.zero;
        Vector2 secondMove = Vector2.zero;
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            if (diff.x > 0)
                tryMove = Vector2.left;
            else
                tryMove = Vector2.right;
            if (diff.y < 0)
                secondMove = Vector2.up;
            else
                secondMove = Vector2.down;
        }
        else
        {
            if (diff.y < 0)
            {
                tryMove = Vector2.up;

            }
            else
                tryMove = Vector2.down;
            if (diff.x > 0)
                secondMove = Vector2.right;
            else
                secondMove = Vector2.left;
        }

        startPos = transform.position;
        Vector3 posDisp = new Vector3(tryMove.x * GameSettings.Instance.TileSize / 100f, tryMove.y * GameSettings.Instance.TileSize / 100f, 0);
        finalPos = SnapTile.Snap(startPos + posDisp);
        //If we fail the first move, try the second
        if (!MoveManager.ValidTile(finalPos))
        {
            posDisp = new Vector3(secondMove.x * GameSettings.Instance.TileSize / 100f, secondMove.y * GameSettings.Instance.TileSize / 100f, 0);
            finalPos = SnapTile.Snap(startPos + posDisp);
            if (!MoveManager.ValidTile(finalPos))    //If we fail again just don't move
                return false;
        }
        EnemyController e = MoveManager.EnemyInTile(finalPos);
        if (e != null)
            return false;
        p = MoveManager.PlayerInTile(finalPos);
        if (p != null)
            p.Kill();
        canMove = true;
        transform.position = finalPos;
        return true;
    }

    private float count = 0;
    private bool flashing = false;
    public float flashSpeed = .1f;
    public override void HandleUpdate()
    {
        if (!isReady)
            GetComponent<SpriteRenderer>().sprite = normal;
        else
        {
            count += Time.deltaTime;
            while (count > flashSpeed)
            {
                count -= flashSpeed;
                flashing = !flashing;
            }
            GetComponent<SpriteRenderer>().sprite = flashing ? flash : normal;
        }
        base.HandleUpdate();
    }

}
