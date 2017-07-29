using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using System;

public class EnemyOneController : EnemyController {

    bool isReady = false;

    public Sprite normal;
    public Sprite flash;

    public override bool TryMove()
    {
        
        if(!isReady)
        {
            isReady = true;
            return false;
        }
        isReady = false;
        Vector2 tryMove = Vector2.up;
        Vector3 startPos = transform.position;
        Vector3 posDisp = new Vector3(tryMove.x * GameSettings.Instance.TileSize / 100f, tryMove.y * GameSettings.Instance.TileSize / 100f, 0);
        Vector3 finalPos = SnapTile.Snap(startPos + posDisp);
        if (!MoveManager.ValidTile(finalPos))
            return false;
        PlayerController p = MoveManager.PlayerInTile(finalPos);
        if (p != null)
            p.Kill();
        EaseToPos(startPos, finalPos);
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
