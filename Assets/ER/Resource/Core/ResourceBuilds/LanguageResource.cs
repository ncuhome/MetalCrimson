using Newtonsoft.Json;
using System.Collections.Generic;

namespace ER.Resource
{
    /// <summary>
    /// 显示文本资源
    /// 资源头: lang
    /// </summary>
    public class LanguageResource : IResource
    {
        private string registryName;
        public string RegistryName { get => registryName; }

        private Dictionary<string, Dictionary<string, string>> displayText;

        /// <summary>
        /// 文本键值对资源对象
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Value => displayText;

        /// <summary>
        /// 加载以 Text 的形式加载, 但是人需要一个单独的类来缓存这类资源
        /// </summary>
        /// <param name="_registryName"></param>
        /// <param name="origin"></param>
        public LanguageResource(string _registryName, string origin)
        {
            registryName = _registryName;
            displayText = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(origin);
        }

        /// <summary>
        /// 获取显示信息
        /// </summary>
        /// <param name="displayPath">显示路径</param>
        /// <returns></returns>
        public Dictionary<string, string> GetInfos(string displayPath)
        {
            if (displayText.TryGetValue(displayPath, out Dictionary<string, string> infos))
            {
                return infos;
            }
            return null;
        }
    }
}