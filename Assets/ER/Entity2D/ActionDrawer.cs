using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 动作区域绘制工具
    /// </summary>
    public class ActionDrawer : MonoBehaviour
    {
        /// <summary>
        /// 使用的材质
        /// </summary>
        public Material material;

        /// <summary>
        /// 相关点
        /// </summary>
        public List<Transform> points;

        /// <summary>
        /// 检测区域
        /// </summary>
        public PolygonCollider2D cld;

        private MeshRenderer mRenderer;
        private MeshFilter mFilter;

        private Vector2[] GetPoints()
        {
            Vector2[] array = new Vector2[points.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = points[i].localPosition;
            }
            return array;
        }

        [ContextMenu("重设判定区域形状")]
        public void UpdateShape()
        {
            cld.points = GetPoints();
            Draw();
        }

        [ContextMenu("重新获取区域设定点")]
        public void UpdateSetPoints()
        {
            points.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                points.Add(transform.GetChild(i));
            }
            UpdateShape();
        }

        [ContextMenu("绘制图像")]
        public void Draw()
        {
            Vector2[] vertices2D = new Vector2[points.Count];
            Vector3[] vertices3D = new Vector3[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 vertice = points[i].localPosition;
                vertices2D[i] = new Vector2(vertice.x, vertice.y);
                vertices3D[i] = vertice;
            }

            Triangulator tr = new Triangulator(vertices2D);
            int[] triangles = tr.Triangulate();

            Mesh mesh = new Mesh();
            mesh.vertices = vertices3D;
            mesh.triangles = triangles;

            if (mRenderer == null)
            {
                mRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
            }
            mRenderer.material = material;
            if (mFilter == null)
            {
                mFilter = gameObject.GetOrAddComponent<MeshFilter>();
            }
            mFilter.mesh = mesh;
        }

        #region Unity

        private void Start()
        {
            UpdateShape();
            Draw();
        }

        public void Update()
        {
            UpdateSetPoints();
        }

        #endregion Unity
    }
}