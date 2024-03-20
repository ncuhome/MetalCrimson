using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.Resource
{
    public class SpriteLoader : AsyncResourceLoader<SpriteResource>
    {
        public SpriteLoader()
        {
            head = "img";
        }
        public override IResource Get(string registryName)
        {
            if (dic.TryGetValue(registryName, out var resource))
            {
                return resource;
            }
            Debug.LogError($"访问图片资源不存在:{registryName}");
            return null;
        }

        protected override IEnumerator GetRequest(string url, string registryName, Action callback)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                dic[registryName] = new SpriteResource(registryName, DownloadHandlerTexture.GetContent(request).TextureToSprite());
            }
            else
            {
                Debug.LogError($"加载资源失败:{registryName}");
            }
            callback?.Invoke();
        }

        protected override void LoadWithAddressable(string url, string registryName, Action callback)
        {
            Addressables.LoadAssetAsync<Texture2D>(url).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    dic[registryName] = new SpriteResource(registryName, handle.Result.TextureToSprite());
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