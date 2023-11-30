using System;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 环境检测器，检测地面和空中两种状态
    /// </summary>
    public class ATEnvironmentDetector : MonoAttribute
    {
        #region 相关

        /// <summary>
        /// 环境状态枚举
        /// </summary>
        public enum EnvironmentType
        { Land, Air };

        /// <summary>
        /// 当前环境状态
        /// </summary>
        public EnvironmentType Type { get => type; }

        #endregion 相关

        #region 初始化

        public ATEnvironmentDetector()
        { AttributeName = nameof(ATEnvironmentDetector); }

        public override void Initialize()
        {
            animator = owner.GetAttribute<ATAnimator>().Animator;//获取实体的动画器
            animator.GetInteger("env");//尝试获取动画控制变量
            if (region == null) { Debug.LogError("环境检测器缺少区域体"); }
            else
            {
                Debug.Log("环境检测器初始化...");
                region.touchEvent += () => { Land(); };
                region.notTouchEvent += () => { Air(); };
            }
        }

        #endregion 初始化

        #region 属性

        [SerializeField]
        [Tooltip("检测区域")]
        private ATButtonRegion region;

        [SerializeField]
        private EnvironmentType type = EnvironmentType.Land;

        /// <summary>
        /// 实体自身的动画器
        /// </summary>
        private Animator animator;

        #endregion 属性

        #region 事件

        public event Action OnLandEvent;

        public event Action OnAirEvent;

        #endregion 事件

        #region 功能

        private void Land()
        {
            type = EnvironmentType.Land;
            Debug.Log("着陆");
            animator.SetInteger("env", (int)type);
            if (OnLandEvent != null) { OnLandEvent(); }
        }

        private void Air()
        {
            type = EnvironmentType.Air;
            Debug.Log("悬空");
            animator.SetInteger("env", (int)type);
            if (OnAirEvent != null) { OnAirEvent(); }
        }

        #endregion 功能
    }
}