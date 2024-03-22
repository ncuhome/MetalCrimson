// Ignore Spelling: Mold

using ER.Resource;
using System.Collections.Generic;

namespace Mod_Resource
{
    /// <summary>
    /// 部件模具
    /// </summary>
    public class RComponentMold:IResource
    {
        #region 引用资源
        private string registryName;
        private string spriteName;//图片资源的注册名
        private string textRegistryName;//文本资源注册名
        private string displayPath;//相关文本路径

        #endregion 引用资源

        #region 本体属性

        private string typeName;//模具类型注册名
        private string targetName;//目标部件注册名
        private float multi_weight;//重量系数:模具重量将会受此属性影响
        private float cost;//建造花费

        #endregion 本体属性

        #region 引用资源属性

        private SpriteResource sprite;//图片资源
        private string displayName;//显示名称
        private string description;//显示描述

        #endregion 引用资源属性

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
        /// 所属模具类型资源注册名
        /// </summary>
        public string TypeName { get => typeName; }
        /// <summary>
        /// 目标导出模具资源注册名
        /// </summary>
        public string TargetName { get => targetName; }
        /// <summary>
        /// 重量系数
        /// </summary>
        public float Multi_weight { get => multi_weight; }

        /// <summary>
        /// 建造花费
        /// </summary>
        public float Cost { get => cost; }
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

        public RComponentMold(RComponentMoldInfo info)
        {
            registryName = info.registryName;
            spriteName = info.spriteName;
            textRegistryName = info.textRegistryName; 
            displayPath = info.displayPath;

            typeName = info.type;
            targetName = info.target;
            multi_weight = info.multi_weight;
            cost = info.cost;

            Dictionary<string, string> displayText = GR.Get<LanguageResource>(textRegistryName)?.GetInfos(displayPath);
            if (displayText != null)
            {
                if(displayText.TryGetValue(MetalCrimson.NameKey,out string value))
                {
                    displayName = value;
                }
                else
                {
                    displayName = "error:loss";
                }
                if (displayText.TryGetValue(MetalCrimson.DescriptionKey, out string value_2))
                {
                    description = value_2;
                }
                else
                {
                    displayName = "error:loss";
                }
            }
            sprite = GR.Get<SpriteResource>(spriteName);
        }
    }

    public struct RComponentMoldInfo
    {
        public string registryName;
        public string spriteName;//图片资源的注册名
        public string textRegistryName;//文本资源注册名
        public string displayPath;//相关文本路径

        public string type;//模具类型注册名
        public string target;//目标部件注册名
        public float multi_weight;//重量系数:模具重量将会受此属性影响
        public float cost;//建造花费
    }
}