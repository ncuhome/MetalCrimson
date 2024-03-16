// Ignore Spelling: mold leffs

using ER.Resource;
using System.Collections.Generic;

namespace Mod_Resource
{
    public class RComponent : IResource
    {
        #region 引用资源
        private string registryName;
        private string spriteName;//图片资源的注册名
        private string textRegistryName;//文本资源注册名
        private string displayPath;//相关文本路径
        private string displayNameKey;//名称键
        private string descriptionKey;//描述键

        #endregion 引用资源

        #region 本体属性

        private string moldName;//模具名称
        private LinkageEffectStack[] leffs;//默认羁绊
        private float weight;//默认部件重量
        private int in_diameter;//输入直径
        private int out_diameter;//输出直径

        #endregion 本体属性

        #region 引用资源属性

        /// <summary>
        /// 图片资源
        /// </summary>
        private SpriteResource sprite;
        private string displayName;//显示名称
        private string description;//显示描述

        #endregion 引用资源属性

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
        /// 显示名称键
        /// </summary>
        public string DisplayNameKey { get => displayNameKey; }

        /// <summary>
        /// 描述键
        /// </summary>
        public string DescriptionKey { get => descriptionKey; }

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
        /// 图片资源
        /// </summary>
        public SpriteResource Sprite=> sprite;

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get => displayName; }

        /// <summary>
        /// 显示描述
        /// </summary>
        public string Description { get => description; }

        #endregion 访问

        public RComponent(RComponentInfo info)
        {
            registryName = info.registryName;
            spriteName = info.spriteName;
            textRegistryName = info.textRegistryName;
            displayNameKey = info.displayNameKey;
            descriptionKey = info.descriptionKey;
            moldName = info.moldName;
            leffs = info.leffs;
            weight = info.weight;
            in_diameter = info.in_diameter;
            out_diameter = info.out_diameter;

            Dictionary<string,string> displayText = GR.Get<LanguageResource>(textRegistryName)?.GetInfos(displayPath);
            if(displayText!=null)
            {
                displayName = displayText["displayName"];
                description = displayText["description"];
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
        public string displayNameKey;//名称键
        public string descriptionKey;//描述键

        public string moldName;//模具名称
        public LinkageEffectStack[] leffs;//默认羁绊
        public float weight;//默认部件重量
        public int in_diameter;//输入直径
        public int out_diameter;//输出直径
    }
}