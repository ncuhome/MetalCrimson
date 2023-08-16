
using System.Collections.Generic;

namespace ER
{
    /// <summary>
    /// 通用拓展方法类
    /// </summary>
    public static class ObjectExpand
    {
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
        /// 为此对象创建一个虚拟访问锚点
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="anchorName">锚点名称</param>
        public static void RegisterAnchor(this object obj, string anchorName)
        {
            VirtualAnchor anchor = new VirtualAnchor(anchorName);
            anchor.Owner = obj;
            AnchorManager.Instance.AddAnchor(anchor);
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
        /// 判断值是否在指定范围
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// 判断值是否在指定范围
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(this double value, double min, double max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// 判断值是否在指定范围
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// 求数组的和
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int Sum(this int[] array)
        {
            int sum = 0;
            foreach (int item in array)
            {
                sum += item;
            }
            return sum;
        }

        /// <summary>
        /// 求数组的和
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static float Sum(this float[] array)
        {
            float sum = 0;
            foreach (float item in array)
            {
                sum += item;
            }
            return sum;
        }

        /// <summary>
        /// 求数组的和
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double Sum(this double[] array)
        {
            double sum = 0;
            foreach (double item in array)
            {
                sum += item;
            }
            return sum;
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
        /// 对数组与运算
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool AndAll(this bool[] array)
        {
            foreach (bool item in array)
            {
                if (!item) return false;
            }
            return true;
        }

        /// <summary>
        /// 对数组或运算
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool OrAll(this bool[] array)
        {
            foreach (bool item in array)
            {
                if (item) return true;
            }
            return false;
        }

        /// <summary>
        /// 对数组与运算
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool AndAll(this List<bool> array)
        {
            foreach (bool item in array)
            {
                if (!item) return false;
            }
            return true;
        }

        /// <summary>
        /// 对数组或运算
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool OrAll(this List<bool> array)
        {
            foreach (bool item in array)
            {
                if (item) return true;
            }
            return false;
        }
    }
}