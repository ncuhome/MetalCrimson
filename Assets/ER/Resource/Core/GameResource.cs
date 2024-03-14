using ER.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using static ER.Resource.GameResource;

namespace ER.Resource
{
    /// <summary>
    /// 游戏资源(GameResource 的静态替身), 没有额外作用, 只是方便调用
    /// </summary>
    public static class GR
    {
        /// <summary>
        /// 判断是否是资源注册名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsRegistryName(string str)
        {
            return Regex.IsMatch(str, @"^[a-z_\/]+:[a-z_\/]+:[a-z_\/]+$");
        }
        /// <summary>
        /// 获取资源注册名的模组名
        /// </summary>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public static string GetModName(string registryName)
        {
            string[] parts = registryName.Split(':');
            return parts[1];
        }
        /// <summary>
        /// 获取注册名的资源头
        /// </summary>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public static string GetTypeName(string registryName)
        {
            string[] parts = registryName.Split(':');
            return parts[0];
        }
        /// <summary>
        /// 获取注册名的全路径
        /// </summary>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public static string GetAddressAll(string registryName)
        {
            string[] parts = registryName.Split(':');
            return ERinbone.Combine(parts[0], parts[2]);
        }



        /// <summary>
        /// 添加资源加载器
        /// </summary>
        /// <param name="loader"></param>
        public static void AddLoader(IResourceLoader loader)
        {
            GameResource.Instance.AddLoader(loader);
        }
        /// <summary>
        /// 移除指定资源头的加载器
        /// </summary>
        /// <param name="head"></param>
        public static void RemoveLoader(string head)
        {
            GameResource.Instance.RemoveLoader(head);
        }
        /// <summary>
        /// 移除所有资源加载器
        /// </summary>
        public static void RemoveLoaderAll()
        {
            GameResource.Instance.RemoveLoaderAll();
        }
        /// <summary>
        /// 加载指定资源
        /// </summary>
        /// <param name="callback">资源组加载完毕后回调</param>
        /// <param name="check">是否启用重复性检查, 若启用则会跳过加载已经在缓存中的资源</param>
        /// <param name="registryName"></param>
        public static void Load(Action callback, bool check, params string[] registryName)
        {
            GameResource.Instance.Load(callback,check, registryName);
        }
        /// <summary>
        /// 强制加载指定资源
        /// </summary>
        /// <param name="callback">资源组加载完毕后回调</param>
        /// <param name="registryName">注册名</param>
        public static void LoadForce(Action callback, params string[] registryName)
        {
            GameResource.Instance.LoadForce(callback, registryName);
        }

        /// <summary>
        /// 清除所有资源缓存(除了强制加载的资源)
        /// </summary>
        public static void Clear()
        {
            GameResource.Instance.Clear();
        }
        /// <summary>
        /// 强制移除所有资源缓存
        /// </summary>
        public static void ClearForce()
        {
            GameResource.Instance.ClearForce();
        }

        /// <summary>
        /// 卸载资源缓存
        /// </summary>
        /// <param name="keys"></param>
        public static void Unload(params string[] registryName)
        {
            GameResource.Instance.Unload(registryName);
        }
        /// <summary>
        /// 判断指定资源是否已经加载
        /// </summary>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public static bool IsLoaded(string registryName)
        {
            return GameResource.Instance.IsLoaded(registryName);
        }
        /// <summary>
        /// 获取指定资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public static IResource Get(string registryName)
        {
            return GameResource.Instance.Get(registryName);
        }
        /// <summary>
        /// 获取指定资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public static T Get<T>(string registryName) where T : class, IResource
        {
            return GameResource.Instance.Get<T>(registryName);
        }

    }

    /// <summary>
    /// 游戏资源: 用于 管理 和 缓存 游戏所需要的各种资源
    /// #1: 使用资源注册名 访问指定资源
    /// #2: 返回封装后的资源
    /// #3: 注册名: 资源头:模组名:地址名
    /// #4: 文本等效资源头, 列表中资源类型将会以指定资源的形式加载
    /// #5: 不属于 wav,txt,img 且没有填写等效资源头 的资源默 认以txt形式加载
    /// </summary>
    public class GameResource : Singleton<GameResource>
    {
        private Dictionary<string, IResourceLoader> loaders = new Dictionary<string, IResourceLoader>();//资源加载器 资源头:加载器对象
        private List<LoadProgress> loadProgresses = new List<LoadProgress>();//资源加载任务表

