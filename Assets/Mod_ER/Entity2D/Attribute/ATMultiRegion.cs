// Ignore Spelling: collider


using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ER.Entity2D
{

    /// <summary>
    /// 复合的区域检测管理器
    /// </summary>
    public class ATMultiRegion : ATBaseRegionManager
    {
        #region 初始化
        public ATMultiRegion() { AttributeName = nameof(ATMultiRegion); }
        #endregion

        #region 事件
        /// <summary>
        /// 全部激活时触发的事件
        /// </summary>
        public event Action<Collider2D> AndAllEvent;
        /// <summary>
        /// 存在激活时触发的事件
        /// </summary>
        public event Action<Collider2D> OrAllEvent;
        /// <summary>
        /// 全部非激活时触发的事件
        /// </summary>
        public event Action<Collider2D> NotAllEvent;
        #endregion

        #region 函数
        /// <summary>
        /// 设置区域状态
        /// </summary>
        /// <param name="state">区域状态</param>
        /// <param name="index">区域索引</param>
        public override void SetState(bool state,int index,Collider2D collider)
        {
            if(states.InRange(index))
            {
                states[index] = state;
                RegionAction(collider);
            }
        }
        /// <summary>
        /// 区域状态行为
        /// </summary>
        private void RegionAction(Collider2D collider)
        {
            if(states.AndAll())
            {
                if (AndAllEvent != null) AndAllEvent(collider);
            }
            else if(states.OrAll())
            {
                if (OrAllEvent != null) OrAllEvent(collider);
            }
            else
            {
                if (NotAllEvent != null) NotAllEvent(collider);
            }
        }
        #endregion
    }
}