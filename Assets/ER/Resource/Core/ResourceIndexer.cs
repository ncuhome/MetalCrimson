using ER.Parser;
using ER.ResourcePacker;
using ER.Template;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER.Resource
{
    /// <summary>
    /// 资源索引器, 将资源注册名 转化为 地址或者url
    /// 外部资源使用@作为前缀标识
    /// </summary>
    public class ResourceIndexer:Singleton<ResourceIndexer>,MonoInit
    {
        /// <summary>
        /// 子索引器缓存:  (资源包名:资源包所在的绝对路径)
        /// </summary>
        private Dictionary<string, string> indexers = new Dictionary<string, string>();
        /// <summary>
        /// 键值字典
        /// </summary>
        private Dictionary<string, string> dic=new Dictionary<string, string>();
        /// <summary>
        /// 重新初始化资源索引器
        /// </summary>
        public void Init()
        {
            dic.Clear();
            LoadIndexer();
        }
        /// <summary>
        /// 加载内部和外部索引器
        /// </summary>
        private void LoadIndexer()
        {
            string config = File.ReadAllText(ERinbone.CustomRIndexerPath);
            indexers = JsonConvert.DeserializeObject<Dictionary<string,string>>(config);
            foreach(var path in indexers.Values)
            {
                string url = ERinbone.Combine(path,ResourcePack.IndexerFileName);
                if (!File.Exists(url))
                {
                    Debug.Log($"加载资源索引文件出错: 无效url:{url}");
                    return;
                }
                INIParser parser = new INIParser();
                parser.ParseINIFile(path);

                Dictionary<string, string> _dic = parser.GetSection("indexer");
                foreach (KeyValuePair<string, string> pair in _dic)
                {
                    dic[pair.Key] = ERinbone.Combine(path, pair.Value);//因为indexer里填的是相对路径, 所以需要拼接成完整路径
                }

            }
        }
        /// <summary>
        /// 将注册名转化为加载路径
        /// </summary>
        /// <param name="registryName">资源注册名</param>
        /// <param name="defResource">是否是本地路径</param>
        /// <returns></returns>
        public string Convert(string registryName,out bool defResource)
        {
            if(dic.TryGetValue(registryName,out string value))//如果有替换索引, 默认表示是外部资源
            {
                defResource = false;
                return value;
            }
            else
            {
                defResource= true;
                string[] parts= registryName.Split(':');//0: 资源头  1:模组名 2:路径
                return ERinbone.Combine(parts[0], parts[2]);
            }
        }
    }
}