        /// <summary>
        /// 添加资源加载器
        /// </summary>
        /// <param name="loader"></param>
        public void AddLoader(IResourceLoader loader)
        {
            loaders.Add(loader.Head,loader);
        }
        /// <summary>
        /// 移除指定资源头的加载器
        /// </summary>
        /// <param name="head"></param>
        public void RemoveLoader(string head)
        {
            loaders.Remove(head);
        }
        /// <summary>
        /// 移除所有资源加载器
        /// </summary>
        public void RemoveLoaderAll()
        {
            loaders.Clear();
        }
        /// <summary>
        /// 加载指定资源
        /// </summary>
        /// <param name="callback">资源组加载完毕后回调</param>
        /// <param name="check">是否启用重复性检查, 若启用则会跳过加载已经在缓存中的资源</param>
        /// <param name="registryName"></param>
        public void Load(Action callback,bool check,params string[] registryName)
        {
            if (registryName.Length == 0)
            {
                callback?.Invoke();
                return;
            }
            Action progressAdd = null;//子回调函数
            if (callback != null)
            {
                for (int i = 0; i < loadProgresses.Count; i++)//清空加载任务列表中已经完成的任务表
                {
                    if (loadProgresses[i].done)
                    {
                        loadProgresses.RemoveAt(i);
                        i--;
                    }
                }
                //创建新的加载任务表
                LoadProgress progress = new LoadProgress();
                progress.loaded = 0;
                progress.count = registryName.Length;
                progress.callback = callback;
                progress.done = false;
                loadProgresses.Add(progress);
                progressAdd = () =>
                {
                    progress.AddProgress();
                };
            }
            for(int i=0;i< registryName.Length;i++)//逐个加载资源
            {
                string head = GR.GetTypeName(registryName[i]);
                if(loaders.TryGetValue(head,out IResourceLoader loader))
                {
                    if(check)
                    {
                        loader.ELoad(registryName[i], callback);
                    }
                    else
                    {
                        loader.Load(registryName[i], callback);
                    }
                }
                else//如果没有找到加载器则报错
                {
                    Debug.LogWarning($"缺失 {head} 资源加载器, <{registryName[i]}>资源加载失败");
                    progressAdd?.Invoke();
                }
            }
        }
        /// <summary>
        /// 强制加载指定资源
        /// </summary>
        /// <param name="callback">资源组加载完毕后回调</param>
        /// <param name="registryName">注册名</param>
        public void LoadForce(Action callback, params string[] registryName)
        {
            if (registryName.Length == 0)
            {
                callback?.Invoke();
                return;
            }
            Action progressAdd = null;//子回调函数
            if (callback != null)
            {
                for (int i = 0; i < loadProgresses.Count; i++)//清空加载任务列表中已经完成的任务表
                {
                    if (loadProgresses[i].done)
                    {
                        loadProgresses.RemoveAt(i);
                        i--;
                    }
                }
                //创建新的加载任务表
                LoadProgress progress = new LoadProgress();
                progress.loaded = 0;
                progress.count = registryName.Length;
                progress.callback = callback;
                progress.done = false;
                loadProgresses.Add(progress);
                progressAdd = () =>
                {
                    progress.AddProgress();
                };
            }
            for (int i = 0; i < registryName.Length; i++)//逐个加载资源
            {
                string head = GR.GetTypeName(registryName[i]);
                if (loaders.TryGetValue(head, out IResourceLoader loader))
                {
                    loader.LoadForce(registryName[i], callback);
                }
                else//如果没有找到加载器则报错
                {
                    Debug.LogWarning($"缺失 {head} 资源加载器, <{registryName[i]}>资源加载失败");
                    progressAdd?.Invoke();
                }
            }
        }

        /// <summary>
        /// 清除所有资源缓存(除了强制加载的资源)
        /// </summary>
        public void Clear()
        {
           foreach(var pair in loaders)
            {
                pair.Value.Clear();
            }
        }
        /// <summary>
        /// 强制移除所有资源缓存
        /// </summary>
        public void ClearForce()
        {
            foreach (var pair in loaders)
            {
                pair.Value.ClearForce();
            }
        }

        /// <summary>
        /// 卸载资源缓存
        /// </summary>
        /// <param name="keys"></param>
        public void Unload(params string[] registryName)
        {
            for (int i = 0; i < registryName.Length; i++)//逐个加载资源
            {
                string head = GR.GetTypeName(registryName[i]);
                if (loaders.TryGetValue(head, out IResourceLoader loader))
                {
                    loader.Unload(registryName[i]);
                }
                else//如果没有找到加载器则报错
                {
                    Debug.LogWarning($"缺失 {head} 资源加载器, <{registryName[i]}>资源卸载失败");
                }
            }
        }
        /// <summary>
        /// 判断指定资源是否已经加载
        /// </summary>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public bool IsLoaded(string registryName)
        {
            string head = GR.GetTypeName(registryName);
            if (loaders.TryGetValue(head, out IResourceLoader loader))
            {
                return loader.Exist(registryName);
            }
            else//如果没有找到加载器则报错
            {
                Debug.LogWarning($"缺失 {head} 资源加载器, 在判断<{registryName}>资源时失败");
                return false;
            }
        }
        /// <summary>
        /// 获取指定资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public IResource Get(string registryName)
        {
            string head = GR.GetTypeName(registryName);
            if (loaders.TryGetValue(head, out IResourceLoader loader))
            {
                Debug.Log($"获取资源:{registryName}");
                return loader.Get(registryName);
            }
            else//如果没有找到加载器则报错
            {
                Debug.LogWarning($"缺失 {head} 资源加载器, 在获取<{registryName}>资源时失败");
                return null;
            }
        }
        /// <summary>
        /// 获取指定资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public T Get<T>(string registryName) where T : class, IResource
        {
            return Get(registryName) as T;
        }
    }

    public struct LoadProgress
    {
        public int loaded;
        public int count;
        public Action callback;
        public bool done;

        public void AddProgress()
        {
            loaded++;
            if (loaded >= count)
            {
                done = true;
                callback?.Invoke();
            }
        }
    }
}