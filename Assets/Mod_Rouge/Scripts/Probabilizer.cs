// Ignore Spelling: Probabilizer

using UnityEngine;

namespace Mod_Rouge
{
    /// <summary>
    /// 概率器
    /// </summary>
    public class Probabilizer
    {
        /// <summary>
        /// 获取一个随机结果
        /// 自动纠正概率溢出/不足
        /// </summary>
        /// <param name="indexStart">选项开始索引</param>
        /// <param name="probabilities">选项概率值</param>
        /// <returns></returns>
        public static int Parse(int indexStart, params float[] probabilities)
        {
            if (probabilities.Length >= 2)
            {
                for (int i = 1; i < probabilities.Length; i++)
                {
                    //ConsolePanel.Print($"概率a[{(RoomType)i}]：{probabilities[i]}");
                    probabilities[i] += probabilities[i - 1];
                    //ConsolePanel.Print($"概率b[{(RoomType)i}]：{probabilities[i]}");
                }
                float value = Random.Range(0, probabilities[probabilities.Length - 1]);
                //ConsolePanel.Print($"骰子值：{value}");
                for (int i = 0; i < probabilities.Length; i++)
                {
                    if (value < probabilities[i]) return i + indexStart;
                }
            }
            return indexStart;
        }
    }
}