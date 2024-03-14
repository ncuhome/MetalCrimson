// Ignore Spelling: obj

using System;
using System.IO;
using UnityEngine;

namespace ER.ResourcePacker
{
    /// <summary>
    /// 资源包信息
    /// </summary>
    public struct ResourcePackInfo
    {
        /// <summary>
        /// 消息输出接口
        /// </summary>
        public static event Action<string> Output = message => Debug.Log(message);

        /// <summary>
        /// 包所在目录(全)
        /// </summary>
        public string PackPath;

        /// <summary>
        /// 包名称
        /// </summary>
        public string PackName;

        /// <summary>
        /// 包版本
        /// </summary>
        public string PackVersion;

        /// <summary>
        /// 包作者
        /// </summary>
        public string PackAuthor;

        /// <summary>
        /// 封面图片路径
        /// </summary>
        public string ImagePath;

        /// <summary>
        /// 包描述
        /// </summary>
        public string PackDescription;

        /// <summary>
        /// 获取包描述文件的路径
        /// </summary>
        public string DescriptionPath => ERinbone.Combine(PackPath, ResourcePack.IllustrationFileName);

        /// <summary>
        /// 获取包索引文件的路径
        /// </summary>
        public string IndexerPath => ERinbone.Combine(PackPath, ResourcePack.IndexerFileName);

        public void Print()
        {
            Output($"Path:{PackPath}");
            Output($"name:{PackName}");
            Output($"version:{PackVersion}");
            Output($"author:{PackAuthor}");
            Output($"image:{ImagePath}");
            Output($"description:{PackDescription}");
        }

        public static ResourcePackInfo Empty
        {
            get
            {
                return new ResourcePackInfo
                {
                    PackPath = string.Empty,
                    PackName = string.Empty,
                    PackVersion = string.Empty,
                    PackAuthor = string.Empty,
                    ImagePath = string.Empty,
                    PackDescription = string.Empty
                };
            }
        }

        public static bool operator ==(ResourcePackInfo obj, ResourcePackInfo obj2)
        {
            if (obj.PackPath != obj2.PackPath) return false;
            if (obj.PackName != obj2.PackName) return false;
            if (obj.PackVersion != obj2.PackVersion) return false;
            if (obj.PackAuthor != obj2.PackAuthor) return false;
            if (obj.ImagePath != obj2.ImagePath) return false;
            if (obj.PackDescription != obj2.PackDescription) return false;

            return true;
        }

        public static bool operator !=(ResourcePackInfo obj, ResourcePackInfo obj2)
        {
            return !(obj == obj2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}