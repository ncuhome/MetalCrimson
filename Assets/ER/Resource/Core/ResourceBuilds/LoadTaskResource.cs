using Newtonsoft.Json;
using UnityEngine;

namespace ER.Resource
{
    /// <summary>
    /// 音频资源
    /// </summary>
    public class LoadTaskResource : IResource
    {
        private string registryName;
        private LoadTask task;
        public string RegistryName { get => registryName; }
        /// <summary>
        /// AudioClip 资源对象
        /// </summary>
        public LoadTask Value => task;

        public LoadTaskResource(string _registryName, string json)
        {
            registryName = _registryName;
            task = JsonConvert.DeserializeObject<LoadTask>(json);
        }
    }
}