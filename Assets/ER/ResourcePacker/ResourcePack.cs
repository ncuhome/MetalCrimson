using ER.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace ER.ResourcePacker
{
    /// <summary>
    /// 资源包对象
    /// </summary>
    public class ResourcePack
    {
        /// <summary>
        /// 消息输出接口
        /// </summary>
        public static event Action<string> Output = message => Debug.Log(message);

        /// <summary>
        /// 包说明文件名称
        /// </summary>
        public const string IllustrationFileName = "illustration.ini";

        /// <summary>
        /// 转接文件名称
        /// </summary>
        public const string IndexerFileName = "indexer.ini";

        #region 语言包信息

        /// <summary>
        /// 文件包所在的路径
        /// </summary>
        public string PackPath { get; private set; } = string.Empty;

        /// <summary>
        /// 包名称
        /// </summary>
        public string PackName = string.Empty;

        /// <summary>
        /// 包版本
        /// </summary>
        public string PackVersion = string.Empty;

        /// <summary>
        /// 包作者
        /// </summary>
        public string PackAuthor = string.Empty;

        /// <summary>
        /// 封面图片路径
        /// </summary>
        public string ImagePath = string.Empty;

        /// <summary>
        /// 包描述
        /// </summary>
        public string PackDescription = string.Empty;

        /// <summary>
        /// 资源索引器
        /// </summary>
        private Dictionary<string, string> Indexer = new Dictionary<string, string>();
        /// <summary>
        /// 获取包描述文件的路径
        /// </summary>
        public string GetIllPath=>Path.Combine(PackPath, IllustrationFileName);
        /// <summary>
        /// 获取包索引文件的路径
        /// </summary>
        public string GetIndexerPath=>Path.Combine(PackPath, IndexerFileName);

        #endregion 语言包信息

        #region 语言包函数

        /// <summary>
        /// 加载资源包, 仅获取资源包描述信息
        /// </summary>
        /// <param name="packPath"></param>
        public ResourcePack(string packPath)
        {

            if (Directory.Exists(packPath))
            {
                PackPath = packPath;
                INIParser parser = new INIParser();
                parser.ParseINIFile(Path.Combine(packPath, IllustrationFileName));

                PackName = parser.GetValue("description", "name") + string.Empty;
                PackVersion = parser.GetValue("description", "version") + string.Empty;
                PackAuthor = parser.GetValue("description", "author") + string.Empty;
                ImagePath = parser.GetValue("description", "image") + string.Empty;
                PackDescription = parser.GetValue("description", "description") + string.Empty;
                Output("加载成功");
            }
            else
            {
                Output("加载失败");
            }
        }

        public ResourcePackInfo GetInfo() => new ResourcePackInfo()
        {
            PackVersion = PackVersion,
            PackPath = PackPath,
            PackName = PackName,
            PackDescription = PackDescription,
            PackAuthor = PackAuthor,
            ImagePath = ImagePath,
        };

        public void PrintInfo()
        {
            Output($"Path:{PackPath}");
            Output($"name:{PackName}");
            Output($"version:{PackVersion}");
            Output($"author:{PackAuthor}");
            Output($"image:{ImagePath}");
            Output($"description:{PackDescription}");
        }

        /// <summary>
        /// 获取语言包配置信息
        /// </summary>
        /// <param name="packPath">语言包路径</param>
        /// <returns></returns>
        public static ResourcePackInfo GetInfo(string packPath)
        {
            INIParser parser = new INIParser();
            Debug.Log("路径:" + packPath + "   "+ IllustrationFileName);
            string path = Path.Combine(packPath, IllustrationFileName);
            Debug.Log("自定义资源包配置路径加载文件路径:"+path);
            parser.ParseINIFile(path);
            return new ResourcePackInfo()
            {
                PackPath = packPath,
                PackName = parser.GetValue("description", "name") + string.Empty,
                PackVersion = parser.GetValue("description", "version") + string.Empty,
                PackAuthor = parser.GetValue("description", "author") + string.Empty,
                ImagePath = parser.GetValue("description", "image") + string.Empty,
                PackDescription = parser.GetValue("description", "description") + string.Empty
            };
        }
        public void LoadIndexer()
        {
            INIParser parser = new INIParser();
            parser.ParseINIFile(Path.Combine(PackPath, IndexerFileName));
            var adp = parser.GetSection("indexer");

            if (adp == null) return;
            foreach (string key in adp.Keys)
            {
                Indexer[key] = Path.Combine(PackPath, adp[key]);
            }
        }
        #endregion 语言包函数
    }
}