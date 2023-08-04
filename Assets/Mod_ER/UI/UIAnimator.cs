using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ER.UI
{
    public class UIBoxAnimationInfo
    {
        public enum animation {non, wait,running,pause,over}
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
        /// 动画进度
        /// </summary>
        public float progress;

    }


    public class UIAnimator:MonoBehaviour
    {
        public enum AnimationType
        {
            /// <summary>
            /// 无动画
            /// </summary>
            None=0,
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
        }

        #region 单例封装
        public static UIAnimator Instance;
        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else{ Destroy(gameObject); }
        }
        #endregion

        #region 属性
        private Dictionary<RectTransform,UIBoxAnimationInfo> infos = new Dictionary<RectTransform, UIBoxAnimationInfo> ();
        #endregion

        #region 功能函数
        /// <summary>
        /// 添加动画列表
        /// </summary>
        /// <param name="transform">UI控件体</param>
        /// <param name="type">动画类型</param>
        /// <param name="callBack">回调函数</param>
        public void AddAnimation(RectTransform transform,AnimationType type,Action callBack=null)
        {
            infos.Add(transform,new UIBoxAnimationInfo 
            { 
                type = type,
                callBack = callBack ,
                transform=transform,
                speed=1,
                autoRemove =true,
                status = UIBoxAnimationInfo.animation.wait,
                progress = 0f,
            });
        }
        /// <summary>
        /// 移除指定控件的动画
        /// </summary>
        /// <param name="transform"></param>
        public void RemoveAnimation(RectTransform transform)
        {
            if(infos.Keys.Contains(transform))
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
        public UIBoxAnimationInfo GetAnimationInfo(RectTransform transform)
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
            info.status = UIBoxAnimationInfo.animation.running;
        }
        /// <summary>
        /// 跳过动画
        /// </summary>
        /// <param name="transform"></param>
        public void SkipAnimation(RectTransform transform)
        {
            var info = infos[transform];
            info.progress = 1f;
            info.status = UIBoxAnimationInfo.animation.running;
        }
        /// <summary>
        /// 暂停动画
        /// </summary>
        /// <param name="transform"></param>
        public void PauseAnimation(RectTransform transform)
        {
            var info = infos[transform];
            info.status = UIBoxAnimationInfo.animation.pause;
        }
        #endregion

        #region 内部函数
        private void BoxAnimation_Left_Open(UIBoxAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, 0);
            info.transform.anchorMax = new Vector2(info.progress,1);
        }
        private void BoxAnimation_Left_Close(UIBoxAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, 0);
            info.transform.anchorMax = new Vector2(1-info.progress, 1);
        }
        private void BoxAnimation_Right_Open(UIBoxAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(1-info.progress, 0);
            info.transform.anchorMax = new Vector2(1, 1);
        }
        private void BoxAnimation_Right_Close(UIBoxAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(info.progress, 0);
            info.transform.anchorMax = new Vector2(1, 1);
        }
        private void BoxAnimation_Top_Open(UIBoxAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0,1- info.progress);
            info.transform.anchorMax = new Vector2(1, 1);
        }
        private void BoxAnimation_Top_Close(UIBoxAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, info.progress);
            info.transform.anchorMax = new Vector2(1, 1);
        }
        private void BoxAnimation_Bottom_Open(UIBoxAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, 0);
            info.transform.anchorMax = new Vector2(1, info.progress);
        }
        private void BoxAnimation_Bottom_Close(UIBoxAnimationInfo info)
        {
            info.transform.anchorMin = new Vector2(0, 0);
            info.transform.anchorMax = new Vector2(1, 1-info.progress);
        }
        #endregion

        #region Unity
        private void Update()
        {
            List<RectTransform> trfs = new List<RectTransform>();
            foreach(UIBoxAnimationInfo info in infos.Values)
            {
                if(info.status == UIBoxAnimationInfo.animation.running)
                {
                    switch(info.type)
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
                    }
                    if (info.progress >= 1) 
                    { 
                        info.status = UIBoxAnimationInfo.animation.over;
                        trfs.Add(info.transform);
                    }
                    info.progress = Mathf.Clamp(info.progress + Time.deltaTime * info.speed, 0f, 1f);
                }
            }

            foreach(RectTransform transform in trfs)
            {
                UIBoxAnimationInfo info = infos[transform];
                if (info.callBack != null) { info.callBack(); }
                if (info.autoRemove) { infos.Remove(info.transform); }
            }
        }
        #endregion
    }
}
