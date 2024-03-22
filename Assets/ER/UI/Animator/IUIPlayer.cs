namespace ER.UI.Animator
{
    /// <summary>
    /// 动画播放器接口
    /// </summary>
    public interface IUIPlayer
    {
        /// <summary>
        /// 动画类型标签
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// 每帧被调用, 处理cd的动画播放
        /// </summary>
        /// <param name="cd"></param>
        /// <returns>返回该动画是否应该已结束</returns>
        public bool Update(UIAnimationCD cd,float deltaTime);
    }
}