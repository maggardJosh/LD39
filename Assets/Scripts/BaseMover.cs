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
            StartCoroutine(EaseFunctions.GenericTween(EaseFunctions.Type.Linear, GameSettings.Instance.MoveTime, (t) => { transform.position = startPos + disp * GameSettings.Instance.MoveCurve.Evaluate(t); }, null, () => { transform.position = newPos; HandleMoveDone(); }));
        }

        protected virtual void HandleMoveDone()
        {

        }
    }
}
