using UnityEngine;

namespace ER
{
    /// <summary>
    /// 场景过渡基类
    /// </summary>
    public abstract class SceneTransition:MonoBehaviour
    {
        /// <summary>
        /// 进度
        /// </summary>
        private float progress;
        /// <summary>
        /// 场景加载进度
        /// </summary>
        public virtual float Progress { get => progress; set => progress =value; }
        /// <summary>
        /// 加载场景(开始过渡)
        /// </summary>
        public abstract void EnterTransition();
        /// <summary>
        /// 离开场景(结束过渡)
        /// </summary>
        public abstract void ExitTransition();


    }
}