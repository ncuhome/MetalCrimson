using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.UI
{
    public class UIAnimationInfo
    {
        public enum animation
        { non, wait, running, pause, over }

        /// <summary>
        /// UI控件组件
        /// </summary>
        public RectTransform transform;

        /// <summary>
        /// 动画类型
        /// </summary>
        public UIAnimator.AnimationType type;

        /// <summary>
        /// 回调函数
        /// </summary>
        public Action callBack;

        /// <summary>
        /// 动画速率
        /// </summary>
        public float speed;

        /// <summary>
        /// 播放完毕后自动从列表中移除
        /// </summary>
        public bool autoRemove;

        /// <summary>
        /// 动画状态
        /// </summary>
        public animation status;

        /// <summary>
        /// 动画进度(0~1)
        /// </summary>
        public float progress;

        public UIAnimationInfo Copy()
        {
            return new UIAnimationInfo
            {
                transform = transform,
                type = type,
                callBack = callBack,
                speed = speed,
                autoRemove = autoRemove,
                status = status,
                progress = progress
            };
        }
    }

    public class UIAnimator : MonoSingleton<UIAnimator>
    {
        public enum AnimationType
        {
            /// <summary>
            /// 无动画
            /// </summary>
            None = 0,

            /// <summary>
            /// 左向盒子动画（打开）
            /// </summary>
            BoxOpen_Left,

            /// <summary>
            /// 右向盒子动画（打开）
            /// </summary>
            BoxOpen_Right,

            /// <summary>
            /// 上向盒子动画（打开）
            /// </summary>
            BoxOpen_Top,

            /// <summary>
            /// 下向盒子动画（打开）
            /// </summary>
            BoxOpen_Bottom,

            /// <summary>
            /// 左向盒子动画（关闭）
            /// </summary>
            BoxClose_Left,

            /// <summary>
            /// 右向盒子动画（关闭）
            /// </summary>
            BoxClose_Right,

            /// <summary>
            /// 上向盒子动画（关闭）
            /// </summary>
            BoxClose_Top,

            /// <summary>
            /// 下向盒子动画（关闭）
            /// </summary>
            BoxClose_Bottom,

            /// <summary>
            /// 淡出动画(仅限 Image 和 TMP_Text)
            /// </summary>
            FadeOut,

            /// <summary>
            /// 淡入动画(仅限 Image 和 TMP_Text)
            /// </summary>
            FadeIn,
        }

        #region 属性

        private Dictionary<RectTransform, UIAnimationInfo> infos = new Dictionary<RectTransform, UIAnimationInfo>();

        #endregion 属性

        #region 通常

        public static UIAnimationInfo CreateAnimationInfo(RectTransform transform, AnimationType type = AnimationType.None, Action callBack = null)
        {
            return new UIAnimationInfo
            {
                type = type,
                callBack = callBack,
                transform = transform,
                speed = 1,
                autoRemove = true,
                status = UIAnimationInfo.animation.wait,
                progress = 0f,
            };
        }

        #endregion 通常

        #region 盒子动画

        /// <summary>
        /// 添加动画列表
        /// </summary>
        /// <param name="transform">UI控件体</param>
        /// <param name="type">动画类型</param>
        /// <param name="callBack">回调函数</param>
        public UIAnimationInfo AddAnimation(RectTransform transform, AnimationType type, Action callBack = null)
        {
            UIAnimationInfo info = CreateAnimationInfo(transform, type, callBack);
            infos[transform] = info;
            return info;
        }

        public void SetAnimation(RectTransform transform, UIAnimationInfo info)
        {
            infos[transform] = info;
        }

        /// <summary>
        /// 移除指定控件的动画
        /// </summary>
        /// <param name="transform"></param>
        public void RemoveAnimation(RectTransform transform)
        {
            if (infos.Keys.Contains(transform))
            {
                infos.Remove(transform);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="type"></param>
        /// <param name="speed"></param>
        /// <param name="callBack"></param>
        public UIAnimationInfo GetAnimationInfo(RectTransform transform)
        {
            if (infos.Keys.Contains(transform)) { return infos[transform]; }
            return null;
        }

        /// <summary>
        /// 开启动画
        /// </summary>
        /// <param name="transform"></param>
        public void StartAnimation(RectTransform transform)
        {
            var info = infos[transform];
            info.progress = 0f;
            info.status = UIAnimationInfo.animation.running;
        }

        /// <summary>
        /// 开启动画并使用该动画信息的拷贝作为动画记录
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="_info"></param>
        public void StartAnimation(RectTransform transform, UIAnimationInfo _info)
        {
            var info = infos[transform] = _info.Copy();
            info.progress = 0f;
            info.status = UIAnimationInfo.animation.running;
        }

        /// <summary>
        /// 开启动画并使用该动画信息的拷贝作为动画记录
        /// </summary>
        /// <param name="_info"></param>
        public void StartAnimation(UIAnimationInfo _info)
        {
            if (_info == null) return;
            StartAnimation(_info.transform, _info);
        }

        /// <summary>
        /// 跳过动画
        /// </summary>
        /// <param name="transform"></param>
        public void SkipAnimation(RectTransform transform)
        {
            var info = infos[transform];
            info.progress = 1f;
            info.status = UIAnimationInfo.animation.running;
        }

        /// <summary>
        /// 暂停动画
        /// </summary>
        /// <param name="transform"></param>
        public void PauseAnimation(RectTransform transform)
        {
            var info = infos[transform];
            info.status = UIAnimationInfo.animation.pause;
        }

        #endregion 盒子动画



        #region 内部函数

        private void BoxAnimation_Left_Open(UIAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, 0);
            info.transform.anchorMax = new Vector2(info.progress, 1);
        }

        private void BoxAnimation_Left_Close(UIAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, 0);
            info.transform.anchorMax = new Vector2(1 - info.progress, 1);
        }

        private void BoxAnimation_Right_Open(UIAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(1 - info.progress, 0);
            info.transform.anchorMax = new Vector2(1, 1);
        }

        private void BoxAnimation_Right_Close(UIAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(info.progress, 0);
            info.transform.anchorMax = new Vector2(1, 1);
        }

        private void BoxAnimation_Top_Open(UIAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, 1 - info.progress);
            info.transform.anchorMax = new Vector2(1, 1);
        }

        private void BoxAnimation_Top_Close(UIAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, info.progress);
            info.transform.anchorMax = new Vector2(1, 1);
        }

        private void BoxAnimation_Bottom_Open(UIAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, 0);
            info.transform.anchorMax = new Vector2(1, info.progress);
        }

        private void BoxAnimation_Bottom_Close(UIAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, 0);
            info.transform.anchorMax = new Vector2(1, 1 - info.progress);
        }

        private void FadeInAnimation(UIAnimationInfo info)
        {
            Image image = info.transform.GetComponent<Image>();
            TMP_Text text = info.transform.GetComponent<TMP_Text>();
            if (image != null)
            {
                image.color = image.color.ModifyAlpha(info.progress);
            }
            if (text != null)
            {
                text.color = text.color.ModifyAlpha(info.progress);
            }
        }

        private void FadeOutAnimation(UIAnimationInfo info)
        {
            Image image = info.transform.GetComponent<Image>();
            TMP_Text text = info.transform.GetComponent<TMP_Text>();
            if (image != null)
            {
                image.color = image.color.ModifyAlpha(1 - info.progress);
            }
            if (text != null)
            {
                text.color = text.color.ModifyAlpha(1 - info.progress);
            }
        }

        #endregion 内部函数

        #region Unity

        private void Update()
        {
            List<RectTransform> trfs = new List<RectTransform>();//用于记录本帧完成动画的对象
            List<RectTransform> dest = new List<RectTransform>();
            foreach (UIAnimationInfo info in infos.Values)
            {
                if (info.status == UIAnimationInfo.animation.running)
                {
                    if (info.transform == null)
                    {
                        Debug.Log("移除移除移除移除移除移除移除移除");
                        dest.Add(info.transform);
                        continue;
                    }

                    switch (info.type)
                    {
                        case AnimationType.None:
                            break;

                        case AnimationType.BoxOpen_Left:
                            BoxAnimation_Left_Open(info);
                            break;

                        case AnimationType.BoxClose_Left:
                            BoxAnimation_Left_Close(info);
                            break;

                        case AnimationType.BoxOpen_Right:
                            BoxAnimation_Right_Open(info);
                            break;

                        case AnimationType.BoxClose_Right:
                            BoxAnimation_Right_Close(info);
                            break;

                        case AnimationType.BoxOpen_Top:
                            BoxAnimation_Top_Open(info);
                            break;

                        case AnimationType.BoxClose_Top:
                            BoxAnimation_Top_Close(info);
                            break;

                        case AnimationType.BoxOpen_Bottom:
                            BoxAnimation_Bottom_Open(info);
                            break;

                        case AnimationType.BoxClose_Bottom:
                            BoxAnimation_Bottom_Close(info);
                            break;

                        case AnimationType.FadeIn:
                            FadeInAnimation(info);
                            break;

                        case AnimationType.FadeOut:
                            FadeOutAnimation(info);
                            break;
                    }
                    if (info.progress >= 1)
                    {
                        info.status = UIAnimationInfo.animation.over;
                        trfs.Add(info.transform);
                    }
                    info.progress = Mathf.Clamp(info.progress + Time.deltaTime * info.speed, 0f, 1f);
                }
            }
            //调用回调函数
            foreach (RectTransform transform in trfs)
            {
                UIAnimationInfo info = infos[transform];
                if (info.callBack != null) { info.callBack(); }
                if (info.autoRemove) { infos.Remove(info.transform); }
            }
            foreach (RectTransform rt in dest)
            {
                infos.Remove(rt);
            }
        }

        #endregion Unity
    }
}