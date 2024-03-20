using ER.Resource;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor.PackageManager.Requests;
using System.Threading.Tasks;
using ER.Template;
using System.Collections;

namespace Mod_Resource
{
    public class LanguageLoader : AsyncResourceLoader<LanguageResource>
    {
        public LanguageLoader()
        {
            head = "lang";
        }

        public override IResource Get(string registryName)
        {
            if (dic.TryGetValue(registryName, out var resource))
            {
                return resource;
            }
            Debug.LogError($"访问语言资源不存在:{registryName}");
            return null;
        }

        protected override void LoadWithAddressable(string url, string registryName, Action callback)
        {
            Addressables.LoadAssetAsync<TextAsset>(url).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    dic[registryName] = new LanguageResource(registryName, handle.Result.text);
                }
                else
                {
                    Debug.LogError($"加载资源失败:{registryName}");
                }
                callback?.Invoke();
            };
        }

        protected override IEnumerator GetRequest(string url, string registryName, Action callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                dic[registryName] = new LanguageResource(registryName, request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"加载资源失败:{registryName}");
            }
            callback?.Invoke();
        }
    }
}