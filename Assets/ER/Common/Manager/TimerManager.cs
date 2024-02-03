using ER.Template;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 计时器
    /// </summary>
    public class ERTimer
    {
        /// <summary>
        /// 状态更新模式
        /// </summary>
        public enum UpdateMode
        {
            /// <summary>
            /// 不更新
            /// </summary>
            None,
            /// <summary>
            /// 每帧更新
            /// </summary>
            Update,
            /// <summary>
            /// 每个物理帧更新
            /// </summary>
            FixedUpdate,
        }
        /// <summary>
        /// 销毁模式
        /// </summary>
        public enum DestroyMode
        {
            /// <summary>
            /// 不自动销毁
            /// </summary>
            None,
            /// <summary>
            /// 运行一次后自动销毁
            /// </summary>
            Single,
            /// <summary>
            /// 不销毁且循环运行
            /// </summary>
            Loop
        }
        /// <summary>
        /// 计时器标志
        /// </summary>
        public string tag;
        /// <summary>
        /// 状态更新模式
        /// </summary>
        public UpdateMode updateMode;
        /// <summary>
        /// 销毁模式
        /// </summary>
        public DestroyMode destroyMode;
        /// <summary>
        /// 回调函数
        /// </summary>
        public Action callback;
        /// <summary>
        /// 计时限制
        /// </summary>
        public float limit;
        /// <summary>
        /// 当前计时数
        /// </summary>
        public float timer;
        /// <summary>
        /// 最近一次开始计时的点(非实际时间点,仅用作计时参考)
        /// </summary>
        public float last_time;
        /// <summary>
        /// 获取一个短暂的(一次性)计时器
        /// </summary>
        /// <returns></returns>
        public static ERTimer GetBriefTimer(string _tag,Action _callback, float limit, UpdateMode updateMode = UpdateMode.Update)
        {
            return new ERTimer()
            {
                tag=_tag,
                updateMode = updateMode,
                destroyMode = DestroyMode.Single,
                callback=_callback,
                limit = limit,
                timer = 0,
                last_time = 0
            };
        }
        /// <summary>
        /// 获取一个循环的(重复性)计时器
        /// </summary>
        /// <returns></returns>
        public static ERTimer GetLoopTimer(string _tag, Action _callback, float limit, UpdateMode updateMode = UpdateMode.Update)
        {
            return new ERTimer()
            {
                tag = _tag,
                updateMode = updateMode,
                destroyMode = DestroyMode.Loop,
                callback = _callback,
                limit = limit,
                timer = 0,
                last_time = 0
            };
        }
    }

    /// <summary>
    /// 计时器管理器
    /// </summary>
    public class TimerManager:MonoSingletonAutoCreate<TimerManager>
    {
        private List<ERTimer> timers = new List<ERTimer>();//所有计时器列表
        private List<ERTimer> timer_update = new List<ERTimer>();//每帧更新的计时器
        private List<ERTimer> timer_fixedUpdate = new List<ERTimer>();//每个物理帧更新的计时器

        /// <summary>
        /// 注册一个计时器
        /// </summary>
        /// <param name="timer"></param>
        public void RegisterTimer(ERTimer timer)
        {
            Debug.Log("注册新的计时器"+timer.tag);
            timers.Add(timer);
            timer.last_time = Time.fixedTime;
        }
        /// <summary>
        /// 注销指定标签的所有计时器
        /// </summary>
        /// <param name="tag">计时器标签</param>
        public void UnregisterTimer(string tag)
        {
            for(int i=0;i< timers.Count;i++)
            {
                if (timers[i].tag == tag)
                {
                    timers.RemoveAt(i);
                    i--;
                }
            }
            UpdateList();
        }
        /// <summary>
        /// 更新计时器列表(分类)
        /// </summary>
        private void UpdateList()
        {
            timer_update.Clear();
            timer_fixedUpdate.Clear();
            foreach(ERTimer timer in timers)
            {
                switch(timer.updateMode)
                {
                    case ERTimer.UpdateMode.Update:
                        timer_update.Add(timer);
                        break;
                    case ERTimer.UpdateMode.FixedUpdate:
                        timer_fixedUpdate.Add(timer);
                        break;
                }
            }
        }
        /// <summary>
        /// 设置计时器的更新模式
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="updateMode"></param>
        public void SetTimerUpdateMode(string tag,ERTimer.UpdateMode updateMode)
        {
            foreach (ERTimer timer in timers)
            {
                if(timer.tag == tag)
                {
                    timer.updateMode = updateMode;
                    timer.last_time = Time.fixedTime - timer.timer;
                }
            }
            UpdateList();
        }
        /// <summary>
        /// 设置计时器的销毁模式
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="destroyMode"></param>
        public void SetTimerDestroyMode(string tag, ERTimer.DestroyMode destroyMode)
        {
            foreach (ERTimer timer in timers)
            {
                if (timer.tag == tag)
                {
                    timer.destroyMode = destroyMode;
                }
            }
            UpdateList();
        }
        private void UpdateTimerState(List<ERTimer> ts)
        {
            foreach (ERTimer timer in ts)
            {
                timer.timer = Time.fixedTime - timer.last_time;
                if (timer.timer >= timer.limit)
                {
                    timer.callback?.Invoke();
                    switch (timer.destroyMode)
                    {
                        case ERTimer.DestroyMode.None:
                            break;
                        case ERTimer.DestroyMode.Loop:
                            timer.last_time = Time.fixedTime;
                            timer.timer = 0;
                            break;
                        case ERTimer.DestroyMode.Single:
                            timers.Remove(timer);
                            break;
                    }
                }
            }
            UpdateList();
        }
        private void Update()
        {
            UpdateTimerState(timer_update);
        }
        private void FixedUpdate()
        {
            UpdateTimerState(timer_fixedUpdate);
        }
    }
}