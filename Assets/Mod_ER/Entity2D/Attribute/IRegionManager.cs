using Common;
using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{

    public interface IRegionManager
    {
        public void SetState(bool state,int index,Collision2D collision);
    }

    public class ATBaseRegionManager : MonoAttribute, IRegionManager
    {

        #region 初始化
        public ATBaseRegionManager() { AttributeName = nameof(ATBaseRegionManager); }

        public override void Initialize()
        {
            if (Regions != null && Regions.Count > 0)
            {
                foreach (var region in Regions)
                {
                    AddRegion(region);
                }
            }
        }
        #endregion

        #region 属性
        [SerializeField]
        [Tooltip("区域检测初始化表")]
        public List<ATRegion> Regions = new();

        /// <summary>
        /// 区域状态
        /// </summary>
        protected List<bool> states = new();
        /// <summary>
        /// 检测区域
        /// </summary>
        protected List<ATRegion> regions = new();
        /// <summary>
        /// 标签名单模式
        /// </summary>
        public ATRegion.ListType listType = ATRegion.ListType.Off;
        /// <summary>
        /// 标签名单
        /// </summary>
        public List<string> TagList = new();
        #endregion

        #region 函数
        /// <summary>
        /// 添加新的检测区域
        /// </summary>
        /// <param name="region"></param>
        public virtual void AddRegion(ATRegion region)
        {
            region.SetManager(this, regions.Count);
            regions.Add(region);
            if (listType != ATRegion.ListType.Off)
            {
                region.listType = listType;
                region.TagList = TagList;
            }
        }
        /// <summary>
        /// 移除指定区域
        /// </summary>
        /// <param name="index">区域索引</param>
        /// <param name="destroy">是否销毁该区域</param>
        public virtual void RemoveRegion(int index, bool destroy = false)
        {
            if (regions.InRange(index))
            {
                ATRegion region = regions[index];
                regions.Remove(region);
                if (destroy) region.Destroy();
                for (int i = index; i < regions.Count; i++)
                {
                    regions[i].index = i;
                }
            }
        }
        /// <summary>
        /// 设置区域状态
        /// </summary>
        /// <param name="state">区域状态</param>
        /// <param name="index">区域索引</param>
        public virtual void SetState(bool state, int index, Collision2D collision)
        {
            if (states.InRange(index))
            {
                states[index] = state;
            }
        }
        #endregion
    }
}