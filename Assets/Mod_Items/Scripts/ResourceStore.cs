

using ER.ResourcePacker;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Item
{
    /// <summary>
    /// 资源总仓库
    /// </summary>
    public class ResourceStore
    {
        private Dictionary<string, SpriteResource> sprites = new Dictionary<string, SpriteResource>();
        private Dictionary<string,AudioResource> audios = new Dictionary<string, AudioResource>();
        private Dictionary<string,TextResource> texts = new Dictionary<string, TextResource>();
        private Dictionary<string,IMMaterial> marerials = new Dictionary<string,IMMaterial>();
        private Dictionary<string, IMComponent> components = new Dictionary<string, IMComponent>();
        private Dictionary<string, IMComponentMold> component_molds = new Dictionary<string, IMComponentMold>();
        /// <summary>
        /// 判断指定资源是否已经加载
        /// </summary>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public bool IsLoaded(string registryName)
        {
            string[] part = registryName.Split(':');
            if (part[1] == "mc")//本模组名称
            {
                switch (part[0])
                {
                    case "img":
                    case "wav":
                    case "txt":
                        return GameResource.Instance.LoadedExist(registryName);
                    case "material":
                        break;
                    case "component_mold":
                        return component_molds.ContainsKey(registryName);
                    case "component":
                        return components.ContainsKey(registryName);
                    case "item":
                        Debug.Log("代码未完善");
                        break;
                    default:
                        Debug.Log($"错误资源类别:{part[0]}, 无法读取该资源信息");
                        break;
                }
            }
            else
            {
                Debug.Log("模组系统暂未完成, 无法读取该资源信息");
            }
            return false;
        }

        public IResource this[string registryName]
        {
            get
            {
                string[] part = registryName.Split(':');
                //0: 资源类别   1:模组名称   2:资源地址
                if (part[1] == "mc")//本模组名称
                {
                    switch(part[0])
                    {
                        case "img":
                            break;
                        case "wav":
                            break;
                        case "txt":
                            break;
                        case "material":
                            break;
                        case "component_mold":
                            break;
                        case "component":
                            break;
                        case "item":
                            Debug.Log("代码未完善");
                            break;
                        default:
                            Debug.Log($"错误资源类别:{part[0]}, 无法读取该资源信息");
                            break;
                    }
                }
                else
                {
                    Debug.Log("模组系统暂未完成, 无法读取该资源信息"); 
                }
                return null;
            }
        }
    }
}