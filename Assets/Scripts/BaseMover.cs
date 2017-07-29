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

        public void Update()
        {
            HandleUpdate();
        }

        public virtual void HandleUpdate()
        {

        }
        public abstract bool TryMove();

        protected void EaseToPos(Vector3 startPos, Vector3 newPos)
        {
            Vector3 disp = newPos - startPos;
            transform.position = startPos;
            StartCoroutine(EaseFunctions.GenericTween(EaseFunctions.Type.Linear, GameSettings.Instance.MoveTime, (t) =>
            {
                SpawnDirt();
                transform.position = startPos + disp * GameSettings.Instance.MoveCurve.Evaluate(t);
            }, null, () => { transform.position = newPos; HandleMoveDone(); }));
        }

        protected void EaseToAttack(Vector3 startPos, EnemyController e)
        {
            Vector3 newPos = e.transform.position;
            Vector3 disp = newPos - startPos;
            transform.position = startPos;
            bool killedEnemy = false;
            StartCoroutine(EaseFunctions.GenericTween(EaseFunctions.Type.Linear, GameSettings.Instance.MoveTime, (t) =>
            {
                SpawnDirt();
                transform.position = startPos + disp * GameSettings.Instance.AttackCurve.Evaluate(t);
                if (t > .5f && !killedEnemy)
                {
                    e.Kill();
                    killedEnemy = true;
                }
            }, null, () => { transform.position = startPos; HandleMoveDone(); }));
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

        protected virtual void HandleMoveDone()
        {

        }
    }
}
