using System.Runtime.InteropServices;
using UnityEngine;
namespace ER.Entity2D
{
    /// <summary>
    /// 对角色动作的封装，隶属于 ATActionManager 下
    /// </summary>
    public abstract class MDAction:MonoBehaviour
    {

        #region 初始化
        public MDAction() { actionName = "Unknown"; }
        /// <summary>
        /// 动作被加载进 ATActionManager 后的初始化
        /// </summary>
        public abstract void Initialize();
        #endregion


        #region 属性
        public bool acting;
        /// <summary>
        /// 动作名称
        /// </summary>
        public string actionName;
        /// <summary>
        /// 所属动作集合对象
        /// </summary>
        public ATActionManager manager;
        /// <summary>
        /// 动作索引
        /// </summary>
        public int index;
        /// <summary>
        /// 所在动画层名称(动作管理器将根据此名称修改 相应的动画参数, 动画参数名称 = "act_"+"动画层名称")
        /// </summary>
        public string layer;
        #endregion

        #region 功能
        /// <summary>
        /// 判断当前条件是否满足动作执行
        /// </summary>
        /// <returns></returns>
        public abstract bool ActionJudge();
        /// <summary>
        /// 启用这个动作
        /// </summary>
        /// <param name="keys"></param>
        public void StartACT(params string[] keys)
        {
            acting = true;
            StartAction(keys);
        }
        /// <summary>
        /// 停止这个动作
        /// </summary>
        /// <param name="keys"></param>
        public void StopACT(params string[] keys)
        {
            acting = false;
            StopAction(keys);
        }

        /// <summary>
        /// 开启活动
        /// </summary>
        protected abstract void StartAction(params string[] keys);
        /// <summary>
        /// 停止活动
        /// </summary>
        protected abstract void StopAction(params string[] keys);
        #endregion
    }
}