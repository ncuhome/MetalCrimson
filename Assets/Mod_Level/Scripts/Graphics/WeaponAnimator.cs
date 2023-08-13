using UnityEngine;
using UnityEngine.UIElements;

namespace Mod_Level
{
    /// <summary>
    /// 同步武器动画
    /// </summary>
    public class WeaponAnimator : MonoBehaviour
    {
        /// <summary>
        /// 位置参考基点
        /// </summary>
        public Transform PointA;

        /// <summary>
        /// 方向参考基点
        /// </summary>
        public Transform PointB;

        /// <summary>
        /// 武器贴图
        /// </summary>
        public Transform Image;

        /// <summary>
        /// 默认AB向量
        /// </summary>
        private Vector2 DefAB;

        /// <summary>
        /// 相对角度
        /// </summary>
        private float offsetAngle;

        /// <summary>
        /// 相对长度
        /// </summary>
        private float offsetLength;

        public Vector2 offsetNow;

        public float angleNow;

        private void Awake()
        {
            DefAB = PointB.localPosition - PointA.localPosition;
            Vector2 offset = Image.localPosition - PointA.localPosition;
            offsetAngle = Vector2.Angle(DefAB, offset);
            offsetLength = offset.magnitude;
            Debug.Log($"angle:{offsetAngle},length:{offsetLength}");
        }

        private void Update()
        {
            Vector2 AB = PointB.localPosition - PointA.localPosition;//方向向量
            float alpha = Vector2.Angle(new Vector2(1, 0), AB);
            float bata = (offsetAngle + alpha)/180*Mathf.PI;
            offsetNow = new Vector2(Mathf.Cos(bata), Mathf.Sin(bata)) * offsetLength;
            Debug.DrawLine(PointA.transform.position,(Vector2)PointA.transform.position+offsetNow);

            Image.localPosition = new Vector3(offsetNow.x + PointA.localPosition.x, offsetNow.y + PointA.localPosition.y,Image.localPosition.z);//确定新的P点坐标
            
            if(Vector2.Angle(AB,Vector2.right) > Vector2.Angle(DefAB,Vector2.right))
            {
                angleNow = Vector2.Angle(DefAB, AB);
            }
            else
            {
                angleNow = -Vector2.Angle(DefAB, AB);
            }
            
            Image.eulerAngles = new Vector3(0, 0, angleNow);
            
        }
    }
}