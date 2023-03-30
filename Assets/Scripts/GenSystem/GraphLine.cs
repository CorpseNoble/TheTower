using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GenSystem
{
    public class GraphLine : GraphElement
    {
        public override void GenInternalStruct()
        {
            Vector3 currPos = transform.position;
            GraphElement currElem = null;
            GraphElement preElem = null;

            for (int i = 0; i < size + 1; i++)
            {
                currElem = FabricManager.InstantiateT(subElement, currPos, Quaternion.identity, transform);
                subElements.Add(currElem);

                if (preElem != null)
                {
                    currElem.Connect(preElem);
                    currElem.backElement = preElem.newWays.Where(nW => nW.vector == buildVector).Select(v=>v.elemement).FirstOrDefault();
                }
                else
                {
                    currElem.backElement = backElement;
                }

                currElem.GenInternalStruct();
                currElem.GenNewWays();

                currPos = currElem.newWays.Where(nW => nW.vector == buildVector).Select(v => v.position).FirstOrDefault();

                if (!prefsGraph.blacklistManager.CheckPos(currPos))
                    break;

                if (!prefsGraph.blacklistManager.CheckArea(currPos, buildVector, subElement.SumSize))
                    break;

                preElem = currElem;
            }

            rootElement = subElements.First();
        }

        public override void GenNewWays()
        {
            var elem = subElements.Last();
            foreach (var nW in elem.newWays)
            {
                if (!prefsGraph.blacklistManager.blacklistpositions.Contains(nW.position))
                    newWays.Add(nW);
            }
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