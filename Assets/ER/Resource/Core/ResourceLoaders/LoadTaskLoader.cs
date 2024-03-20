using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.Resource
{
    public class LoadTaskLoader : AsyncResourceLoader<LoadTaskResource>
    {
        public LoadTaskLoader()
        {
            head = "pack";
        }
        public override IResource Get(string registryName)
        {
            if (dic.TryGetValue(registryName, out var resource))
            {
                return resource;
            }
            Debug.LogError($"访问包资源不存在:{registryName}");
            return null;
        }

        protected override IEnumerator GetRequest(string url, string registryName, Action callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                dic[registryName] = new LoadTaskResource(registryName, request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"加载资源失败:{registryName}");
            }
            callback?.Invoke();
        }

        protected override void LoadWithAddressable(string url, string registryName, Action callback)
        {
            Addressables.LoadAssetAsync<TextAsset>(url).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    dic[registryName] = new LoadTaskResource(registryName, handle.Result.text);
                }
                else
                {
                    Debug.LogError($"加载资源失败:{registryName}");
                }
                callback?.Invoke();
            };
        }
    }
}