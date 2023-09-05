using System.Collections.Generic;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 效果管理器
    /// </summary>
    public class ATBuffManager : MonoAttribute
    {
        #region 初始化

        public ATBuffManager()
        { AttributeName = nameof(ATBuffManager); }

        public override void Initialize()
        {
        }

        #endregion 初始化

        #region 属性

        [SerializeField]
        private List<MDBuff> buffs = new List<MDBuff>();

        private Dictionary<string, MDBuff> buffsDic = new Dictionary<string, MDBuff>();

        #endregion 属性

        #region 管理

        /// <summary>
        /// 添加效果
        /// </summary>
        /// <param name="buff"></param>
        public void Add(MDBuff buff)
        {
            if (buffsDic.TryGetValue(buff.buffName, out MDBuff bf))//出现同一个buff重复添加
            {
                switch (bf.repeatType)
                {
                    case MDBuff.RepeatType.None: break;
                    case MDBuff.RepeatType.NoneAndEnter:
                        bf.Enter();
                        break;

                    case MDBuff.RepeatType.MoreLevel:
                        if (bf.level < bf.levelMax)
                        {
                            bf.level += 1;
                        }
                        break;

                    case MDBuff.RepeatType.MoreLevelAndEnter:
                        if (bf.level < bf.levelMax)
                        {
                            bf.level += 1;
                        }
                        bf.Enter();
                        break;

                    case MDBuff.RepeatType.MoreTime:
                        bf.time += buff.time;
                        break;

                    case MDBuff.RepeatType.MoreTimeAndEnter:
                        bf.time += buff.time;
                        bf.Enter();
                        break;

                    case MDBuff.RepeatType.RepeatTime:
                        bf.time = bf.defTime;
                        break;

                    case MDBuff.RepeatType.RepeatTimeAndEnter:
                        bf.time = bf.defTime;
                        bf.Enter();
                        break;

                    case MDBuff.RepeatType.Custom:
                        bf.Repeat();
                        break;
                }
            }
            else
            {
                Debug.Log($"添加新的Buff:{buff.buffName}");
                buff.owner = this;
                buffsDic[buff.buffName] = buff;
                buffs.Add(buff);
                buff.ResetTime();
                buff.Enter();
                ArrangeBuffs();
            }
        }

        /// <summary>
        /// 移除效果
        /// </summary>
        /// <param name="name"></param>
        /// <returns>如果移除失败则返回false</returns>
        public bool Remove(string name)
        {
            bool exist = false;
            MDBuff buff = null;
            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].buffName == name)
                {
                    buff = buffs[i];
                    buffs.RemoveAt(i);
                    exist = true;
                    break;
                }
            }
            if (exist)
            {
                buffsDic.Remove(name);
                buff.Exit();
            }
            return exist;
        }

        /// <summary>
        /// 判断指定效果是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            return buffsDic.ContainsKey(name);
        }

        /// <summary>
        /// 按优先级排列效果
        /// </summary>
        private void ArrangeBuffs()
        {
            for (int i = 0; i < buffs.Count - 1; i++)
            {
                for (int k = i + 1; k < buffs.Count; k++)
                {
                    if (buffs[k].priority > buffs[i].priority)
                    {
                        var temp = buffs[k];
                        buffs[k] = buffs[i];
                        buffs[i] = temp;
                    }
                }
            }
        }

        #endregion 管理

        #region Unity

        private void Update()
        {
            List<MDBuff> removeBuffs = new();
            foreach (var buff in buffs)
            {
                //Debug.Log($"buff:{buff.buffName} : time:{buff.time}");
                if (buff.defTime > 0)
                {
                    buff.time -= Time.deltaTime;
                    if (buff.time <= 0)
                    {
                        buff.Exit();
                        removeBuffs.Add(buff);
                    }
                    else
                    {
                        buff.Stay();
                    }
                }
                else
                {
                    buff.Stay();
                }
            }
            foreach (var buff in removeBuffs)
            {
                Debug.Log($"remove buff:{buff.buffName}");
                buffs.Remove(buff);
                buffsDic.Remove(buff.buffName);
            }
        }

        #endregion Unity
    }
}