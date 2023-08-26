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
        /// <param name="posture"></param>
        public void StopAttack(string posture)
        {
            switch (posture.ToLower())
            {
                case "up":
                    manager.Stop("Attack");
                    manager.Action("PostureUp");
                    break;
                case "down":
                    manager.Stop("Attack");
                    manager.Action("PostureDown");
                    break;
                default:
                    manager.Stop("Attack");
                    manager.Action("PostureFront");
                    break;
            }
        }
    }
}