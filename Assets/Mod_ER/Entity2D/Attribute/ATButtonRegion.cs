// Ignore Spelling: collider

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 特殊的Collider封装，提供对接触者收集判断的功能
    /// </summary>
    public class ATButtonRegion : ATRegion
    {
        #region 属性

        [SerializeField]
        protected List<GameObject> record = new List<GameObject>();

        #endregion 属性

        #region 事件

        /// <summary>
        /// 当接触区域存在白名单物体时触发(仅在检测物体数量发生改变时触发)
        /// </summary>
        public event Action touchEvent;

        /// <summary>
        /// 当接触区域不存在白名单物体时触发(仅在检测物体数量发生改变时触发)
        /// </summary>
        public event Action notTouchEvent;

        #endregion 事件

        protected override void VirtualEnter(Collider2D collider)
        {
            if (!record.Contains(collider.gameObject))
            {
                if (record.Count == 0)
                {
                    if (touchEvent != null) touchEvent();
                }
                record.Add(collider.gameObject);
                Debug.Log($"进入:{collider.gameObject.name}, tag:{collider.tag}");
            }
        }

        protected override void VirtualStay(Collider2D collider)
        {
            if (!record.Contains(collider.gameObject))
            {
                if (record.Count == 0)
                {
                    if (touchEvent != null) touchEvent();
                }
                record.Add(collider.gameObject);
                Debug.Log($"进入:{collider.gameObject.name}, tag:{collider.tag}");
            }
        }

        protected override void VirtualExit(Collider2D collider)
        {
            if (record.Contains(collider.gameObject))
            {
                record.Remove(collider.gameObject);
                if (record.Count == 0)
                {
                    if (notTouchEvent != null) notTouchEvent();
                }
                Debug.Log($"离开:{collider.gameObject.name}, tag:{collider.tag}");
            }
        }
    }
}