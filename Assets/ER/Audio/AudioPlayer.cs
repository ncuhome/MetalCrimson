using System;
using UnityEngine;

namespace ER
{
    public class AudioPlayer:Water
    {
        #region 枚举
        public enum PlayMode
        {
            /// <summary>
            /// 单次播放
            /// </summary>
            Single,
            /// <summary>
            /// 单次播放后隐藏
            /// </summary>
            SingleHide,
            /// <summary>
            /// 循环播放
            /// </summary>
            Loop,
        }
        public enum Effect
        {
            /// <summary>
            /// 无效果
            /// </summary>
            None,
            /// <summary>
            /// 低频
            /// </summary>
            Low,
            /// <summary>
            /// 高频
            /// </summary>
            High,
        }
        #endregion

        #region 组件
        public AudioSource source;
        public AudioHighPassFilter highPassFilter;
        public AudioLowPassFilter lowPassFilter;
        #endregion

        #region 私有
        private Effect effect = Effect.None;
        private PlayMode mode = PlayMode.Single;
        private bool autoHide = false;//自动隐藏
        private float autoFadeOutTime = -1f;
        private float timer = 0f;
        private Action callback = null;//委托锚点
        private Action funcAnchor = null;//委托锚点
        private bool fading = false;
        private float fade_start = 0f;
        private float fade_end = 1f;
        private float fade_time = 1f;
        private float fade_k = 1f;
        #endregion

        #region 属性
        /// <summary>
        /// 播放模式
        /// </summary>
        public PlayMode Mode
        {
            get { return mode; }
            set 
            {
                mode = value; 
                switch(mode)
                {
                    case PlayMode.Single:
                        source.loop = false;
                        autoHide = false;
                        break;
                    case PlayMode.SingleHide:
                        source.loop = false;
                        autoHide = true;
                        break;
                    case PlayMode.Loop:
                        source.loop = true;
                        autoHide = false;
                        break;
                }
            }
        }
        /// <summary>
        /// 音频效果
        /// </summary>
        public Effect AudioEffect
        {
            get => effect;
            set
            {
                effect = value;
                switch(effect)
                {
                    case Effect.None:
                        highPassFilter.enabled = false; 
                        lowPassFilter.enabled = false;
                        break;
                    case Effect.Low:
                        highPassFilter.enabled = false;
                        lowPassFilter.enabled = true;
                        break;
                    case Effect.High:
                        highPassFilter.enabled = true;
                        lowPassFilter.enabled = false;
                        break;
                }
            }
        }
        /// <summary>
        /// 音量
        /// </summary>
        public float Volume
        {
            get=>source.volume;
            set => source.volume = value;   
        }
        /// <summary>
        /// 音频资源
        /// </summary>
        public AudioClip Clip { get=>source.clip; set => source.clip = value; }
        /// <summary>
        /// 自动淡出过渡时间(循环次模式下失效) 此值小于等于0表示不过渡
        /// </summary>
        public float AutoFadeOutTime
        {
            get => autoFadeOutTime;
            set
            {
                autoFadeOutTime = value;
                if(autoFadeOutTime>0)
                {
                    funcAnchor = FadeOutCheck;
                }
                else
                {
                    funcAnchor = null;
                }
            }
        }
        #endregion

        #region 操作
        /// <summary>
        /// 淡入播放音频
        /// </summary>
        /// <param name="start">初始音量</param>
        /// <param name="aim">最终音量</param>
        /// <param name="time">过渡时间</param>
        public void FadeIn(float start,float aim,float time,float k = 0.95f)
        {
            fade_start = start;
            fade_end = aim;
            fade_time = time;
            fade_k = k;
            fading = true;
            timer = 0f;
            source.volume = fade_start;
            callback = null;
            enabled = true;
            gameObject.SetActive(true);
            source.Play();
        }
        /// <summary>
        /// 淡出结束音频
        /// </summary>
        /// <param name="start">初始音量</param>
        /// <param name="aim">最终音量</param>
        /// <param name="time">过渡时间</param>
        public void FadeOut(float start, float aim, float time,float k = 0.05f)
        {
            fade_start = start;
            fade_end = aim;
            fade_time = time;
            fade_k= k;
            fading = true;
            timer = 0f;
            source.volume = fade_start;
            callback = FadeOutResult;
        }
        /// <summary>
        /// 直接播放
        /// </summary>
        public void Play()
        {
            source.Play();
        }
        /// <summary>
        /// 播放暂停
        /// </summary>
        public void Pause()
        {
            source.Pause();
        }
        /// <summary>
        /// 播放停止
        /// </summary>
        public void Stop()
        {
            source.Stop();
        }
        #endregion
        private void FadeOutResult()
        {
            source.Stop();
            if (autoHide)
            {
                Destroy();
            }
        }

        private void FadeOutCheck()
        {
            if(source.clip.length - source.time <= autoFadeOutTime && source.isPlaying)
            {
                FadeOut(Volume,0,autoFadeOutTime);
                funcAnchor = null;
            }
        }

        public override void ResetState()
        {
            source.clip = null;
            source.playOnAwake = false;
            source.mute = false;
            source.volume = 1;
            source.priority = 128;
            source.pitch = 1;
            source.panStereo = 0f;

            autoFadeOutTime = -1f;
            Mode = PlayMode.Single;
            AudioEffect = Effect.None;
            callback = null;
            fading = false;
        }

        protected override void OnHide()
        {
            
        }

        private void Update()
        {
            funcAnchor?.Invoke();
            if (fading)
            {
                if(source.clip.length >= source.time)//未播放结束
                {
                    if(source.isPlaying)
                    {
                        timer += Time.deltaTime;
                    }
                }
                else
                {
                    timer += Time.deltaTime;
                }
                source.volume = MathExpand.QuadraticBezierInterpolate(
                    new Vector2(0, fade_start),
                    new Vector2(1, fade_end),
                    new Vector2(fade_k, (fade_start + fade_end) / 10),
                    timer / fade_time);
                if (timer>=fade_time)
                {
                    callback?.Invoke();
                }
            }
            else
            {
                if (autoHide&& source.time >= source.clip.length)//播放完毕
                {
                    Destroy();
                }
            }
        }
    }

    
}