using ER.Resource;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Mod_Resource
{
    public class RComponentMoldLoader : AsyncResourceLoader<RComponentMold>
    {
        public RComponentMoldLoader()
        {
            head = "compm";
        }
        public override IResource Get(string registryName)
        {
            return dic[registryName];
        }

        protected override IEnumerator GetRequest(string url, string registryName, Action callback)
        {

            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                dic[registryName] = CreateItem(registryName, request.downloadHandler.text);
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
                    dic[registryName] = CreateItem(registryName, handle.Result.text);
                }
                else
                {
                    Debug.LogError($"加载资源失败:{registryName}");
                }
                callback?.Invoke();
            };
        }

        private RComponentMold CreateItem(string registryName, string json)
        {
            RComponentMoldInfo infos = JsonConvert.DeserializeObject<RComponentMoldInfo>(json);
            RComponentMold component = new RComponentMold(infos);
            return component;
        }

    }
}