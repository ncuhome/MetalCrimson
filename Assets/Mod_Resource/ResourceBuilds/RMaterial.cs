using ER.Resource;
using Mod_Resource;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Resource
{
    //物品:材料
    public class RMaterial : IResource
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

        private LinkageEffectStack[] leffs;//默认羁绊

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
        public string DisplayName { get => displayName; }

        /// <summary>
        /// 显示描述
        /// </summary>
        public string Description { get => description; }

        #endregion 访问

        public RMaterial(RMaterialInfo info)
        {
            registryName = info.registryName;
            spriteName = info.spriteName;
            textRegistryName = info.textRegistryName;
            displayNameKey = info.displayNameKey;
            descriptionKey = info.descriptionKey;
            leffs = info.leffs;

            Dictionary<string, string> displayText = GR.Get<LanguageResource>(textRegistryName)?.GetInfos(displayPath);
            if (displayText != null)
            {
                displayName = displayText["displayName"];
                description = displayText["description"];
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
        public string displayNameKey;//名称键
        public string descriptionKey;//描述键

        public LinkageEffectStack[] leffs;//默认羁绊
    }
}