using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.GenSystem
{
    public class GraphPoint : GraphElement
    {
        public override int SumSize => prefsGraph.pointElements.size;

        public override void GenInternalStruct()
        {

            if (prefsGraph.blacklistManager.blacklistpositions.Contains(transform.position))
            {
                Debug.Log($"blacklistpositions.Contains({transform.position})");

                return;
            }
            if (backElement != null)
            {
                Connect(backElement);
            }
            size = prefsGraph.pointElements.size;
            PrefsGraph.Instant.pointElements.graphPoints.Add(this);
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = transform.position;
            cube.transform.localScale *= size;
            cube.transform.parent = transform;
            prefsGraph.blacklistManager.blacklistpositions.Add(transform.position);
        }

        public override void GenNewWays()
        {
            var about = new Vector3().GetAbout();
            foreach (var a in about)
            {
                var newpos = transform.position + a * size;

                if (prefsGraph.blacklistManager.blacklistpositions.Contains(newpos))
                    continue;

                newWays.Add(new NewWay(this, newpos, a));
            }
        }

    }
}