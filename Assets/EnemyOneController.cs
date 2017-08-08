using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;

public class EnemyOneController : EnemyController
{

    public bool isReady = false;

    public Sprite normal;
    public Sprite flash;

    public List<Vector2> Moves = new List<Vector2>();

    public override bool TryMove(bool firstTime = true)
    {

        return false;
    }

    private float count = 0;
    private bool flashing = false;
    public float flashSpeed = .1f;
    Animator anim;
    public override void HandleUpdate()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        if (!isReady)
        {
            GetComponent<SpriteRenderer>().sprite = normal;
            anim.enabled = true;
        }
        else
        {
            anim.enabled = false;
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


    public override bool TryIdealMove()
    {
        if (!isReady)
        {
            isReady = true;
            hasMoved = true;
            return false;
        }
        Moves.Clear();
        transform.position = initialPos;
        PlayerController p = MoveManager.GetPlayer();
        Vector3 diff = transform.position - p.transform.position;
        Vector2 tryMove = Vector2.zero;
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            if (diff.x > 0)
            {
                tryMove = Vector2.left;
                if (diff.y < 0)
                    Moves.AddRange(new Vector2[] { Vector2.up, Vector2.down, Vector2.right });
                else
                    Moves.AddRange(new Vector2[] { Vector2.down, Vector2.up, Vector2.right });
            }
            else
            {
                tryMove = Vector2.right;

                if (diff.y < 0)
                    Moves.AddRange(new Vector2[] { Vector2.up, Vector2.down, Vector2.left });
                else
                    Moves.AddRange(new Vector2[] { Vector2.down, Vector2.up, Vector2.left });
            }
        }
        else
        {
            if (diff.y < 0)
            {
                tryMove = Vector2.up;
                if (diff.x < 0)
                    Moves.AddRange(new Vector2[] { Vector2.right, Vector2.left, Vector2.down });
                else
                    Moves.AddRange(new Vector2[] { Vector2.left, Vector2.right, Vector2.down });

            }
            else
            {
                tryMove = Vector2.down;
                if (diff.x < 0)
                    Moves.AddRange(new Vector2[] { Vector2.right, Vector2.left, Vector2.up });
                else
                    Moves.AddRange(new Vector2[] { Vector2.left, Vector2.right, Vector2.up });
            }
        }

        startPos = transform.position;
        Vector3 posDisp = new Vector3(tryMove.x * GameSettings.Instance.TileSize / 100f, tryMove.y * GameSettings.Instance.TileSize / 100f, 0);
        finalPos = SnapTile.Snap(startPos + posDisp);
        //If we fail the first move, try the second
        if (!MoveManager.ValidTile(finalPos))
            return false;

        //Is there a piece in the way?
        EnemyController e = MoveManager.ObjectInTile<EnemyController>(finalPos);
        if (e != null)
        {
            if (!e.hasMoved)    //If it hasn't moved yet then add us to the list of people waiting on them to move
                e.blockedMovers.Add(this);
            return false;
        }
        thingToKill = MoveManager.ObjectInTile<PlayerController>(finalPos);

        canMove = true;
        if (thingToKill == null)    //Don't move into spot if killing player
            transform.position = finalPos;
        hasMoved = true;
        foreach (EnemyController ec in blockedMovers)
            if (ec != null && !ec.hasMoved)
                ec.TryIdealMove();
        blockedMovers.Clear();
        isReady = false;
        return true;
    }

    public override bool TrySecondaryMoves()
    {
        PlayerController p = MoveManager.GetPlayer();
        transform.position = initialPos;
        Vector3 diff = transform.position - p.transform.position;
        foreach (Vector2 tryMove in Moves)
        {
            startPos = transform.position;
            Vector3 posDisp = new Vector3(tryMove.x * GameSettings.Instance.TileSize / 100f, tryMove.y * GameSettings.Instance.TileSize / 100f, 0);
            finalPos = SnapTile.Snap(startPos + posDisp);
            //If we fail the first move, try the second
            if (!MoveManager.ValidTile(finalPos))
                continue;

            //Is there a piece in the way?
            EnemyController e = MoveManager.ObjectInTile<EnemyController>(finalPos);
            if (e != null)
                continue;

            thingToKill = MoveManager.ObjectInTile<PlayerController>(finalPos);

            canMove = true;
            if (thingToKill == null)    //Don't move into spot if killing player
                transform.position = finalPos;
            hasMoved = true;
            foreach (EnemyController ec in blockedMovers)
                if (ec != null && !ec.hasMoved)
                    ec.TryIdealMove();
            blockedMovers.Clear();
            isReady = false;
            return true;
        }
        isReady = false;
        return false;
    }
}
