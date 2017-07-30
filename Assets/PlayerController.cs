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
    public Vector2 storedMove;
    public BatteryNotification bNotif;

    public override void HandleAwake()
    {
        bNotif = GameObject.Instantiate(GameSettings.Instance.batteryNotifPrefab).GetComponent<BatteryNotification>();
        bNotif.gameObject.transform.SetParent(transform);
        bNotif.gameObject.transform.localPosition = Vector3.up * .08f;
        bNotif.SetLevel(BatteryNotification.PowerLevel.HIGH);
        base.HandleAwake();
    }
    public void CheckForPreMoves()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            storedMove = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            storedMove = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            storedMove = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            storedMove = Vector2.right;
    }

    public Vector3 lastMove = Vector3.zero;
    public override bool TryMove(bool firstTime = true)
    {

        Vector2 tryMove = storedMove;
        storedMove = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            tryMove += Vector2.down;
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            tryMove += Vector2.up;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            tryMove += Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            tryMove += Vector2.right;

        if (tryMove == Vector2.zero)
            return false;
        Vector3 startPos = transform.position;
        Vector3 posDisp = new Vector3(tryMove.x * GameSettings.Instance.TileSize / 100f, tryMove.y * GameSettings.Instance.TileSize / 100f, 0);
        Vector3 finalPos = SnapTile.Snap(startPos + posDisp);
        if (!MoveManager.ValidTile(finalPos))
            return false;
        EnemyController e = MoveManager.ObjectInTile<EnemyController>(finalPos);
        lastMove = tryMove;
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
        {
            ChargeBar.Instance.Recharge();
            SpawnChargeParticles();
        }

        GoalTile g = MoveManager.ObjectInTile<GoalTile>(transform.position);
        if (g != null)
        {
            if (g.enemiesKilled)
            {
                SoundManager.Play(GameSettings.Instance.LevelEndSound);
                MoveManager.LoadLevel(g.NextLevel);
                return;
            }
        }
        if (ChargeBar.Instance.charges <= 0)
            Kill();

        bNotif.SetLevel(GetLevel(ChargeBar.Instance.charges));

        base.HandleMoveDone();
    }
    private BatteryNotification.PowerLevel GetLevel(int charges)
    {
        if (charges < 3)
            return BatteryNotification.PowerLevel.LOW;
        if (charges < 6)
            return BatteryNotification.PowerLevel.MED;
        return BatteryNotification.PowerLevel.HIGH;
    }
    private void SpawnChargeParticles()
    {
        for (int i = 0; i < GameSettings.Instance.ChargeParticles; i++)
        {
            GameObject chargeObj = Instantiate(GameSettings.Instance.ChargeParticlePrefab);
            Vector2 randDisp = UnityEngine.Random.insideUnitCircle * (GameSettings.Instance.DustDisp + .04f);
            chargeObj.transform.position = transform.position + GameSettings.Instance.DustStartDisp + new Vector3(randDisp.x, randDisp.y);

        }
        SoundManager.Play(GameSettings.Instance.ChargeSound);
    }


}
