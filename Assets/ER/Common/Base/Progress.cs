using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER
{
    /// <summary>
    /// 进度类, 用于反应一个任务进度, 并提供事件接口
    /// </summary>
    public class Progress
    {
        private int total;//总任务数
        private int current;//当前完成任务数
        private bool done;//是否完成
        /// <summary>
        /// 当进度增加时
        /// </summary>
        public event Action<Progress> OnAddProgress;
        /// <summary>
        /// 当任务全部完成时
        /// </summary>
        public event Action OnDone;
        /// <summary>
        /// 总任务数
        /// </summary>
        public int Total { get { return total; } }
        /// <summary>
        /// 当前完成任务数
        /// </summary>
        public int Current { get { return current; } }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool Done { get { return done; } }
        /// <summary>
        /// 增加进度
        /// </summary>
        public void AddProgress()
        {
            if (Done) return;
            current++;
            OnAddProgress?.Invoke(this);
            if(current>=total)
            {
                done = true;
                OnDone?.Invoke();
            }
        }
        public Progress(int total)
        {
            this.total = total;
            if(total <=0)
            {
                done=true;
            }
        }
    }
}
