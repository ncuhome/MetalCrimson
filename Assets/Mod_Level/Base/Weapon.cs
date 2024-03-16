using ER.Resource;
using Mod_Resource;
using UnityEngine;

namespace Mod_Level
{
    class Weapon
    {
        private Sprite sprite;

        /// <summary>
        /// 组成部件;
        /// </summary>
        private RComponent[] components;
        /// <summary>
        /// 攻击力
        /// </summary>
        private float attack;
        /// <summary>
        /// 武器耐久
        /// </summary>
        public float durable;
        /// <summary>
        /// 武器耐久上限
        /// </summary>
        public float durableMax;
        /// <summary>
        /// 伤害类型
        /// </summary>
        public enum DamageType
        {
            /// <summary>
            /// 钝器伤害
            /// </summary>
            Blunt,
            /// <summary>
            /// 锐器伤害
            /// </summary>
            Sharp,
            /// <summary>
            /// 魔法伤害
            /// </summary>
            Magic
        }
        /// <summary>
        /// 伤害类型
        /// </summary>
        public DamageType damageType;
        /// <summary>
        /// 武器整体重量(计算公式等待嵌入)
        /// </summary>
        public float Weight { get; }
        /// <summary>
        /// 额外回血速率
        /// </summary>
        public float extraHPHealSpeed;
    }
}