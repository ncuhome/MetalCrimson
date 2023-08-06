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
        public MDAction() { actionName = "Unkown"; }
        /// <summary>
        /// 动作被加载进 ATActionManager 后的初始化
        /// </summary>
        public abstract void Initialize();
        #endregion


        #region 属性

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
        #endregion

        #region 功能
        #endregion
    }
}