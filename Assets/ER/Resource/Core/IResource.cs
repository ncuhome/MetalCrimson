// Ignore Spelling: color

using UnityEngine;

namespace ER.Resource
{
    /// <summary>
    /// 资源接口
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// 注册名称: 仅在读取配置中修改
        /// </summary>
        public string RegistryName { get; }
    }

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
    /// <summary>
    /// 音频资源
    /// </summary>
    public class AudioResource : IResource
    {
        private string registryName;
        private AudioClip clip;
        public string RegistryName { get => registryName; }
        /// <summary>
        /// AudioClip 资源对象
        /// </summary>
        public AudioClip Value => clip;

        public static explicit operator AudioClip(AudioResource source)
        {
            return source.Value;
        }
        public AudioResource(string _registryName, AudioClip origin)
        {
            clip = origin;
            registryName = _registryName;
        }
    }

    public class TextResource : IResource
    {
        private string registryName;
        private string text;
        public string RegistryName { get => registryName; }
        /// <summary>
        /// Text 资源对象
        /// </summary>
        public string Value => text;

        public static explicit operator string(TextResource source)
        {
            return source.Value;
        }
        public TextResource(string _registryName, string origin)
        {
            text = origin;
            registryName = _registryName;
        }
    }

}