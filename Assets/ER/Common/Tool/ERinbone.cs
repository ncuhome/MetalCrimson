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
        /// Res下的配置目录
        /// </summary>
        public static string ResPathConfig
        {
            get => "configs";
        }

        /// <summary>
        /// Res下的音频目录
        /// </summary>
        public static string ResPathAudio
        {
            get => "audios";
        }

        /// <summary>
        /// Res下的图片目录
        /// </summary>
        public static string ResPathImage
        {
            get => "images";
        }

        /// <summary>
        /// Res资源文件夹目录(压缩)
        /// </summary>
        public static string ResPath
        {
            get => "Assets\\Res";
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
        public static int RandomNumber(int min, int max)
        {
            if (random == null)
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