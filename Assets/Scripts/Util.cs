using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Util
    {
        public static IEnumerator SmoothMove2D(Vector2 origin, Vector2 target, float speed, int maxIterations, System.Action<Vector2> callback, System.Action finished = null)
        {
            int iterations = 0;
            while (origin != target)
            {
                origin = Vector2.Lerp(origin, target, speed * Time.deltaTime);

                if (++iterations > maxIterations)
                    origin = target;

                callback(origin);

                yield return null;
            }
            finished?.Invoke();
        }
    }
}
