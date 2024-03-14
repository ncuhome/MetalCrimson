namespace ER
{
    /// <summary>
    /// 游戏设置接口(建议使用json读取本地数据)
    /// 内部设置文件: ERinbone.DefSettingsAddress
    /// 外部设置文件: ERinbone.CustomSettinsgPath
    /// 注意游戏全局应当只有一个 Settings 对象, 并且确保注册了锚点, 锚点标签:"Settings"
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// 重新读取设置文本 (先内部再外部,外部设置覆盖内部设置)
        /// </summary>
        public void UpdateSettings();
        /// <summary>
        /// 获取指定设置内容
        /// </summary>
        /// <param name="registryName">设置片段名称</param>
        /// <returns></returns>
        public object GetSettings(string registryName);
        /// <summary>
        /// 设置指定片段的设置内容
        /// </summary>
        /// <param name="registryName"></param>
        /// <param name="settings"></param>
        public void SetSettings(string registryName, object settings);
        /// <summary>
        /// 保存当前设置信息(写入外部设置文件)
        /// </summary>
        public void Save();
    }
}