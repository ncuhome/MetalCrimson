using System.Runtime.InteropServices;
using UnityEngine;
namespace ER.Entity2D
{
    /// <summary>
    /// 对角色动作的封装
    /// </summary>
    public class ATAction : MonoAttribute
    {

        #region 初始化
        public ATAction() { AttributeName = nameof(ATAction); }
        public override void Initialize()
        {
            
        }
        #endregion


        #region 属性
        /// <summary>
        /// 动作索引
        /// </summary>
        private int index;
        /// <summary>
        /// 动作有效区域（动作可以无有效区域，例如移动）
        /// </summary>
        private ATActionRegion region;
        /// <summary>
        /// 动作自身的动画机
        /// </summary>
        private Animator animator;
        #endregion

        #region 功能
        
        #endregion
    }
}