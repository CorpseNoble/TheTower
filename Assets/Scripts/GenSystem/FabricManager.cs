using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


namespace Assets.Scripts.GenSystem
{
    public static class FabricManager
    {
        public static T InstantiateT<T>(T t, Vector3 position, Quaternion quaternion, Transform parent) where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate(t, position, quaternion, parent);
        }
    }

    public static class Extension
    {
        public static Vector3[] GetAboutPosition(this Vector3 pos)
        {
            return new Vector3[]
            {
                pos + Vector3.forward,
                pos + Vector3.left,
                pos + Vector3.right,
                pos + Vector3.back,
            };
        }
        public static Vector3[] GetAbout(this Vector3 pos)
        {
            return new Vector3[]
            {
                Vector3.forward,
                Vector3.left,
                Vector3.right,
                Vector3.back,
            };
        }
        public static Vector3 Abort(this Vector3 pos)
        {
            return pos.ToRight().ToRight();
        }
        public static Vector3 ToRight(this Vector3 vector)
        {
            if (vector == Vector3.forward) return Vector3.right;
            else if (vector == Vector3.right) return Vector3.back;
            else if (vector == Vector3.back) return Vector3.left;
            else if (vector == Vector3.left) return Vector3.forward;
            else return Vector3.forward;
        }
        /// <summary>
        /// set value vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 ToTheRight(this Vector3 vector)
        {
            vector = vector.ToRight();
            return vector;
        }
        public static Vector3 ToLeft(this Vector3 vector)
        {
            if (vector == Vector3.forward) return Vector3.left;
            else if (vector == Vector3.left) return Vector3.back;
            else if (vector == Vector3.back) return Vector3.right;
            else if (vector == Vector3.right) return Vector3.forward;
            else return Vector3.forward;
        }
        /// <summary>
        /// set value vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 ToTheLeft(this Vector3 vector)
        {
            vector = vector.ToLeft();
            return vector;
        }

        public static int Comparer(this Vector3 vector, Vector3 vector1)
        {
            int x = 0;

            if (vector1.x > vector.x)
                x += 1;
            else if (vector1.x < vector.x)
                x -= 1;

            if (vector1.y > vector.y)
                x += 2;
            else if (vector1.y < vector.y)
                x -= 2;

            if (vector1.z > vector.z)
                x += 3;
            else if (vector1.z < vector.z)
                x -= 3;

            if (x > 0) return 1;
            if (x < 0) return -1;

            return 0;
        }
    }
}
