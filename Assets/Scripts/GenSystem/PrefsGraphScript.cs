using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GenSystem
{
    [ExecuteInEditMode]
    public class PrefsGraphScript : MonoBehaviour
    {
        public PrefsGraph prefsGraph = PrefsGraph.Instant;
    }

    [Serializable]
    public class PrefsGraph
    {
        public PointElementsData pointElements;
        public BlacklistManager blacklistManager;
        public static PrefsGraph Instant
        {
            get
            {
                instant ??= new PrefsGraph();

                return instant;
            }
        }
        private static PrefsGraph instant;

        private PrefsGraph() { }
    }
}
