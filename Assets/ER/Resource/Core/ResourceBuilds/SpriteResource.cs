using UnityEngine;

namespace ER.Resource
{
    /// <summary>
    /// 图片资源
    /// </summary>
    public class SpriteResource : IResource
    {
        private string registryName;
        public string RegistryName { get => registryName; }

        private Sprite sprite;
        /// <summary>
        /// 色相遮罩
        /// </summary>
        private Color color_mask;

        /// <summary>
        /// Sprite资源对象
        /// </summary>
        public Sprite Value => sprite;
        /// <summary>
        /// 色相遮罩
        /// </summary>
        public Color Mask => color_mask;
        /// <summary>
        /// 强转重载
        /// </summary>
        /// <param name="source"></param>

        public static explicit operator Sprite(SpriteResource source)
        {
            return source.Value;
        }

        public SpriteResource(string _registryName, Sprite origin)
        {
            sprite = origin;
            registryName = _registryName;
        }
        public SpriteResource(string _registryName, Sprite origin, Color _color_mask)
        {
            sprite = origin;
            registryName = _registryName;
            color_mask = _color_mask;
        }
    }
}