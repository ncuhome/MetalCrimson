using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace ER
{
    /// <summary>
    /// 随机唱片信息
    /// </summary>
    public struct RandomClipInfo
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string registryName;

        /// <summary>
        /// 音频资源
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// 权重(需要大于零)
        /// </summary>
        private float _weight;

        /// <summary>
        /// 权重(非负数)
        /// </summary>
        public float weight
        {
            get => _weight;
            set
            {
                _weight = Mathf.Max(value, 0);
            }
        }
    }

    /// <summary>
    /// 唱片组:
    /// 可以先定义唱片的 名称,权重信息, 最后给 SoundManager 填充具体的 AudioClip 信息
    /// </summary>
    public class AudioClipGroup
    {
        /// <summary>
        /// 唱片组
        /// </summary>
        public RandomClipInfo[] infos;

        /// <summary>
        /// 唱片组名称
        /// </summary>
        public string name;
    }

    /// <summary>
    /// 随机音效播放器, 播放时随机播放列表中的其中一种音效
    /// </summary>
    public class RandomPlayer : Water
    {
        private List<RandomClipInfo> clips = new List<RandomClipInfo>();
        private float[] anchors;//权重锚点(0~1)
        public AudioPlayer player;

        private Random random;

        public void SetClips(AudioClipGroup group)
        {
            clips.Clear();
            clips.Add(group.infos);
        }

        /// <summary>
        /// 添加新的片段
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="weight"></param>
        public void AddClip(string registryName, AudioClip clip, float weight)
        {
            clips.Add(new RandomClipInfo
            {
                registryName = registryName,
                clip = clip,
                weight = weight
            });
            UpdateAnchor();
        }

        /// <summary>
        /// 添加新的片段
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="weight"></param>
        public void AddClip(string registryName, float weight)
        {
            clips.Add(new RandomClipInfo
            {
                registryName = registryName,
                clip = SoundManager.Instance.GetClip(registryName),
                weight = weight
            });
            UpdateAnchor();
        }

        /// <summary>
        /// 添加新的片段
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="weight"></param>
        public void AddClip(AudioClip clip, float weight)
        {
            clips.Add(new RandomClipInfo
            {
                registryName = string.Empty,
                clip = clip,
                weight = weight
            });
            UpdateAnchor();
        }

        public void ClearClip()
        {
            clips.Clear();
        }

        /// <summary>
        /// 更新概率锚点
        /// </summary>
        public void UpdateAnchor()
        {
            anchors = new float[clips.Count];
            float sum = 0f;
            foreach (RandomClipInfo randomClipInfo in clips)
            {
                sum += randomClipInfo.weight;
            }
            for (int i = 0; i < anchors.Length; i++)
            {
                if (i == 0)
                {
                    anchors[i] = clips[i].weight / sum;
                }
                else
                {
                    anchors[i] = anchors[i - 1] + clips[i].weight / sum;
                }
            }
        }

        /// <summary>
        /// 随机播放音效
        /// </summary>
        public void Play()
        {
            double number = random.NextDouble();
            int i = 0;
            AudioClip clip = null;
            Debug.Log($"number:{number}");
            while (i < anchors.Length)
            {
                //Debug.Log($"anchors[{i}]:{anchors[i]}");
                if (number < anchors[i])
                {
                    clip = clips[i].clip;
                    break;
                }
                i++;
            }
            player.Clip = clip;
            player.Play();
        }

        public override void ResetState()
        {
            player.ResetState();
            player.Mode = AudioPlayer.PlayMode.Single;
            clips.Clear();
            anchors = new float[0];
        }

        protected override void OnHide()
        {
        }

        private void Start()
        {
            random =new Random(DateTime.Now.Millisecond);
        }
    }
}