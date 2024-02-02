// Ignore Spelling: Math

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 提供一些 与数学相关的拓展方法 和 容器操作的拓展方法
    /// </summary>
    public static class MathExpand
    {
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

        /// <summary>
        /// 获取指定向量到目标向量所经过的夹角(逆时针为正方向)
        /// </summary>
        public static float ClockAngle(this Vector2 defaultVector, Vector2 aimVector, float offset = 0)
        {
            float angle = Vector2.Angle(defaultVector, aimVector) + offset;
            if (GetRotateDir(defaultVector, aimVector) == RotateDir.Anticlockwise)
                return angle;
            else
                return -angle;
        }

        /// <summary>
        /// 获取旋转方向, 返回逆时针/顺时针/平行
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="aim"></param>
        /// <returns></returns>
        public static RotateDir GetRotateDir(Vector2 origin, Vector2 aim)
        {
            float f = origin.x * aim.y - aim.x * origin.y;
            if (f > 0)
                return RotateDir.Anticlockwise;
            else if (f < 0)
                return RotateDir.Clockwise;
            else
                return RotateDir.Parallel;
        }

        /// <summary>
        /// 获取该向量逆时针旋转angle度
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector2 Rotate(this Vector2 dir, float angle)
        {
            return new Vector2(dir.x * MathF.Cos(angle) - dir.y * MathF.Sin(angle), dir.x * MathF.Sin(angle) + dir.y * MathF.Cos(angle));
        }

        /// <summary>
        /// 获取指定向量的垂直向量(逆时针方向)
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Vector2 Vertical(this Vector2 dir)
        {
            Vector2 v = new Vector2(1, -dir.x / dir.y).normalized;
            if (GetRotateDir(dir, v) == RotateDir.Anticlockwise)
                return v;
            else
                return -v;
        }

        /// <summary>
        /// 修改指定向量的值
        /// </summary>
        /// <param name="vector">源向量</param>
        /// <param name="mx">是否修改x</param>
        /// <param name="my">是否修改y</param>
        /// <param name="mz">是否修改z</param>
        /// <param name="modVector">目标向量</param>
        /// <returns></returns>
        public static Vector3 Modify(this Vector3 vector, bool mx, bool my, bool mz, Vector3 modVector)
        {
            Vector3 v = vector;
            if (mx)
                v.x = modVector.x;
            if (my)
                v.y = modVector.y;
            if (mz)
                v.z = modVector.z;
            return v;
        }

        /// <summary>
        /// 判断指定二维向量是否在指定 矩形范围内
        /// </summary>
        /// <returns></returns>
        public static bool InRange(this Vector2 dir, Vector2 min, Vector2 max)
        {
            return dir.x >= min.x && dir.x <= max.x && dir.y >= min.y && dir.y <= max.y;
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
        /// <summary>
        /// 返回贝赛尔曲线插值
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <param name="k">控制点: 控制变化位置, 最后曲线将经过 起点终点和k点</param>
        /// <param name="t">最后返回x=t对应的y值, t 必须在0~1之间</param>
        /// <returns></returns>
        public static float QuadraticBezierInterpolate(Vector2 start,Vector2 end,Vector2 k,float t)
        {
            t = Mathf.Max(t, 0);
            t = Mathf.Min(t, 1);
            return Mathf.Pow(1 - t, 2) * start.y + 2 * (1 - t) * t * k.y + Mathf.Pow(t, 2) * end.y;

        }
    }
}