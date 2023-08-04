using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Entity
{
    /// <summary>
    /// 效果信息
    /// </summary>
    public struct BuffsInfo
    {
        public enum status
        { 
            /// <summary>
            /// 挂载时
            /// </summary>
            mount, 
            /// <summary>
            /// 生效时
            /// </summary>
            effect, 
            /// <summary>
            /// 卸载时
            /// </summary>
            unload }
        /// <summary>
        /// 效果名称
        /// </summary>
        public string buffName;
        /// <summary>
        /// 效果计时器
        /// </summary>
        public float timer;
        /// <summary>
        /// 效果等级
        /// </summary>
        public int level;
        /// <summary>
        /// 效果是否有效
        /// </summary>
        public bool active;
        /// <summary>
        /// 效果当前状态
        /// </summary>
        public status state;
    }

    public abstract class Buffers
    {
        /// <summary>
        /// 效果名称
        /// </summary>
        public string buffName;
        /// <summary>
        /// 效果计时器
        /// </summary>
        public float timer;
        /// <summary>
        /// 计时是否生效
        /// </summary>
        public bool timerActive;
        /// <summary>
        /// 效果是否有效
        /// </summary>
        public bool active;
        /// <summary>
        /// 效果等级
        /// </summary>
        public int level;
        /// <summary>
        /// 计时器变化
        /// </summary>
        /// <param name="timer">减少值</param>
        public virtual void Timer(float timer)
        {
            this.timer -= timer;
        }
        /// <summary>
        /// 获得此buff时触发的函数
        /// </summary>
        public abstract void BufferStart();
        /// <summary>
        /// 处于buff时触发的函数
        /// </summary>
        /// <returns>是否成功触发效果</returns>
        public abstract bool BufferContent();
        /// <summary>
        /// 移除此buff时触发的函数
        /// </summary>
        public abstract void BufferEnd();
    }
}
