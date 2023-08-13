using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 锚点管理类（非组件单例模式）
    /// </summary>
    public class AnchorManager: Singleton<AnchorManager>
    {
        #region 属性
        /// <summary>
        /// 锚点列表
        /// </summary>
        private Dictionary<string,Anchor> anchors = new Dictionary<string, Anchor>();
        #endregion

        #region 功能函数
        /// <summary>
        /// 添加锚点
        /// </summary>
        /// <param name="anchor">锚点对象</param>
        public void AddAnchor(Anchor anchor)
        {
            if(anchors.ContainsKey(anchor.AnchorTag))
            {
                anchors[anchor.AnchorTag] = anchor;
            }
            else
            {
                anchors.Add(anchor.AnchorTag, anchor);
            }
        }
        /// <summary>
        /// 移动锚点位置，若锚点不存在则新建锚点
        /// </summary>
        /// <param name="tag">锚点标签</param>
        /// <param name="x">锚点x位置</param>
        /// <param name="y">锚点y位置</param>
        public void MoveAnchor(string tag, int x,int y)
        {
            if (!anchors.Keys.Contains(tag))
            {
                VirtualAnchor anchor = new VirtualAnchor(tag,x,y);
                anchors.Add(tag, anchor);
                return;
            }
            anchors[tag].Point = new Vector2(x, y);
        }
        /// <summary>
        /// 移动锚点位置，若锚点不存在则新建锚点
        /// </summary>
        /// <param name="tag">锚点标签</param>
        /// <param name="position">锚点位置</param>
        public void MoveAnchor(string tag, Vector2 position)
        {
            if(!anchors.Keys.Contains(tag))
            {
                VirtualAnchor anchor = new VirtualAnchor(tag, position.x, position.y);
                anchors.Add(tag, anchor);
                return;
            }
            anchors[tag].Point = position;
        }
        /// <summary>
        /// 删除指定锚点
        /// </summary>
        /// <param name="tag">锚点标签</param>
        /// <returns>操作是否成功</returns>
        public bool DeleteAnchor(string tag)
        {
            if(anchors.Keys.Contains(tag))
            {
                anchors[tag].Destroy();
                anchors.Remove(tag);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取指定锚点位置，若锚点不存在则返回null
        /// </summary>
        /// <param name="tag">锚点标签</param>
        /// <returns></returns>
        public Vector2? GetAnchorPoint(string tag)
        {
            if(anchors.Keys.Contains(tag))
            {
                return anchors[tag].Point;
            }
            return null;
        }
        /// <summary>
        /// 获取锚点对象
        /// </summary>
        /// <returns></returns>
        public Anchor GetAnchor(string tag)
        {
            if (anchors.Keys.Contains(tag))
            {
                return anchors[tag];
            }
            return null;
        }
        #endregion
    }
}
