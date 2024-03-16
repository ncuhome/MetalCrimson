﻿using ER.Resource;
using System.Collections.Generic;
using static Mod_Resource.RLinkageEffect;

namespace Mod_Resource
{
    /// <summary>
    /// 羁绊
    /// </summary>
    public class RLinkageEffect:IResource
    {
        public enum DisplayType
        {
            //词条超过 0 后，直接展示给玩家
            Enable,
            //不予展示
            Disable,
            //词条数量达到激活阈值时（即触发后），展示给玩家
            Hide,
        }
        private string registryName;
        private string spriteName;//图片资源的注册名
        private string textRegistryName;//文本资源注册名
        private string displayPath;//相关文本路径
        private string displayNameKey;//名称键
        private string descriptionKey;//描述键

        private DisplayType displayType;//显示方式

        private SpriteResource sprite;//图片资源
        private string displayName;//显示名称
        private string description;//显示描述


        public string RegistryName { get => registryName; }
        /// <summary>
        /// 图片资源的注册名
        /// </summary>
        public string SpriteName { get => spriteName; }
        /// <summary>
        /// 文本资源注册名
        /// </summary>
        public string TextRegistryName { get => textRegistryName; }
        /// <summary>
        /// 相关文本路径
        /// </summary>
        public string DisplayPath { get => displayPath; }
        /// <summary>
        /// 名称键
        /// </summary>
        public string DisplayNameKey { get => displayNameKey; }
        /// <summary>
        /// 描述键
        /// </summary>
        public string DescriptionKey { get => descriptionKey; }
        /// <summary>
        /// 显示方式
        /// </summary>
        public DisplayType DisplayType1 { get => displayType;}
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

        public RLinkageEffect(RLinkageEffectInfo info)
        {
            registryName = info.registryName;
            spriteName = info.spriteName;
            textRegistryName = info.textRegistryName;
            displayNameKey = info.displayNameKey;
            descriptionKey = info.descriptionKey;

            displayType = info.displayType;

            Dictionary<string, string> displayText = GR.Get<LanguageResource>(textRegistryName)?.GetInfos(displayPath);
            if (displayText != null)
            {
                displayName = displayText["displayName"];
                description = displayText["description"];
            }
            sprite = GR.Get<SpriteResource>(spriteName);
        }

        

        
    }

    public struct RLinkageEffectInfo
    {
        public string registryName;
        public string spriteName;//图片资源的注册名
        public string textRegistryName;//文本资源注册名
        public string displayPath;//相关文本路径
        public string displayNameKey;//名称键
        public string descriptionKey;//描述键

        public DisplayType displayType;//显示方式
    }

    //羁绊堆
    public struct LinkageEffectStack
    {
        /// <summary>
        /// 羁绊注册名
        /// </summary>
        public string registryName;
        /// <summary>
        /// 羁绊等级
        /// </summary>
        public int level;
    }
}