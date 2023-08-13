using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity
{
    /// <summary>
    /// 预测接触事件
    /// </summary>
    public delegate void TouchLandEvent();

    /// <summary>
    /// 虚拟碰撞预测
    /// </summary>
    public class TouchLand : MonoBehaviour
    {
        #region 事件
        /// <summary>
        /// 接触事件委托
        /// </summary>
        public event TouchLandEvent TouchEvent;
        /// <summary>
        /// 未接触 事件委托
        /// </summary>
        public event TouchLandEvent UntouchLandEvent;
        #endregion

        #region 属性
        public bool runing = true;
        private List<string> touchTags = new List<string>();//检测标签
        private List<object> lands = new List<object>();
        /// <summary>
        /// 需检测的物体标签
        /// </summary>
        /// <returns></returns>
        public List<string> TouchTags
        {
            get => touchTags;
        }
        #endregion

        #region 触发监听
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (runing)
            {
                if (touchTags.Contains(collision.tag))//检测物体为障碍物
                {
                    if (!lands.Contains(collision.gameObject))
                    {
                        lands.Add(collision.gameObject);
                    }
                    if (TouchEvent != null) { TouchEvent(); }
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (runing)
            {
                if (touchTags.Contains(collision.tag))//检测物体为障碍物
                {
                    if (lands.Contains(collision.gameObject))
                    {
                        lands.Remove(collision.gameObject);
                    }
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (runing)
            {
                if (touchTags.Contains(collision.tag))//检测物体为障碍物
                {
                    if (!lands.Contains(collision.gameObject))
                    {
                        lands.Add(collision.gameObject);
                    }
                    if (TouchEvent != null) { TouchEvent(); }
                }
            }
        }
        #endregion

        #region Unity
        private void Start()
        {
            touchTags.Add("Barrier");
        }
        private void Update()
        {
            if (lands.Count == 0)
            {
                if (UntouchLandEvent != null) { UntouchLandEvent(); }
            }
        }
        #endregion
    }
}
