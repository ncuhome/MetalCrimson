using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ER.UI.Animator
{
    /// <summary>
    /// 碟状态
    /// </summary>
    public enum CDStatus
    {
        /// <summary>
        /// 等待被播放(出厂设置), 已停止播放
        /// </summary>
        Wait,
        /// <summary>
        /// 正在播放
        /// </summary>
        Playing,
        /// <summary>
        /// 被中断, 保留播放状态
        /// </summary>
        Break,
    }
    /// <summary>
    /// UI动画播放碟
    /// </summary>
    public class UIAnimationCD
    {
        private RectTransform owner;//所属对象(控制对象)
        private CDStatus status;//播放状态
        private Dictionary<string, object> marks;//额外属性接口
        private Dictionary<string, object> vars;//变量存储接口
        private string tag;//标签, 用于区分不同cd
        private string type;//动画类型
        /// <summary>
        /// 是否自动销毁, 如果是则相当是一次性动画
        /// </summary>
        public bool auto_destroy=true;
        private Action callback;//动画完成后的回调

        /// <summary>
        /// 所属对象(控制对象)
        /// </summary>
        public RectTransform Owner => owner;
        /// <summary>
        /// 播放状态
        /// </summary>
        public CDStatus Status => status;
        /// <summary>
        /// 动画标记存储接口
        /// </summary>
        public Dictionary<string,object> Marks=>marks;
        /// <summary>
        /// 临时变量存储接口
        /// </summary>
        public Dictionary<string, object> Vars => vars;
        /// <summary>
        /// 标签, 用于区分不同cd
        /// </summary>
        public string Tag => tag;
        /// <summary>
        /// 动画类型
        /// </summary>
        public string Type { get=>type; set => type = value; }

        public UIAnimationCD(RectTransform _owner,string _tag,Action callback)
        {
            owner = _owner;
            tag = _tag;
            status = CDStatus.Wait;
            marks = new Dictionary<string, object>();
            vars = new Dictionary<string, object>();
            this.callback = callback;
        }
        /// <summary>
        /// 开始播放
        /// </summary>
        public void Start()
        {
            switch (status)
            {
                case CDStatus.Wait:
                    Reset(); 
                    status = CDStatus.Playing;
                    break;
                case CDStatus.Playing:
                    //继续保持状态
                    break;
                case CDStatus.Break:
                    //恢复播放进度
                    status = CDStatus.Playing;
                    break;
            }
            UIAnimator.Instance.StartPlay(this);
        }
        /// <summary>
        /// 重置碟状态(清除变量存储接口)
        /// </summary>
        public void Reset()
        {
            vars.Clear();
            status = CDStatus.Wait;
        }
        /// <summary>
        /// 仅限于IUIPlayer调用: 完成动画状态
        /// </summary>
        public void Done()
        {
            status = CDStatus.Wait;
            callback?.Invoke();
        }
        /// <summary>
        /// 访问动画标记
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return marks[key];
            }
            set
            {
                marks[key] = value;
            }
        }
       /// <summary>
       /// 访问指定动画变量, 若不存在则设定为默认值
       /// </summary>
       /// <param name="key"></param>
       /// <returns></returns>
        public object GetVar(string key,object def_value)
        {
            if (vars.TryGetValue(key, out var value)) return value;
            SetVar(key, def_value);
            return def_value;
        }
        /// <summary>
        /// 设置指定动画变量的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetVar(string key,object value)
        {
            vars[key]=value;
        }
        /// <summary>
        /// 设置回调函数
        /// </summary>
        /// <param name="callback"></param>
        public void SetCallback(Action callback)
        {
            this.callback = callback;
        }
    }

    public static class UIAnimatorExpand
    {
        /// <summary>
        /// 创建一个UICD
        /// </summary>
        /// <returns></returns>
        public static UIAnimationCD CreateUICD(this RectTransform rect, string tag ,Action callback=null)
        {
            return new UIAnimationCD(rect, tag,callback);
        }
        /// <summary>
        /// 将碟子注册进管理器
        /// </summary>
        /// <param name="cd"></param>
        public static void Register(this UIAnimationCD cd)
        {
            UIAnimator.Instance.Register(cd);
        }
    }
}