using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GenSystem
{
    public abstract class GraphElement : MonoBehaviour
    {
        [Header("GraphElement")]
        //gen string
        public int hight = 1;
        public int size = 1;
        public GraphElement subElement;
        public PrefsGraph prefsGraph = PrefsGraph.Instant;

        public virtual int SumSize
        {
            get
            {
                return size * (subElement == null ? 1 : subElement.SumSize);
            }
        }

        //internal struct
        public GraphElement rootElement;
        public GraphElement backElement;
        public List<GraphElement> subElements = new List<GraphElement>();
        public List<NewWay> newWays = new List<NewWay>();

        //external struct
        public Vector3 buildVector = Vector3.forward;
        public GraphElement parentElement;
        public List<Connect> connectElements = new List<Connect>();
        public GraphElement preElement;

        public void Init(GraphElement parentElement, GraphElement subElement)
        {
            this.parentElement = parentElement;
            this.subElement = subElement;
        }

        public abstract void GenInternalStruct();

        public virtual void GenNewWays()
        {
            foreach (var sE in subElements)
            {
                foreach (var nW in sE.newWays)
                {
                    if (prefsGraph.blacklistManager.blacklistpositions.Contains(nW.position))
                        continue;

                    newWays.Add(nW);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="connectIndex">
        /// -1 : Free Path
        /// -2 : Wall
        /// >=0 : indexTypePath
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Connect Connect(GraphElement graph, int connectIndex = -1)
        {
            if (this == graph)
                throw new Exception("Connect yourself");

            var connect = new Connect(this, graph);

            if (connectElements.Contains(connect))
                return connect;

            connectElements.Add(connect);
            graph.connectElements.Add(connect);

            connect.connectIndex = connectIndex;
            return connect;
        }
    }
    [Serializable]
    public struct NewWay
    {
        public GraphElement elemement;
        public Vector3 position;
        public Vector3 vector;
        public NewWay(GraphElement elemement, Vector3 position, Vector3 vector) : this()
        {
            this.elemement = elemement;
            this.position = position;
            this.vector = vector;
        }
        public void Deconstruct(out GraphElement elem, out Vector3 pos, out Vector3 forw)
        {
            elem = elemement;
            pos = position;
            forw = vector;
        }
    }

    [Serializable]
    public class Connect
    {
        public GraphElement[] Elements;
        public bool instanted = false;

        /// <summary>
        /// -1 : Free Path
        /// -2 : Wall
        /// </summary>
        public int connectIndex = 0;

        public Connect lowConnect = null;
        public Connect highConnect = null;
        public Connect(GraphElement elem1, GraphElement elem2)
        {
            Elements = new GraphElement[] { elem1, elem2 };
        }
        public GraphElement GetConnect(GraphElement element)
        {
            if (!Elements.Contains(element))
                throw new Exception("not connected element");

            if (Elements[0] == element)
                return Elements[1];
            else
                return Elements[0];
        }
        public bool Equals(Connect con)
        {
            return (con.Elements[0] == Elements[0] || con.Elements[1] == Elements[0])
                && (con.Elements[0] == Elements[1] || con.Elements[1] == Elements[1]);
        }

    }

}
