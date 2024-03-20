using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.Resource
{
    public class AudioLoader : IResourceLoader
    {
        private Dictionary<string, AudioResource> dic = new Dictionary<string, AudioResource>();//资源缓存 注册名:资源
        private HashSet<string> force_load = new HashSet<string>();//用于记录被强制加载的资源的注册名
        private string head = "wav";

        public string Head
        {
            get => head;
            set => head = value;
        }



        public void Clear()
        {
            Dictionary<string, AudioResource> _dic = new Dictionary<string, AudioResource>();
            foreach (var res in dic)
            {
                if (force_load.Contains(res.Key))
                {
                    dic.Add(res.Key, res.Value);
                }
            }
            dic = _dic;
        }

        public void ClearForce()
        {
            dic.Clear();
        }

        public bool Exist(string registryName)
        {
            return dic.ContainsKey(registryName);
        }

        public IResource Get(string registryName)
        {
            if(dic.TryGetValue(registryName, out var resource))
            {
                return resource;
            }
            Debug.LogError($"访问音频资源不存在:{registryName}");
            return null;
        }

        public string[] GetForceResource()
        {
            return force_load.ToArray();
        }

        public void ELoad(string registryName, Action callback, bool skipConvert=false)
        {
            if(!dic.ContainsKey(registryName))
            {
                Load(registryName, callback, skipConvert);
            }
        }
        public async void Load(string registryName, Action callback, bool skipConvert=false)
        {
            bool defRes;

            string url = registryName;
            if (skipConvert)
            {
                if (url.StartsWith('@'))//@开头标识外部加载
                {
                    url = url.Substring(1);
                    defRes = false;
                }
                else
                {
                    defRes = true;
                }
                //处理注册名, head 使用解析器的 head, 模组使用 erinbone, 路径保持原样
                registryName = $"{head}:erinbone:{url}";
            }
            else
            {
                url = ResourceIndexer.Instance.Convert(registryName, out defRes);
            }
            if (defRes)
            {
                Addressables.LoadAssetAsync<AudioClip>(url).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        dic[registryName] = new AudioResource(registryName, handle.Result);
                    }
                    else
                    {
                        Debug.LogError($"加载资源失败:{registryName}");
                    }
                    callback?.Invoke();
                };
            }
            else
            {
                UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
                await Task.Run(request.SendWebRequest);
                if (request.result == UnityWebRequest.Result.Success)
                {
                    dic[registryName] = new AudioResource(registryName, DownloadHandlerAudioClip.GetContent(request));
                }
                else
                {
                    Debug.LogError($"加载资源失败:{registryName}");
                }
                callback?.Invoke();
            }
        }
        public void LoadForce(string registryName, Action callback, bool skipConvert = false)
        {
            Load(registryName, callback,skipConvert);
            force_load.Add(registryName);
        }

        public void Unload(string registryName)
        {
            if (dic.ContainsKey(registryName))
            {
                dic.Remove(registryName);
            }
            if (force_load.Contains(registryName))
            {
                force_load.Remove(registryName);
            }
        }

        public IResource[] GetAll()
        {
            return dic.Values.ToArray();
        }

        public string[] GetAllRegistryName()
        {
            return dic.Keys.ToArray();
        }
    }
}