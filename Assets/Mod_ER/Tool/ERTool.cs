using ER.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace ER
{
    public static class ERTool
    {
        /// <summary>
        /// 获取此字符串的解析数据
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Data Parse(this string text)
        {
            return Data.ParseTo(text);
        }

        /// <summary>
        /// 尝试将此字符串解析为整型
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool TryParseInt(this string text, out int Value)
        {
            int num = 0;
            try
            {
                num = Convert.ToInt32(text);
            }
            catch (FormatException)
            {
                Value = 0;
                return false;
            }
            catch (OverflowException)
            {
                Value = 0;
                return false;
            }
            Value = num;
            return true;
        }

        /// <summary>
        /// 尝试将此字符串解析为整型
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool TryParseDouble(this string text, out double Value)
        {
            double num = 0;
            try
            {
                num = Convert.ToDouble(text);
            }
            catch (FormatException)
            {
                Value = 0;
                return false;
            }
            catch (OverflowException)
            {
                Value = 0;
                return false;
            }
            Value = num;
            return true;
        }

        /// <summary>
        /// 尝试将此字符串解析为布尔值
        /// </summary>
        /// <param name="text"></param>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static bool TryParseBoolean(this string text, out bool Value)
        {
            if (text.ToUpper() == "TRUE")
            {
                Value = true;
                return true;
            }
            else if (text.ToUpper() == "FALSE")
            {
                Value = false;
                return true;
            }
            Value = false;
            return false;
        }

        public static void Print<TKey, TValue>(this KeyValuePair<TKey, TValue> pair,
            Action<string> printDelegate = null)
        {
            string txt = $"<{pair.Key?.ToString()}>:{pair.Value?.ToString()}";
            printDelegate?.Invoke(txt);
            Console.WriteLine(txt);
        }

        /// <summary>
        /// 从外部指定文件中加载图片
        /// </summary>
        /// <param name="height">图片高度</param>
        /// <param name="path">图片的文件路径</param>
        /// <param name="width">图片宽度</param>
        /// <returns></returns>
        public static Texture2D LoadTextureByIO(string path, int width = 2048, int height = 2048)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);//游标的操作，可有可无
            byte[] bytes = new byte[fs.Length];//生命字节，用来存储读取到的图片字节
            try
            {
                fs.Read(bytes, 0, bytes.Length);//开始读取，这里最好用trycatch语句，防止读取失败报错
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
            fs.Close();//切记关闭

            Texture2D texture = new Texture2D(width, height);
            if (texture.LoadImage(bytes))
            {
                UnityEngine.Debug.Log("图片加载完毕 ");
                return texture;//将生成的texture2d返回，到这里就得到了外部的图片，可以使用了
            }
            else
            {
                UnityEngine.Debug.Log("图片尚未加载");
                return null;
            }
        }

        /// <summary>
        /// 浏览文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void ExplorePath(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path.Replace('/', '\\'));
        }

        /// <summary>
        /// 将Texture2d转换为Sprite
        /// </summary>
        /// <param name="tex">参数是texture2d纹理</param>
        /// <returns></returns>
        public static Sprite TextureToSprite(this Texture2D tex)
        {
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
    }
}