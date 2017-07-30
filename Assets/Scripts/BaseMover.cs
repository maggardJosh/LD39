using Assets.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseMover : MonoBehaviour
    {
        public BaseMover thingToKill;

        public void Update()
        {
            HandleUpdate();
        }

        public virtual void HandleUpdate()
        {

        }
        public abstract bool TryMove(bool firstTime = true);

        public void Awake()
        {
            HandleAwake();
        }
        public virtual void HandleAwake()
        {

        }
        public void FixedUpdate()
        {
            if (isMoving)
                SpawnDirt();
        }
        private bool isMoving = false;
        protected void EaseToPos(Vector3 startPos, Vector3 newPos)
        {
            Vector3 disp = newPos - startPos;
            transform.position = startPos;
            isMoving = true;
            StartCoroutine(EaseFunctions.GenericTween(EaseFunctions.Type.Linear, GameSettings.Instance.MoveTime, (t) =>
            {
                transform.position = startPos + disp * GameSettings.Instance.MoveCurve.Evaluate(t);
            }, null, () => { transform.position = newPos; HandleMoveDone(); }));
        }
        Follow followScript;
        protected void EaseToAttack(Vector3 startPos, BaseMover e)
        {
            thingToKill = e;
            Vector3 newPos = e.transform.position;
            Vector3 disp = newPos - startPos;
            transform.position = startPos;
            isMoving = true;
            if (this is PlayerController)
            {
                followScript = FindObjectOfType<Follow>();
                if (followScript != null)
                    followScript.enabled = false;
            }
            bool hasKilled = false;
            StartCoroutine(EaseFunctions.GenericTween(EaseFunctions.Type.Linear, GameSettings.Instance.AttackTime, (t) =>
            {
                transform.position = startPos + disp * GameSettings.Instance.AttackCurve.Evaluate(t);
                if(!hasKilled && t > .5f)
                {
                    hasKilled = true;

                    if (thingToKill != null)
                        thingToKill.Kill();
                }

            }, null, () =>
            {
                if (followScript != null)
                    followScript.enabled = true;
                transform.position = startPos;
                HandleMoveDone();
            }));
        }

        private void SpawnDirt()
        {
            if (UnityEngine.Random.value <= GameSettings.Instance.DustParticleChance)
            {
                GameObject dustObj = Instantiate(GameSettings.Instance.DustPrefab);
                Vector2 randDisp = UnityEngine.Random.insideUnitCircle * GameSettings.Instance.DustDisp;
                dustObj.transform.position = transform.position + GameSettings.Instance.DustStartDisp + new Vector3(randDisp.x, randDisp.y);

            }
        }

        public void OnDestroy()
        {
            if (GameSettings.IsShuttingDown || !MoveManager.Instance.ShouldSpawnParticlesOnDeath)
                return;
            for (int i = 0; i < 30; i++)
            {
                GameObject dustObj = Instantiate(GameSettings.Instance.DustPrefab);
                Vector2 randDisp = UnityEngine.Random.insideUnitCircle * (GameSettings.Instance.DustDisp + .1f);
                dustObj.transform.position = transform.position + GameSettings.Instance.DustStartDisp + new Vector3(randDisp.x, randDisp.y);

            }
        }

        public virtual void Kill()
        {
            if (this is PlayerController)
                MoveManager.ReloadLevel();


            Destroy(gameObject);
        }

        protected virtual void HandleMoveDone()
        {
            thingToKill = null;
            isMoving = false;
        }
    }
}
