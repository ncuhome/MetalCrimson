using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ER.Entity
{
    /// <summary>
    /// 带有取消判定，触发前提 的行为基类模板
    /// </summary>
    public abstract class BHSenior:BHBase
    {
        public BHSenior(ExciteEntity _owner) : base(_owner) { }

        /// <summary>
        /// 当前行为能够被一下 行为打断
        /// </summary>
        public abstract string[] Breakers { get; }
        /// <summary>
        /// 当前状态是否可触发此条件
        /// </summary>
        /// <returns></returns>
        public abstract bool Prerequisite();
    }
}
