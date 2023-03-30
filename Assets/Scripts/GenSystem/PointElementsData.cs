using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GenSystem
{
     [CreateAssetMenu(fileName = "New Point Elements", menuName = "New Point Elements Data", order = 52)]
    public class PointElementsData : ScriptableObject
    {
        public int size = 4;
        public int hight = 2;

        public GameObject FloorGround;
        public GameObject WallH;
        public GameObject WallV;
        public GameObject Roof;
        public PillarPack Pillar = new PillarPack();

        public List<GameObject> PathTypeH = new List<GameObject>();
        public List<GameObject> PathTypeV = new List<GameObject>();


        public List<GraphPoint> graphPoints = new List<GraphPoint>();

        [Serializable]
        public class PillarPack
        {
            public GameObject PillarUp;
            public GameObject PillarDown;
            public GameObject PillarMiddle1;
            public GameObject PillarMiddle2;
        }

        public void GeneratePointConnections()
        {
            graphPoints.Sort((p1, p2) => p1.transform.position.Comparer(p2.transform.position));

            var walls = new Vector3[]
            {
                Vector3.forward,
                Vector3.right,
                Vector3.left,
                Vector3.back,
            };
            var pillars = new Vector3[]
            {
                (Vector3.forward + Vector3.right) / 2,
                (Vector3.forward + Vector3.left) / 2,
                (Vector3.back + Vector3.right) / 2,
                (Vector3.back + Vector3.left) / 2,
            };
            List<Vector3> pillarsBlacklist = new List<Vector3>();
            foreach (var p in graphPoints)
            {
                UnityEngine.Object.DestroyImmediate(p.transform.GetChild(0).gameObject);

                FabricManager.InstantiateT(FloorGround, p.transform.position + Vector3.down * hight / 2, Quaternion.identity, p.transform);

                //gen walls
                foreach (var w in walls)
                {
                    var aboutPos = p.transform.position + w * size;
                    var wallPos = p.transform.position + w * size / 2;
                    var cons = p.connectElements.Where(c => c.GetConnect(p).transform.position == aboutPos);

                    if (cons.Any())
                    {
                        if (cons.First().connectIndex >= 0)
                        {
                            if (w.x != 0)
                                FabricManager.InstantiateT(PathTypeH[cons.First().connectIndex], wallPos, Quaternion.identity, p.transform);
                            if (w.z != 0)
                                FabricManager.InstantiateT(PathTypeV[cons.First().connectIndex], wallPos, Quaternion.identity, p.transform);
                        }
                        continue;
                    }
                    var notConnectedPoint = graphPoints.Where(p => p.transform.position == aboutPos);
                    if (notConnectedPoint.Any())
                    {
                        notConnectedPoint.First().Connect(p, -2);
                    }
                    if (w.x != 0)
                        FabricManager.InstantiateT(WallH, wallPos, Quaternion.identity, p.transform);
                    if (w.z != 0)
                        FabricManager.InstantiateT(WallV, wallPos, Quaternion.identity, p.transform);

                }

                //gen Pillars
                foreach (var pil in pillars)
                {
                    var pilPos = p.transform.position + pil * size;
                    if (pillarsBlacklist.Contains(pilPos))
                        continue;

                    pillarsBlacklist.Add(pilPos);
                    FabricManager.InstantiateT(Pillar.PillarDown, pilPos, Quaternion.identity, p.transform);
                }

            }
        }

    }
}