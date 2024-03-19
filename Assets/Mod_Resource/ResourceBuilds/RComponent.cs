// Ignore Spelling: mold leffs

using ER.ItemStorage;
using ER.Resource;
using System.Collections.Generic;
using System.Linq;

namespace Mod_Resource
{
    public class RComponent : IItemResource
    {
        #region 引用资源
        private string registryName;
        private string spriteName;//图片资源的注册名
        private string textRegistryName;//文本资源注册名
        private string displayPath;//相关文本路径
        #endregion 引用资源

        #region 本体属性

        private string moldName;//模具名称
        private LinkageEffectStack[] leffs;//默认羁绊
        private float weight;//默认部件重量
        private int in_diameter;//输入直径
        private int out_diameter;//输出直径
        private float cost;//建造花费

        #endregion 本体属性

        #region 引用资源属性

        private SpriteResource sprite;//图片资源

        #endregion 引用资源属性

        #region 物品堆

        private bool stackable;//是否可堆叠
        private int amountMax;//堆叠上限
        private Dictionary<string, DescriptionInfo> descriptions;//描述文本
        #endregion

        #region 访问

        public string RegistryName => registryName;

        /// <summary>
        /// 图片资源名称
        /// </summary>
        public string SpriteName => spriteName;
        /// <summary>
        /// 文本资源注册名
        /// </summary>
        public string TextRegistryName => textRegistryName;

        /// <summary>
        /// 显示文本路径
        /// </summary>
        public string DisplayPath { get => displayPath; }

        /// <summary>
        /// 模具注册名
        /// </summary>
        public string MoldName => moldName;

        /// <summary>
        /// 默认羁绊
        /// </summary>
        public LinkageEffectStack[] Leffs { get => leffs; }

        /// <summary>
        /// 默认部件重量
        /// </summary>
        public float Weight { get => weight; }

        /// <summary>
        /// 输入直径
        /// </summary>
        public int In_diameter { get => in_diameter; }

        /// <summary>
        /// 输出直径
        /// </summary>
        public int Out_diameter { get => out_diameter; }
        /// <summary>
        /// 建造花费
        /// </summary>
        public float Cost { get => cost; }  
        /// <summary>
        /// 图片资源
        /// </summary>
        public SpriteResource Sprite=> sprite;

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get => descriptions["name"].text; }

        /// <summary>
        /// 显示描述
        /// </summary>
        public string Description { get => descriptions["description"].text; }

        public DescriptionInfo[] Descriptions => descriptions.Values.ToArray();
        public bool Stackable => stackable;
        public int AmountMax => amountMax;

        #endregion 访问

        public RComponent(RComponentInfo info)
        {
            registryName = info.registryName;
            spriteName = info.spriteName;
            textRegistryName = info.textRegistryName;
            moldName = info.moldName;
            leffs = info.leffs;
            weight = info.weight;
            in_diameter = info.in_diameter;
            out_diameter = info.out_diameter;
            cost = info.cost;

            stackable = info.stackable;
            amountMax = info.amountMax;

            Dictionary<string,string> displayText = GR.Get<LanguageResource>(textRegistryName)?.GetInfos(displayPath);
            if(displayText!=null)
            {
                DescriptionInfo[] dif = info.descriptions;
                for (int i = 0; i < dif.Length; i++)
                {
                    string key = dif[i].key;
                    DescriptionInfo ifs = new DescriptionInfo();
                    ifs.key = key;
                    ifs.text = displayText[key];
                    ifs.tag = dif[i].tag;
                    descriptions[key] = ifs;
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
        public DescriptionInfo[] descriptions;//描述

        public string moldName;//模具名称
        public LinkageEffectStack[] leffs;//默认羁绊
        public float weight;//默认部件重量
        public int in_diameter;//输入直径
        public int out_diameter;//输出直径
        public float cost;//建造花费

        public bool stackable;//是否可堆叠
        public int amountMax;//堆叠上限
    }
}