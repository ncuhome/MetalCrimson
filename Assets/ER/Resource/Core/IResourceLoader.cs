// Ignore Spelling: Res

using System;
using UnityEditor.AddressableAssets.HostingServices;

namespace ER.Resource
{
    /// <summary>
    /// 资源加载接口, 负责一种资源的加载和缓存
    /// </summary>
    public interface IResourceLoader
    {
        /// <summary>
        /// 资源头
        /// </summary>
        public string Head { get; set; }
        /// <summary>
        /// 加载资源(带有检测避免重复加载)
        /// </summary>
        /// <param name="registryName"></param>
        /// <param name="callback"></param>
        public void ELoad(string registryName, Action callback);
        /// <summary>
        /// 缓存资源
        /// </summary>
        /// <param name="registryName">资源名</param>
        /// <param name="callback">加载完毕后的回调</param>
        public void Load(string registryName, Action  callback);
        /// <summary>
        /// 卸载所有资源缓存, 但是无法卸载 LoadForce 加载的资源
        /// </summary>
        public void Clear();
        /// <summary>
        /// 卸载资源
        /// </summary>
        public void Unload(string registryName);
        /// <summary>
        /// 判断指定资源是否存在于缓存区
        /// </summary>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public bool Exist(string registryName);
        /// <summary>
        /// 强制加载资源, 使用该方法加载的资源不会被clear清除, 但可以被unload指定卸载
        /// </summary>
        /// <param name="registryName">资源名</param>
        /// <param name="callback">加载完毕后的回调</param>
        public void LoadForce(string registryName, Action callback);
        /// <summary>
        /// 强制卸载所有资源
        /// </summary>
        public void ClearForce();
        /// <summary>
        /// 获取被强制加载的资源注册名表
        /// </summary>
        /// <returns></returns>
        public string[] GetForceResource();
        /// <summary>
        /// 获取指定资源
        /// </summary>
        /// <param name="registryName">资源注册名</param>
        /// <returns></returns>
        public IResource Get(string registryName);

    }
}