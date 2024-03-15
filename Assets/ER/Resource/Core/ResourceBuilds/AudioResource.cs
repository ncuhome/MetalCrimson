using UnityEngine;

namespace ER.Resource
{
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
}