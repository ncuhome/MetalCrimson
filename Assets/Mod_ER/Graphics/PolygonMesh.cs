using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ER.Graphics
{
    /// <summary>
    /// 未完成
    /// </summary>
    public class PolygonMesh:MonoBehaviour
    {
        /// <summary>
        /// 多边形点
        /// </summary>
        public List<Transform> points;

        public MeshFilter filter;

        private Mesh mesh;

        private void Start()
        {
            UpdatePoints();
        }
        private Vector3[] GetPoints()
        {
            Vector3[] array = new Vector3[points.Count];
            for(int i=0;i<array.Length;i++)
            {
                array[i] = points[i].position;
            }
            return array;
        }
        [ContextMenu("重设位置")]
        public void UpdatePoints()
        {
            mesh = filter.mesh;
            if (mesh == null) mesh = new Mesh();
            mesh.vertices = GetPoints();
            filter.mesh = mesh;
        }
    }
}