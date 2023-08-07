using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace ER.Entity
{
    /// <summary>
    /// buff事件
    /// </summary>
    /// <param name="info"></param>
    public delegate void DelBuff(BuffsInfo info);

    /// <summary>
    /// Buff槽属性
    /// </summary>
    public class ATBufferTrough : DynamicAttribute
    {
        /// <summary>
        /// 效果列表
        /// </summary>
        public Dictionary<string, Buffers> buffs = new Dictionary<string, Buffers>();

        #region 事件
        /// <summary>
        /// 当有新的buff挂载时触发的事件
        /// </summary>
        public event DelBuff BuffMountEvent;
        /// <summary>
        /// 当有buff卸载时触发的事件
        /// </summary>
        public event DelBuff BuffUnloadEvent;
        /// <summary>
        /// buff持续生效时触发的事件
        /// </summary>
        public event DelBuff BuffRunningEvent;
        #endregion

        #region 公开函数
        public void AddBuff(Buffers buff)
        {
            buffs.Add(buff.buffName, buff);
            buff.BufferStart();
            BuffMountEvent(new BuffsInfo
            {
                buffName = buff.buffName,
                active = buff.active,
                level = buff.level,
                state = BuffsInfo.status.mount,
                timer = buff.timer,
            });
        }
        public void RemoveBuff(Buffers buff)
        {
            if (buffs.Values.Contains(buff))
            {
                buff.BufferEnd();
                BuffUnloadEvent(new BuffsInfo
                {
                    buffName = buff.buffName,
                    active = buff.active,
                    level = buff.level,
                    state = BuffsInfo.status.unload,
                    timer = buff.timer,
                });
            }
        }
        #endregion
        public override void Initialization()
        {

        }
        private void Update()
        {
            foreach (var item in buffs.Values)
            {
                if (item.timerActive)
                {
                    item.Timer(Time.deltaTime);
                    if (item.timer < 0) { RemoveBuff(item); }
                }
                if (item.BufferContent())
                {
                    BuffRunningEvent(new BuffsInfo
                    {
                        buffName = item.buffName,
                        timer = item.timer,
                        active = item.active,
                        level = item.level,
                        state = BuffsInfo.status.effect,
                    });
                }
            }
        }
    }
}
