using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GenSystem
{
    public class GraphFork : GraphElement
    {
        public override void GenInternalStruct()
        {
            Vector3 currPos = transform.position;
            GraphElement currElem;


            currElem = FabricManager.InstantiateT(subElement, currPos, Quaternion.identity, transform);
            subElements.Add(currElem);

            currElem.backElement = backElement;

            currElem.GenInternalStruct();
            currElem.GenNewWays();

            RecMethod(currElem);

            rootElement = subElements.First();


        }
        void RecMethod(GraphElement preElem, int curDeep = 0)
        {
            curDeep++;

            List<Action> actions = new List<Action>();

            for (int i = 0; i < preElem.newWays.Count; i++)
            {
                var currPos = preElem.newWays[i].position;

                if (!prefsGraph.blacklistManager.CheckPos(currPos))
                    continue;

                if (!prefsGraph.blacklistManager.CheckArea(currPos, preElem.newWays[i].vector, subElement.SumSize))
                    continue;

                var currElem = FabricManager.InstantiateT(subElement, currPos, Quaternion.identity, transform);
                subElements.Add(currElem);

                currElem.buildVector = preElem.newWays[i].vector;
                currElem.backElement = preElem.newWays[i].elemement;
                currElem.Connect(preElem);

                currElem.GenInternalStruct();
                currElem.GenNewWays();

                if (curDeep < size)
                    actions.Add(() => RecMethod(currElem, curDeep));

            }

            foreach (var a in actions)
                a.Invoke();


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