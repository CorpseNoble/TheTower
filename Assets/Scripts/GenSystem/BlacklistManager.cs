using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GenSystem
{
    [Serializable]
    public class BlacklistManager
    {
        public List<Vector3> blacklistpositions = new List<Vector3>();

        /// <summary>
        /// Check: is empty pos?
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>
        /// True - free
        /// </returns>
        public bool CheckPos(Vector3 vector)
        {
            return !blacklistpositions.Contains(vector);
        }

        /// <summary>
        /// Check: is empty area?
        /// </summary>
        /// <param name="buildVector"></param>
        /// <returns>
        /// True - free
        /// </returns>
        public bool CheckArea(Vector3 pos, Vector3 buildVector, int sumSize)
        {
            var centr = pos + buildVector * sumSize / 2;
            var rad = Vector3.Distance(centr, pos);
            var currPos = pos + buildVector.ToLeft() * rad;
            //var stepDist = PrefsGraph.Instant.pointElements.size;

            for (int i = 0; i < sumSize; i++)
            {
                for (int j = 0; j < sumSize; j++)
                {
                    if (Vector3.Distance(currPos, centr) <= rad)
                    {
                        if (blacklistpositions.Contains(currPos))
                            return false;
                    }
                    currPos += buildVector.ToRight();
                }
                currPos += buildVector.ToLeft() * sumSize;
                currPos += buildVector;
            }

            return true;
        }

    }
}
