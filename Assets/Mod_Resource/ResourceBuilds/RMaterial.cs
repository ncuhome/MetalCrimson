using ER.ItemStorage;
using ER.Resource;
using Mod_Forge;
using Mod_Resource;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mod_Resource
{
    //物品:材料
    public class RMaterial : IItemResource
    {
        #region 引用资源
        private string registryName;
        private string spriteName;//图片资源的注册名
        private string textRegistryName;//文本资源注册名
        private string displayPath;//相关文本路径

        #endregion 引用资源

        #region 本体属性

        private LinkageEffectStack[] leffs;//默认羁绊
        private string color;//16进制: 金属颜色
        private TemperatureColor suitable;//适宜锻造温度

        #endregion 本体属性

        #region 引用资源属性

        /// <summary>
        /// 图片资源
        /// </summary>
        private SpriteResource sprite;

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
        /// 默认羁绊
        /// </summary>
        public LinkageEffectStack[] Leffs { get => leffs; }

        /// <summary>
        /// 图片资源
        /// </summary>
        public SpriteResource Sprite => sprite;

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get => descriptions["name"].text;  }

        /// <summary>
        /// 显示描述
        /// </summary>
        public string Description { get => descriptions["description"].text;  }
        /// <summary>
        /// 16进制: 金属颜色
        /// </summary>
        public string Color { get => color; }
        /// <summary>
        /// 适宜锻造温度
        /// </summary>
        public TemperatureColor Suitable =>suitable;

        public DescriptionInfo[] Descriptions => descriptions.Values.ToArray();

        public bool Stackable => stackable;

        public int AmountMax => amountMax;



        #endregion 访问

        public RMaterial(RMaterialInfo info)
        {
            registryName = info.registryName;
            spriteName = info.spriteName;
            textRegistryName = info.textRegistryName;

            leffs = info.leffs;
            color = info.color;
            suitable = info.suitable;
            stackable = info.stackable;
            amountMax = info.amountMax;


            Dictionary<string, string> displayText = GR.Get<LanguageResource>(textRegistryName)?.GetInfos(displayPath);
            if (displayText != null)
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

    public struct RMaterialInfo
    {
        public string registryName;
        public string spriteName;//图片资源的注册名
        public string textRegistryName;//文本资源注册名
        public string displayPath;//相关文本路径

        public LinkageEffectStack[] leffs;//默认羁绊
        public string color;//16进制: 金属颜色
        public TemperatureColor suitable;//适宜锻造温度

        public bool stackable;//是否可堆叠
        public int amountMax;//堆叠上限
        public DescriptionInfo[] descriptions;//描述文本
    }
}