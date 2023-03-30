using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GenSystem
{
    public class GraphRoom : GraphElement
    {
        public override void GenInternalStruct()
        {
            Vector3 currPos = transform.position;
            var rad = (size + 1) / 2 * subElement.SumSize;
            Vector3 center = transform.position + buildVector * rad;

            RecMethod(null, new NewWay(backElement, currPos, buildVector));

            rootElement = subElements.First();

            void RecMethod(GraphElement preElem, NewWay newWay)
            {
                GraphElement currElem = FabricManager.InstantiateT(subElement, newWay.position, Quaternion.identity, transform);
                subElements.Add(currElem);
                currElem.buildVector = newWay.vector;
                currElem.backElement = newWay.elemement;

                if (preElem != null)
                    currElem.Connect(preElem);

                currElem.GenInternalStruct();
                currElem.GenNewWays();

                for (int i = 0; i < currElem.newWays.Count; i++)
                {
                    var pos = currElem.newWays[i].position;

                    if (!prefsGraph.blacklistManager.CheckPos(pos))
                        continue;

                    if (!prefsGraph.blacklistManager.CheckArea(pos, currElem.newWays[i].vector, subElement.SumSize))
                        continue;

                    if (Vector3.Distance(pos, center) <= rad)
                        RecMethod(currElem, currElem.newWays[i]);
                }
            }
        }

        public override void GenNewWays()
        {
            List<NewWay> newWs = new List<NewWay>();
            foreach (var se in subElements)
            {
                if (se != null && se.newWays != null)
                    foreach (var nw in se.newWays)
                    {
                        if (!prefsGraph.blacklistManager.blacklistpositions.Contains(nw.position))
                            newWs.Add(nw);
                    }
            }
            newWays = newWs.Distinct().ToList();

            //newWays = subElements.
            //     Select(sE => sE.newWays).
            //     Aggregate((nw1, nw2) => (List<NewWay>)nw1.Union(nw2)).
            //     Where(nw => !prefsGraph.blacklistManager.blacklistpositions.Contains(nw.position)).
            //     Select(nw => nw).ToList();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}