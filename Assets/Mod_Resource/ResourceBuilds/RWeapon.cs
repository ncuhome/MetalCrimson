using ER.Resource;

namespace Mod_Resource
{
    public class RWeapon:IResource
    {
        private string registryName;
        /// <summary>
        /// 物品贴图资源
        /// </summary>

        private SpriteResource sprite;
        public string RegistryName { get => registryName; }
        /// <summary>
        /// 显示名称 资源路径
        /// </summary>
        private string displayNamePath;
        /// <summary>
        /// 显示名称
        /// </summary>
        private string displayName;

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