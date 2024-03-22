// Ignore Spelling: Mold

using ER.Resource;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Mod_Resource
{
    /// <summary>
    /// 部件模具类型
    /// </summary>
    public class RComponentMoldType : IResource
    {
        #region 引用资源

        private string registryName;
        private string spriteName;//图片资源的注册名
        private string textRegistryName;//文本资源注册名
        private string displayPath;//相关文本路径

        #endregion 引用资源

        #region 本体属性


        #endregion 本体属性

        #region 引用资源属性

        private SpriteResource sprite;//图片资源
        private string displayName;//显示名称
        private string description;//显示描述

        #endregion 引用资源属性

        public string RegistryName { get => registryName; }
        /// <summary>
        /// 图片资源的注册名
        /// </summary>
        public string SpriteName { get => spriteName;}
        /// <summary>
        /// 文本资源注册名
        /// </summary>
        public string TextRegistryName { get => textRegistryName; }
        /// <summary>
        /// 相关文本路径
        /// </summary>
        public string DisplayPath { get => displayPath; }
        /// <summary>
        /// 图片资源
        /// </summary>
        public SpriteResource Sprite { get => sprite; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get => displayName; }
        /// <summary>
        /// 显示描述
        /// </summary>
        public string Description { get => description; }

        public RComponentMoldType(RComponentMoldTypeInfo info)
        {
            registryName = info.registryName;
            spriteName = info.spriteName;
            textRegistryName = info.textRegistryName;
            displayPath = info.displayPath;

            Dictionary<string, string> displayText = GR.Get<LanguageResource>(textRegistryName)?.GetInfos(displayPath);
            if (displayText != null)
            {
                displayName = displayText[MetalCrimson.NameKey];
                description = displayText[MetalCrimson.DescriptionKey];
            }
            sprite = GR.Get<SpriteResource>(spriteName);
        }
    }

    public struct RComponentMoldTypeInfo
    {
        public string registryName;
        public string spriteName;//图片资源的注册名
        public string textRegistryName;//文本资源注册名
        public string displayPath;//相关文本路径


    }
}