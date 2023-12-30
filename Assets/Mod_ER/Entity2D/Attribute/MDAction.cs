using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 对角色动作的封装，隶属于 ATActionManager 下
    /// </summary>
    public abstract class MDAction : MonoBehaviour
    {
        /// <summary>
        /// 控制类型
        /// </summary>
        public enum ControlType
        { Bool, Trigger }

        /// <summary>
        /// 动作阶段
        /// </summary>
        public enum ActionState
        { Disable,Waiting, Acting, Stoped }

        #region 初始化

        public MDAction()
        { actionName = "Unknown"; }

        /// <summary>
        /// 动作被加载进 ATActionManager 后的初始化
        /// </summary>
        public abstract void Initialize();

        #endregion 初始化

        #region 属性

        /// <summary>
        /// 动作活动状态
        /// </summary>
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
        /// 控制类型
        /// </summary>
        public ControlType controlType;

        [SerializeField]
        protected ActionState state;

        /// <summary>
        /// 动作状态
        /// </summary>
        public ActionState State => state;

        #endregion 属性

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
            manager.ActionBuffer();
        }

        /// <summary>
        /// 中断该动作
        /// </summary>
        public void BreakACT(params string[] keys)
        {
            acting = false;
            BreakAction(keys);
            manager.ActionBuffer();
        }

        /// <summary>
        /// 开启活动
        /// </summary>
        protected abstract void StartAction(params string[] keys);

        /// <summary>
        /// 停止活动
        /// </summary>
        protected abstract void StopAction(params string[] keys);

        /// <summary>
        /// 中断活动
        /// </summary>
        /// <param name="keys"></param>
        protected abstract void BreakAction(params string[] keys);

        /// <summary>
        /// 动作函数(由动画控制器调用, 作为动画帧事件)
        /// </summary>
        /// <param name="keys"></param>
        public virtual void ActionFunction(string key)
        {
        }

        #endregion 功能
    }
}