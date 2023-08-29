// Ignore Spelling: vertices

using UnityEngine;

namespace ER
{
    /// <summary>
    /// 绘制多边形
    /// TODO: 存在两点重合时绘制有问题
    /// </summary>
    public class PolygonDrawer : MonoBehaviour
    {
        public Material material;
        public Transform[] vertices;
        private MeshRenderer mRenderer;
        private MeshFilter mFilter;

        private void Start()
        {
            Draw();
        }

        private void Update()
        {
            Draw();
        }

        [ContextMenu("重新获取区域设定点")]
        public void UpdateSetPoints()
        {
            vertices = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                vertices[i] = transform.GetChild(i);
            }
        }

        [ContextMenu("绘制图像")]
        public void Draw()
        {
            Vector2[] vertices2D = new Vector2[vertices.Length];
            Vector3[] vertices3D = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vertice = vertices[i].localPosition;
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

        private void OnValidate()
        {
            UpdateSetPoints();
        }
    }
}