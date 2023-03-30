using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GenSystem
{
    [ExecuteInEditMode]
    public class GenMain : MonoBehaviour
    {
        public GraphPoint point;
        public List<GraphElement> graphMap = new List<GraphElement>();
        [ContextMenu("Main")]
        public void Main()
        {
            if (graphMap.Count > 1)
                for (int i = 0; i < graphMap.Count; i++)
                {
                    if (i == 0)
                        graphMap[i].Init(null, graphMap[i + 1]);
                    else if (i == graphMap.Count - 1)
                        graphMap[i].Init(graphMap[i - 1], point);
                    else
                        graphMap[i].Init(graphMap[i - 1], graphMap[i + 1]);
                }
            else
                graphMap[0].Init(null, point);

            var root = FabricManager.InstantiateT(graphMap[0], transform.position, Quaternion.identity, transform);
            root.GenInternalStruct();

        }
        [ContextMenu("GenPointCon")]
        public void GenPointCon()
        {
            PrefsGraph.Instant.pointElements.GeneratePointConnections();
        }
        [ContextMenu("Clear")]
        public void Clear()
        {
            PrefsGraph.Instant.blacklistManager.blacklistpositions.Clear();
            PrefsGraph.Instant.pointElements.graphPoints.Clear();
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}