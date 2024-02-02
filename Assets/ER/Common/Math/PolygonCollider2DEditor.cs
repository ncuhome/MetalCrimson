// Ignore Spelling: Collider cld

using System.Collections.Generic;
using UnityEngine;

namespace ER
{
    public class PolygonCollider2DEditor : MonoBehaviour
    {
        public List<Transform> points;
        public PolygonCollider2D cld;

        private void Start()
        {
            UpdateShape();
        }

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
        }

        [ContextMenu("重新获取区域设定点")]
        public void UpdateSetPoints()
        {
            points.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                points.Add(transform.GetChild(i));
            }
        }
    }
}