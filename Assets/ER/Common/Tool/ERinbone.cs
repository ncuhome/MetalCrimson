// Ignore Spelling: Rinbone

using System.IO;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 旋转方向
    /// </summary>
    public enum RotateDir
    {
        /// <summary>
        /// 逆时针
        /// </summary>
        Anticlockwise,

        /// <summary>
        /// 顺时针
        /// </summary>
        Clockwise,

        /// <summary>
        /// 平行
        /// </summary>
        Parallel
    }

    /// <summary>
    /// 4方向枚举
    /// </summary>
    public enum Dir4
    {
        /// <summary>
        /// 无方向(错误方向)
        /// </summary>
        None,

        /// <summary>
        /// 上
        /// </summary>
        Up,

        /// <summary>
        /// 下
        /// </summary>
        Down,

        /// <summary>
        /// 左
        /// </summary>
        Left,

        /// <summary>
        /// 右
        /// </summary>
        Right,
    }

    /// <summary>
    /// 用于存储 ER 下一些静态字段
    /// </summary>
    public static class ERinbone
    {
        private static System.Random random;
        /// <summary>
        /// 获取内部设置文件地址
        /// </summary>
        public static string DefSettingsAddress
        {
            get => "config/settings";
        }
        /// <summary>
        /// 获取用户配置目录地址
        /// </summary>
        public static string CustomConfigPath
        {
            get => Combine(Application.streamingAssetsPath, "config");
        }
        /// <summary>
        /// 获取用户设置文件地址
        /// </summary>
        public static string CustomSettingsPath
        {
            get => Combine(CustomConfigPath, "settings.json");
        }
        /// <summary>
        /// 获取用户设置文件信息
        /// </summary>
        public static FileInfo CustomSettingsFile
        {
            get
            {
                FileInfo fileInfo = new FileInfo(CustomSettingsPath);
                if(!fileInfo.Exists)
                {
                    if(!Directory.Exists(CustomConfigPath))
                    {
                        Directory.CreateDirectory(CustomConfigPath);
                    }
                    fileInfo.Create();
                }
                return fileInfo;
            }
        }
        /// <summary>
        /// 用户自定义资源索引器配置文件地址
        /// </summary>
        public static string CustomRIndexerPath
        {
            get => Combine(CustomConfigPath, "resource_indexer.json");
        }
        /// <summary>
        /// 用户自定义资源索引器配置文件信息
        /// </summary>
        public static FileInfo CustomRIndexerFile
        {
            get
            {
                FileInfo fileInfo = new FileInfo(CustomRIndexerPath);
                if (!fileInfo.Exists)
                {
                    if (!Directory.Exists(CustomConfigPath))
                    {
                        Directory.CreateDirectory(CustomConfigPath);
                    }
                    fileInfo.Create();
                }
                return fileInfo;
            }
        }

        /// <summary>
        /// 预设存档文件夹路径
        /// </summary>
        public static string SavePath
        {
            get => Path.Combine(Application.streamingAssetsPath, "saves");
        }

        /// <summary>
        /// 预设可读写配置文件夹路径
        /// </summary>
        public static string ConfigPath
        {
            get => Path.Combine(Application.streamingAssetsPath, "config");
        }

        /// <summary>
        /// 预设资源包文件夹路径
        /// </summary>
        public static string ResourcePath
        {
            get => Path.Combine(Application.streamingAssetsPath, "resource");
        }

        /// <summary>
        /// 拼接路径
        /// </summary>
        /// <returns></returns>
        public static string Combine(string str1, string str2)
        {
            return str1 + '/' + str2;
        }
        /// <summary>
        /// 获取一个随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomNumber(int min,int max)
        {
            if(random == null)
                random = new System.Random();
            return random.Next(min, max);
        }
        /// <summary>
        /// 获取一个随机数
        /// </summary>
        /// <returns></returns>
        public static double RandomNumber()
        {
            if (random == null)
                random = new System.Random();
            return random.NextDouble();
        }
    }
}