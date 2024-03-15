// Ignore Spelling: color

using UnityEngine;

namespace ER.Resource
{
    /// <summary>
    /// 资源接口
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// 注册名称: 仅在读取配置中修改
        /// </summary>
        public string RegistryName { get; }
    }

}