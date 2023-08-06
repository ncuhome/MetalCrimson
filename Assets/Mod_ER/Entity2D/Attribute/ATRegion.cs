using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 区域检测事件
    /// </summary>
    /// <param name="collision"></param>
    public delegate void DelRegion(Collision2D collision);

    /// <summary>
    /// 区域判定，对 collider 组件的封装
    /// </summary>
    public class ATRegion : MonoAttribute
    {
        #region 初始化

        public ATRegion()
        { AttributeName = nameof(ATRegion);}

        public override void Initialize()
        {
        }

        #endregion 初始化

        #region 属性

        /// <summary>
        /// 所属复合区域管理器
        /// </summary>
        private IRegionManager manager;

        /// <summary>
        /// 所属区域索引
        /// </summary>
        public int index;
        public enum ListType
        { Off, BlackList, WhiteList }

        /// <summary>
        /// 名单模式
        /// </summary>
        public ListType listType = ListType.Off;

        /// <summary>
        /// 标签名单
        /// </summary>
        public List<string> TagList = new();

        #endregion 属性

        #region 事件

        /// <summary>
        /// 区域进入接触事件
        /// </summary>
        public event DelRegion EnterEvent;

        /// <summary>
        /// 区域保持接触事件
        /// </summary>
        public event DelRegion StayEvent;

        /// <summary>
        /// 区域离开接触事件
        /// </summary>
        public event DelRegion ExitEvent;

        #endregion 事件

        #region 函数

        /// <summary>
        /// 设置所属复合管理器
        /// </summary>
        public void SetManager(IRegionManager manager, int index)
        { this.manager = manager; this.index = index; }

        #endregion 函数

        #region 区域检测
        protected void EnterAction(Collision2D collision) { if (EnterEvent != null) EnterEvent(collision); }
        protected void StayAction(Collision2D collision) { if (StayEvent != null) StayEvent(collision); }
        protected void ExitAction(Collision2D collision) { if (ExitEvent != null) ExitEvent(collision); }
        /// <summary>
        /// 标签检测
        /// </summary>
        /// <returns></returns>
        protected virtual bool TagJudge(string tag)
        {
            switch (listType)
            {
                case ListType.Off: return true;
                case ListType.BlackList:
                    if(TagList.Contains(tag))
                    {
                        return false;
                    }
                    return true;

                case ListType.WhiteList:
                    if (TagList.Contains(tag))
                    {
                        return true;
                    }
                    return false;

                default: return false;
            }
        }
        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if(TagJudge(collision.gameObject.tag))
            {
                EnterAction(collision);
                if (manager != null) manager.SetState(true, index,collision);
            }
        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            if (TagJudge(collision.gameObject.tag))
            {
                StayAction(collision);
                if (manager != null) manager.SetState(true, index,collision);
            }
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            if (TagJudge(collision.gameObject.tag))
            {
                ExitAction(collision);
                if (manager != null) manager.SetState(false, index,collision);
            }
        }

        #endregion 区域检测
    }
}