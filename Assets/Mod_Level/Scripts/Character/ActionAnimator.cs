using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 用于将 动画机关联 动作管理器, 辅助动画播放, 动画调用动作管理器的函数
    /// </summary>
    public class ActionAnimator:MonoBehaviour
    {
        [SerializeField]
        [Tooltip("动作管理器")]
        private ATActionManager manager;

        /// <summary>
        /// 停止指定动作
        /// </summary>
        /// <param name="index"></param>
        public void StopAction(string actionName)
        {
            manager.Stop(actionName);
            Debug.Log($"尝试停止动作:{actionName}");
        }
        /// <summary>
        /// 停止攻击, 并回到指定姿势
        /// </summary>
        public void StopAttack()
        {
            manager.Stop("Attack");
        }
        /// <summary>
        /// 调用动作的事件帧函数(第一个参数指定动作名称,第二个参数指定函数参数, 两个参数使用冒号分割)
        /// </summary>
        /// <param name="key"></param>
        public void ActionFunction(string key)
        {
            string[] _params = key.Split(':');
            manager.ActionFunction(_params[0], _params[1]);
        }
    }
}