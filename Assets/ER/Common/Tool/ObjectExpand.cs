using ER.Parser;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ER
{
    /// <summary>
    /// 提供一些通用的拓展方法 和 一些杂项拓展方法
    /// </summary>
    public static class ObjectExpand
    {
        /// <summary>
        /// 为此对象创建一个虚拟访问锚点
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="anchorName">锚点名称</param>
        public static void RegisterAnchor(this object obj, string anchorName)
        {
            VirtualAnchor anchor = new VirtualAnchor(anchorName);
            anchor.Owner = obj;
            AM.AddAnchor(anchor);
        }

        /// <summary>
        /// 判断指定索引是否在合法区间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        public static bool InRange<T>(this List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        /// <summary>
        /// 获取字典的深拷贝
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Dictionary<string, TValue> Copy<TValue>(this Dictionary<string, TValue> dic)
        {
            Dictionary<string, TValue> d = new();
            foreach (string key in dic.Keys)
            {
                d[key] = dic[key];
            }
            return d;
        }

        /// <summary>
        /// 尝试获取数组值，如果获取失败（越界），则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">索引</param>
        /// <param name="array"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T TryValue<T>(this T[] array, int index, T defaultValue)
        {
            if (index < 0 || index >= array.Length)
            {
                return defaultValue;
            }
            return array[index];
        }

        /// <summary>
        /// 尝试以字符串的形式将键值对打印出来
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="pair"></param>
        /// <param name="printDelegate"></param>
        public static void Print<TKey, TValue>(this KeyValuePair<TKey, TValue> pair,
            Action<string> printDelegate = null)
        {
            string txt = $"<{pair.Key?.ToString()}>:{pair.Value?.ToString()}";
            printDelegate?.Invoke(txt);
            Console.WriteLine(txt);
        }

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

        public static void Add<T>(this List<T> list, T[] array)
        {
            foreach (T item in array)
            {
                list.Add(item);
            }
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
        /// 将制定事件响应委托封装成可跳过的时间委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="function"></param>
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

        /// <summary>
        /// 将数组中全部初始化为一个默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="defaultValue"></param>
        public static T[] InitDefault<T>(this T[] array, T defaultValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = defaultValue;
            }
            return array;
        }
        /// <summary>
        /// 将该uid对象注册进管理器
        /// </summary>
        /// <param name="obj"></param>
        public static void Registry(this IUID obj)
        {
            UIDManager.Instance.Registry(obj);
        }
        /// <summary>
        /// 将该uid对象从管理器中注销
        /// </summary>
        /// <param name="obj"></param>
        public static void Unregistry(this IUID obj)
        {
            UIDManager.Instance.Unregistry(obj);
        }
    }
}