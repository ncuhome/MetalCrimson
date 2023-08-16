#define Test
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ER.Parser
{
    /// <summary>
    /// 语言包加载错误
    /// </summary>
    public class LanguagePackLoadException : Exception
    {
        /// <summary>
        /// 消息输出接口
        /// </summary>
        public static event Action<string> Output = message => Debug.Log(message);
        public LanguagePackLoadException() : base("语言包加载错误")
        {
            Output("语言包加载错误");
        }

        public LanguagePackLoadException(string message) : base(message)
        {
            Output("语言包加载错误");
        }
    }

    public struct LanguagePackInfo
    {
        /// <summary>
        /// 消息输出接口
        /// </summary>
        public static event Action<string> Output = message => Debug.Log(message);
        public string LanguagePackPath;
        /// <summary>
        /// 语言包名称
        /// </summary>
        public string LanguagePackName;
        /// <summary>
        /// 语言包版本
        /// </summary>
        public string LanguagePackVersion;
        /// <summary>
        /// 语言包作者
        /// </summary>
        public string LanguagePackAuthor;
        /// <summary>
        /// 语言包图片路径
        /// </summary>
        public string ImagePath;
        /// <summary>
        /// 语言包描述
        /// </summary>
        public string LanguagePackDescription;

        public void Print()
        {
            Output($"Path:{LanguagePackPath}");
            Output($"name:{LanguagePackName}");
            Output($"version:{LanguagePackVersion}");
            Output($"author:{LanguagePackAuthor}");
            Output($"image:{ImagePath}");
            Output($"description:{LanguagePackDescription}");

        }
    }

    /// <summary>
    /// 语言包对象
    /// </summary>
    public class LanguagePack
    {
        /// <summary>
        /// 消息输出接口
        /// </summary>
        public static event Action<string> Output = message => Debug.Log(message);
        /// <summary>
        /// 语言包说明文件名称
        /// </summary>
        public const string IllustrationFileName = "illustration.ini";
        /// <summary>
        /// 路径转接文件名称
        /// </summary>
        public const string AdapterFileName = "adapter.ini";

        #region 语言包信息
        /// <summary>
        /// 文件包所在的路径
        /// </summary>
        public string LanguagePackPath { get; private set; } = string.Empty;
        /// <summary>
        /// 语言包名称
        /// </summary>
        public string LanguagePackName = string.Empty;
        /// <summary>
        /// 语言包版本
        /// </summary>
        public string LanguagePackVersion = string.Empty;
        /// <summary>
        /// 语言包作者
        /// </summary>
        public string LanguagePackAuthor = string.Empty;
        /// <summary>
        /// 语言包图片路径
        /// </summary>
        public string ImagePath = string.Empty;
        /// <summary>
        /// 语言包描述
        /// </summary>
        public string LanguagePackDescription = string.Empty;
        /// <summary>
        /// 路径转接器，用于快速查找文本文件所在的路径
        /// </summary>
        private Dictionary<string, string> PathAdapter = new Dictionary<string, string>();
        #endregion

        /// <summary>
        /// 当前正在加载的文本片段文件路径（用于避免重复加载）
        /// </summary>
        private string loadedFilePath = string.Empty;
        /// <summary>
        /// 文本缓存器
        /// </summary>
        private INIParser TextCache = new INIParser();

        #region 语言包函数
        /// <summary>
        /// 加载语言包
        /// </summary>
        /// <param name="packPath"></param>
        public LanguagePack(string packPath)
        {
            if (LoadPack(packPath))
            {
                Output("加载成功");
            }
            else
            {
                Output("加载失败");
            }
        }
        /// <summary>
        /// 封装一个语言包对象，当语言包存在缺失成分时，将使用默认包中的内容
        /// </summary>
        /// <param name="pack1">包1路径，通常作为默认包，是稳定的</param>
        /// <param name="pack2">包2路径，此路径的信息将会覆盖包1的信息，允许不稳定</param>
        /// <exception cref="LanguagePackLoadException">当语言包路径无效，或者包资源结构不完整时，抛出这个异常</exception>
        public LanguagePack(string pack1, string pack2)
        {
            if (LoadPack(pack1))
            {
                Output("包1加载成功");
            }
            else
            {
                Output("包1加载失败");
            }
            if (pack1 == pack2) return;
            if (LoadPack(pack2))
            {
                Output("包2加载成功");
            }
            else
            {
                Output("包2加载失败");
            }
        }

        public LanguagePackInfo GetInfo() => new LanguagePackInfo()
        {
            LanguagePackVersion = LanguagePackVersion,
            LanguagePackPath = LanguagePackPath,
            LanguagePackName = LanguagePackName,
            LanguagePackDescription = LanguagePackDescription,
            LanguagePackAuthor = LanguagePackAuthor,
            ImagePath = ImagePath,
        };

        public void PrintInfo()
        {
            Output($"Path:{LanguagePackPath}");
            Output($"name:{LanguagePackName}");
            Output($"version:{LanguagePackVersion}");
            Output($"author:{LanguagePackAuthor}");
            Output($"image:{ImagePath}");
            Output($"description:{LanguagePackDescription}");

        }
        /// <summary>
        /// 获取语言包配置信息
        /// </summary>
        /// <param name="packPath">语言包路径</param>
        /// <returns></returns>
        public static LanguagePackInfo GetInfo(string packPath)
        {
            INIParser parser = new INIParser();
            parser.ParseINIFile(Path.Combine(packPath, IllustrationFileName));
            return new LanguagePackInfo()
            {
                LanguagePackName = parser.GetValue("description", "name") + string.Empty,
                LanguagePackVersion = parser.GetValue("description", "version") + string.Empty,
                LanguagePackAuthor = parser.GetValue("description", "author") + string.Empty,
                ImagePath = parser.GetValue("description", "image") + string.Empty,
                LanguagePackDescription = parser.GetValue("description", "description") + string.Empty
            };
        }
        /// <summary>
        /// 加载语言包，覆盖旧的包
        /// </summary>
        /// <param name="packPath">语言包路径</param>
        /// <returns>加载是否成功</returns>
        public bool LoadPack(string packPath)
        {
            if (Directory.Exists(packPath))
            {
                LanguagePackPath = packPath;
                INIParser parser = new INIParser();
                parser.ParseINIFile(Path.Combine(packPath, IllustrationFileName));

                LanguagePackName = parser.GetValue("description", "name") + string.Empty;
                LanguagePackVersion = parser.GetValue("description", "version") + string.Empty;
                LanguagePackAuthor = parser.GetValue("description", "author") + string.Empty;
                ImagePath = parser.GetValue("description", "image") + string.Empty;
                LanguagePackDescription = parser.GetValue("description", "description") + string.Empty;

                parser.Clear();
                parser.ParseINIFile(Path.Combine(packPath, AdapterFileName));
                var adp = parser.GetSection("adapter");

                if (adp == null) return false;
                foreach (string key in adp.Keys)
                {
                    PathAdapter[key] = Path.Combine(packPath, adp[key]);
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 缓存指定文本片段
        /// </summary>
        /// <param name="name">文本片段名</param>
        /// <returns>是否加载成功</returns>
        public bool Load(string name)
        {
            if (PathAdapter.TryGetValue(name, out string path))
            {
                if (File.Exists(path))
                {
                    if (path != loadedFilePath)//避免重复加载
                    {
                        TextCache.ParseINIFile(path);
                        loadedFilePath = path;

                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 清空文本缓存
        /// </summary>
        public void Clear()
        {
            TextCache.Clear();
            loadedFilePath = string.Empty;
        }
        /// <summary>
        /// 清空文本缓存，只保留指定节段的文本
        /// </summary>
        /// <param name="sections">需要保留的节段名称</param>
        public void Clear(params string[] sections)
        {
            TextCache.Clear(sections);
        }
        /// <summary>
        /// 读取指定键的文本内容，如果不存在相应的文本内容则返回null
        /// </summary>
        /// <param name="key">文本键名</param>
        /// <returns></returns>
        public string this[string section, string key]
        {
            get
            {
                var sec = TextCache.GetSection(section);
                if (sec == null)
                {
                    if (Load(section))
                    {
                        return TextCache.GetValue(section, key);
                    }
                    return null;
                }
                else
                {
                    if (sec.TryGetValue(key, out string value))
                    {
                        return value;
                    }
                    return null;
                }

            }
        }
        /// <summary>
        /// 根据路径获取文本内容，默认最后一部分名称是键名，前面的是路径名，路径使用"."分隔部分名
        /// </summary>
        /// <param name="path">路径名</param>
        /// <returns></returns>
        public string this[string path]
        {
            get
            {
                //获取截断点索引
                int index = path.Length - 1;
                for (;index>=0; index--)
                {
                    if (path[index] == '.')
                    {
                        break;
                    }
                }
                string sectionName = path.Substring(0, index);
                string keyName = path.Substring(index+1);
                return this[sectionName, keyName];
            }
        }
        #endregion
    }

    /// <summary>
    /// 文本包替换器
    /// </summary>
    public class TextReplacer
    {
        /// <summary>
        /// 消息输出接口
        /// </summary>
        public static event Action<string> Output = message => Debug.Log(message);
        /// <summary>
        /// 语言包文件夹路径
        /// </summary>
        public string LanguagePackPath = string.Empty;
        /// <summary>
        /// 默认语言包
        /// </summary>
        public string defaultPack = string.Empty;
        /// <summary>
        /// 当前选择的语言包
        /// </summary>
        public string selected = string.Empty;

        private List<LanguagePackInfo> packs = new List<LanguagePackInfo>();

        
        /// <summary>
        /// 检查并更新语言包
        /// </summary>
        public void CheckPath()
        {
            if (PathExist(LanguagePackPath))
            {
                if (IsDirectory(LanguagePackPath))//判断语言包路径是否为一个文件夹
                {
                    string[] subdirectories = Directory.GetDirectories(LanguagePackPath);
                    foreach (string subdirectory in subdirectories)
                    {
                        string path = Path.Combine(subdirectory, LanguagePack.IllustrationFileName);
                        if (File.Exists(path))
                        {
                            Output($"路径存在：{path}");
                            packs.Add(LanguagePack.GetInfo(subdirectory));
                        }
                    }
                }
            }
        }
        public LanguagePackInfo[] GetPackInfos()
        {
            return packs.ToArray();
        }

        #region 静态工具方法
        public static bool PathExist(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsDirectory(string path)
        {
            FileAttributes attributes = File.GetAttributes(path);
            return (attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }
        public static void CompressFolder(string sourceFolderPath, string compressedFilePath)
        {
            ZipFile.CreateFromDirectory(sourceFolderPath, compressedFilePath);
        }
        public static void DecompressFolder(string compressedFilePath, string decompressedFolderPath)
        {
            ZipFile.ExtractToDirectory(compressedFilePath, decompressedFolderPath);
        }
        #endregion
    }
}

