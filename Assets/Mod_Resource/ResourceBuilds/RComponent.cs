// Ignore Spelling: Mold

using ER.ItemStorage;
using ER.Resource;
using System.Collections.Generic;
using System.Linq;

namespace Mod_Resource
{
    /// <summary>
    /// 部件模具
    /// </summary>
    public class RComponent : IItemResource
    {
        #region 引用资源
        private string registryName;
        private string spriteName;//图片资源的注册名
        private string textRegistryName;//文本资源注册名
        private string displayPath;//相关文本路径
        private Dictionary<string,DescriptionInfo> descriptions;//描述

        #endregion 引用资源

        #region 本体属性

        private string moldName;//模具注册名
        private LinkageEffectStack[] leffs;//部件羁绊堆
        private float weight;//重量
        private int in_diameter;//IN端直径
        private int out_diameter;//OUT端直径
        private bool stackable;//是否可堆叠
        private int amountMax;//堆叠上限

        #endregion 本体属性

        #region 引用资源属性

        private SpriteResource sprite;//图片资源

        #endregion 引用资源属性

        /// <summary>
        /// IN端直径
        /// </summary>
        public int In_diameter => in_diameter;
        /// <summary>
        /// OUT端直径
        /// </summary>
        public int Out_diameter => out_diameter;
        /// <summary>
        /// 所带羁绊堆
        /// </summary>
        public LinkageEffectStack[] Leffs => leffs;
        public string RegistryName { get => registryName; }
        /// <summary>
        /// 图片资源注册名
        /// </summary>
        public string SpriteName { get => spriteName; }
        /// <summary>
        /// 文本资源注册名
        /// </summary>
        public string TextRegistryName => textRegistryName;

        /// <summary>
        /// 显示文本路径
        /// </summary>
        public string DisplayPath { get => displayPath; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get => descriptions[MetalCrimson.NameKey].text; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get => descriptions[MetalCrimson.DescriptionKey].text; }
        /// <summary>
        /// 所属模具
        /// </summary>
        public string MoldName => moldName;
        /// <summary>
        /// 图片资源
        /// </summary>
        public SpriteResource Sprite { get => sprite; }

        public DescriptionInfo[] Descriptions => descriptions.Values.ToArray();

        public bool Stackable => stackable;

        public int AmountMax => amountMax;

        public RComponent(RComponentInfo info)
        {
            registryName = info.registryName;
            spriteName = info.spriteName;
            textRegistryName = info.textRegistryName;
            displayPath = info.displayPath;
            moldName = info.mold;
            leffs = info.leff;
            weight = info.weight;
            in_diameter = info.in_diameter;
            out_diameter = info.out_diameter;
            stackable = info.stackable;
            amountMax = info.amountMax;

            Dictionary<string, string> displayText = GR.Get<LanguageResource>(textRegistryName)?.GetInfos(displayPath);
            descriptions = new Dictionary<string, DescriptionInfo>();
            if (displayText != null)
            {
                for(int i =0;i<info.descriptions.Length;i++)
                {
                    DescriptionInfo des = info.descriptions[i];
                    des.text = displayText[des.key];
                    descriptions[des.key] = des;
                }
            }
            sprite = GR.Get<SpriteResource>(spriteName);
        }
    }

    public struct RComponentInfo
    {
        public string registryName;
        public string spriteName;//图片资源的注册名
        public string textRegistryName;//文本资源注册名
        public string displayPath;//相关文本路径

        public DescriptionInfo[] descriptions;
        public string mold;//模具注册名
        public LinkageEffectStack[] leff;//部件羁绊堆
        public float weight;//重量
        public int in_diameter;//IN端直径
        public int out_diameter;//OUT端直径

        public bool stackable;//是否可堆叠
        public int amountMax;//堆叠上限
    }
}