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
            return dic[registryName];
        }

        public string[] GetForceResource()
        {
            return force_load.ToArray();
        }

        public void ELoad(string registryName, Action callback)
        {
            if(!dic.ContainsKey(registryName))
            {
                Load(registryName, callback);
            }
        }
        public async void Load(string registryName, Action callback)
        {
            bool defRes;
            string url = ResourceIndexer.Instance.Convert(registryName, out defRes);
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
        public void LoadForce(string registryName, Action callback)
        {
            Load(registryName, callback);
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
    }
}