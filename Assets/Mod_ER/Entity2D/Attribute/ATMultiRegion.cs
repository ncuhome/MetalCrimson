using Common;
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
        public event Action<Collision2D> AndAllEvent;
        /// <summary>
        /// 存在激活时触发的事件
        /// </summary>
        public event Action<Collision2D> OrAllEvent;
        /// <summary>
        /// 全部非激活时触发的事件
        /// </summary>
        public event Action<Collision2D> NotAllEvent;
        #endregion

        #region 函数
        /// <summary>
        /// 设置区域状态
        /// </summary>
        /// <param name="state">区域状态</param>
        /// <param name="index">区域索引</param>
        public override void SetState(bool state,int index,Collision2D collision)
        {
            if(states.InRange(index))
            {
                states[index] = state;
                RegionAction(collision);
            }
        }
        /// <summary>
        /// 区域状态行为
        /// </summary>
        private void RegionAction(Collision2D collision)
        {
            if(states.AndAll())
            {
                if (AndAllEvent != null) AndAllEvent(collision);
            }
            else if(states.OrAll())
            {
                if (OrAllEvent != null) OrAllEvent(collision);
            }
            else
            {
                if (NotAllEvent != null) NotAllEvent(collision);
            }
        }
        #endregion
    }
}