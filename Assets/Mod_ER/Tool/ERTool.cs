using ER.Parser;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        public static Texture2D LoadTextureByIO(string path, int width = 2, int height = 2)
        {
            // 从本地文件系统读取图片数据
            byte[] imageData = System.IO.File.ReadAllBytes(path);

            // 创建纹理对象
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(imageData);

            return texture;
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
        public static Sprite TextureToSprite(this Texture2D texture)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            return sprite;
        }

        /// <summary>
        /// 获取一个颜色仅改变透明度的颜色
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color ModifyAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public static void PassEvent<T>(this PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            GameObject current = data.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                    break;
                    //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
                }
            }
        }
    }
}