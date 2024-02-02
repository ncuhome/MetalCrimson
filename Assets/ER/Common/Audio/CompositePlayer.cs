using System;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 复合音乐播放器(切换播放时自动过渡)
    /// </summary>
    public class CompositePlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioPlayer player_1;

        [SerializeField]
        private AudioPlayer player_2;

        private int activeIndex = 0;
        [SerializeField]
        private float volume = 1f;
        /// <summary>
        /// 通常音量
        /// </summary>
        private float Volume
        {
            get => volume;
            set => volume = value;
        }
        /// <summary>
        /// 淡入时间
        /// </summary>
        public float fadeIn = 5f;
        /// <summary>
        /// 淡出时间
        /// </summary>
        public float fadeOut = 5f;

        /// <summary>
        /// 当前激活的播放器索引
        /// </summary>
        public int ActivePlayerIndex => activeIndex;
        /// <summary>
        /// 获取当前激活的播放器
        /// </summary>
        /// <returns></returns>
        public AudioPlayer GetActivePlayer()
        {
            switch (activeIndex)
            {
                case 1: return player_1;
                case 2: return player_2;
                default: return null;
            }
        }

        public void Init()
        {
            player_1.Mode = AudioPlayer.PlayMode.Loop;
            player_1.ResetState();
            player_2.Mode = AudioPlayer.PlayMode.Loop;
            player_2.ResetState();
        }

        public void Play()
        {
            GetActivePlayer()?.FadeIn(0, Volume,fadeIn);
        }
        public void Pause()
        {
            GetActivePlayer()?.Pause();
        }
        public void Stop()
        {
            AudioPlayer p = GetActivePlayer();
            if(p != null)
            {
                p.FadeOut(p.Volume, 0, fadeOut);
            }
            activeIndex = 0;
        }
        public void Play(AudioClip clip)
        {
            if(_clip==null)
            {
                AudioPlayer p = GetActivePlayer();
                if (p != null)
                {
                    p.FadeOut(p.Volume, 0, fadeOut);
                }
                activeIndex++;
                if (activeIndex > 2)
                {
                    activeIndex = 1;
                }//跳转下条索引
            }
            _clip = clip;
            timer = (fadeIn + fadeOut) / 4;
        }
        private float timer;
        private AudioClip _clip;

        private void Update()
        {
            if(_clip!=null)
            {
                timer -= Time.deltaTime;
                if(timer<0)
                {
                    AudioPlayer player = GetActivePlayer();
                    player.Clip = _clip;
                    player.FadeIn(0, Volume, fadeIn);
                    _clip = null;
                }
            }
        }
    }
}