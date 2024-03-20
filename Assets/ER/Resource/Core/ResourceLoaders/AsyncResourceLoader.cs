using ER.Template;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace ER.Resource
{
    public abstract class AsyncResourceLoader<T>:MonoBehaviour,IResourceLoader,MonoInit where T:class,IResource
    {
        protected Dictionary<string, T> dic = new Dictionary<string, T>();//资源缓存 注册名:资源
        protected HashSet<string> force_load = new HashSet<string>();//用于记录被强制加载的资源的注册名
        protected string head = "res";

        public string Head
        {
            get => head;
            set => head = value;
        }



        public void Clear()
        {
            Dictionary<string, T> _dic = new Dictionary<string, T>();
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

        public abstract IResource Get(string registryName);

        public string[] GetForceResource()
        {
            return force_load.ToArray();
        }

        public void ELoad(string registryName, Action callback, bool skipConvert = false)
        {
            if (!dic.ContainsKey(registryName))
            {
                Load(registryName, callback, skipConvert);
            }
        }
        public void Load(string registryName, Action callback, bool skipConvert = false)
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
                LoadWithAddressable(url, registryName, callback);
            }
            else
            {
                StartCoroutine(GetRequest(url, registryName, callback));
            }
        }
        protected abstract void LoadWithAddressable(string url, string registryName, Action callback);
        protected abstract IEnumerator GetRequest(string url, string registryName, Action callback);
        
        public void LoadForce(string registryName, Action callback, bool skipConvert = false)
        {
            Load(registryName, callback, skipConvert);
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

        public void Init()
        {
            GR.AddLoader(this);
            MonoLoader.InitCallback();
        }
    }
}