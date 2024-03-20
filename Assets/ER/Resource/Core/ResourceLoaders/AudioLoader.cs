using ER.Template;
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
    public class AudioLoader : AsyncResourceLoader<AudioResource>
    {
        public AudioLoader()
        {
            head = "wav";
        }

        public override IResource Get(string registryName)
        {
            if(dic.TryGetValue(registryName,out AudioResource resource ))
            {
                return resource;
            }
            Debug.LogError($"访问音频资源不存在:{registryName}");
            return null;
        }

        protected override void LoadWithAddressable(string url, string registryName, Action callback)
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

        protected override IEnumerator GetRequest(string url, string registryName, Action callback)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);
            yield return request.SendWebRequest();
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
}