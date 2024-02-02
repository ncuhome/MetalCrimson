// Ignore Spelling: collider

using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 区域检测事件
    /// </summary>
    /// <param name="collision"></param>
    public delegate void DelRegion(Collider2D collider);

    /// <summary>
    /// 区域判定，对 collider 组件的封装
    /// </summary>
    public class ATRegion : MonoAttribute
    {
        #region 初始化

        public ATRegion()
        { AttributeName = nameof(ATRegion); }

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

        protected void EnterAction(Collider2D collider)
        { if (EnterEvent != null) EnterEvent(collider); }

        protected void StayAction(Collider2D collider)
        { if (StayEvent != null) StayEvent(collider); }

        protected void ExitAction(Collider2D collider)
        { if (ExitEvent != null) ExitEvent(collider); }

        /// <summary>
        /// 子类额外过程接口（进入）
        /// </summary>
        protected virtual void VirtualEnter(Collider2D collider)
        { }

        /// <summary>
        /// 子类额外过程接口（保持）
        /// </summary>
        protected virtual void VirtualStay(Collider2D collider)
        { }

        /// <summary>
        /// 子类额外过程接口（离开）
        /// </summary>
        protected virtual void VirtualExit(Collider2D collider)
        { }

        /// <summary>
        /// 条件检测(检测标签是否在白名单/黑名单内)
        /// </summary>
        /// <returns></returns>
        protected virtual bool Judge(Collider2D collider)
        {
            switch (listType)
            {
                case ListType.Off: return true;
                case ListType.BlackList:
                    if (TagList.Contains(collider.tag))
                    {
                        return false;
                    }
                    return true;

                case ListType.WhiteList:
                    if (TagList.Contains(collider.tag))
                    {
                        return true;
                    }
                    return false;

                default: return false;
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("collision接触:" + collision.gameObject.tag);
            if (Judge(collision.collider))
            {
                EnterAction(collision.collider);
                VirtualEnter(collision.collider);
                if (manager != null) manager.SetState(true, index, collision.collider);
            }
        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            if (Judge(collision.collider))
            {
                StayAction(collision.collider);
                VirtualStay(collision.collider);
                if (manager != null) manager.SetState(true, index, collision.collider);
            }
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            if (Judge(collision.collider))
            {
                ExitAction(collision.collider);
                VirtualExit(collision.collider);
                if (manager != null) manager.SetState(false, index, collision.collider);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            //Debug.Log("collider接触" + collider.gameObject.tag);
            if (Judge(collider))
            {
                EnterAction(collider);
                VirtualEnter(collider);
                if (manager != null) manager.SetState(true, index, collider);
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D collider)
        {
            if (Judge(collider))
            {
                StayAction(collider);
                VirtualStay(collider);
                if (manager != null) manager.SetState(true, index, collider);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collider)
        {
            if (Judge(collider))
            {
                ExitAction(collider);
                VirtualExit(collider);
                if (manager != null) manager.SetState(false, index, collider);
            }
        }

        #endregion 区域检测
    }
}