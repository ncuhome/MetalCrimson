// Ignore Spelling: collider

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 特殊的Collider封装，提供对接触者收集判断的功能
    /// </summary>
    public class ATButtonRegion:ATRegion
    {
        #region 属性
        [SerializeField]
        private List<GameObject> record = new List<GameObject>();
        #endregion

        #region 事件
        public event Action touchEvent;
        public event Action notTouchEvent;
        #endregion

        protected override void VirtualEnter(Collider2D collider)
        {

            if(!record.Contains(collider.gameObject))
            {
                if(record.Count == 0)
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