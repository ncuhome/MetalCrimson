using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 环境检测器，检测地面和空中两种状态
    /// </summary>
    public class ATEnvironmentDetector : MonoAttribute
    {
        #region 初始化

        public ATEnvironmentDetector()
        { AttributeName = nameof(ATEnvironmentDetector); }

        public override void Initialize()
        {
            animator = owner.GetComponent<Animator>();//获取实体的动画器
            animator.GetInteger("env");//尝试获取动画控制变量
            if (region == null) { Debug.LogError("环境检测器缺少区域体"); }
            else
            {
                region.EnterEvent += Land;
                region.StayEvent += Land;
                region.ExitEvent += Air;
            }
        }

        #endregion 初始化

        #region 属性

        [SerializeField]
        [Tooltip("检测区域")]
        private ATRegion region;

        public enum EnvironmentType
        { Land, Air };

        private EnvironmentType type = EnvironmentType.Land;

        /// <summary>
        /// 当前环境状态
        /// </summary>
        public EnvironmentType Type { get => type; }

        /// <summary>
        /// 实体自身的动画器
        /// </summary>
        private Animator animator;

        #endregion 属性

        #region 功能

        private void Land(Collision2D collision)
        {
            type = EnvironmentType.Land;
            animator.SetInteger("env", (int)type);
        }

        private void Air(Collision2D collision)
        {
            type = EnvironmentType.Air;
            animator.SetInteger("env", (int)type);
        }

        #endregion 功能
    }
}