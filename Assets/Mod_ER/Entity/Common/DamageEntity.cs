#define debug
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst.Intrinsics;
using System.Linq;
using Common;
using System.Net;

namespace ER.Entity
{
    /// <summary>
    /// 击退模式
    /// </summary>
    public enum RepelMode
    {
        /// <summary>
        /// 无击退的
        /// </summary>
        Off,
        /// <summary>
        /// 自动的（根据伤害体位置与被判定实体的位置自动确定方向）
        /// </summary>
        Auto,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom,
        /// <summary>
        /// 锁定X轴的（击退方向限制在X轴）
        /// </summary>
        LimX,
        /// <summary>
        /// 锁定Y轴的（击退方向限制在Y轴）
        /// </summary>
        LimY,
    }

    /// <summary>
    /// 伤害事件信息
    /// </summary>
    public struct DamageEventInfo
    {
        /// <summary>
        /// 伤害值
        /// </summary>
        public float damage;
        /// <summary>
        /// 伤害源对象/受到攻击的对象
        /// </summary>
        public object hiter;
    }

    /// <summary>
    /// 伤害体脚本
    /// </summary>
    public class DamageEntity : MonoBehaviour
    {
        #region 内部类|枚举
        public enum FlashMode
        {
            /// <summary>
            /// 关闭
            /// </summary>
            Off,
            /// <summary>
            /// 白屏
            /// </summary>
            White,
            /// <summary>
            /// 黑屏
            /// </summary>
            Black,
            /// <summary>
            /// 先白屏后黑屏
            /// </summary>
            WhiteAndBlack
        }
        /// <summary>
        /// 对象计时器
        /// </summary>
        private class Timer
        {
            /// <summary>
            /// 所属对象
            /// </summary>
            public Entity owner;
            /// <summary>
            /// 计时器
            /// </summary>
            public float timer;
            /// <summary>
            /// 附加计数器
            /// </summary>
            public int times;
        }
        #endregion

        #region 关联组件
        /// <summary>
        /// 所挂载物体身上的碰撞器
        /// </summary>
        private Collider2D Collider;
        #endregion

        #region 属性
        /// <summary>
        /// 关联此伤害体的对象（伤害源对象）
        /// </summary>
        public object owner;
        private Dictionary<Entity, Timer> timers = new Dictionary<Entity, Timer>();//计时器
        public List<string> damageTag = new List<string>();//伤害标签
        private List<Entity> inDamageItems = new List<Entity>();//处于伤害范围内的实体对象
        /// <summary>
        /// 伤害判定是否有效
        /// </summary>
        public bool effective = true;

        #region 伤害体属性配置
        /// <summary>
        /// 伤害值
        /// </summary>
        public float damage = 0;
        /// <summary>
        /// 对同一对象的伤害次数是否有限
        /// </summary>
        public bool singleLimit = false;
        /// <summary>
        /// 是否自动死亡
        /// </summary>
        public bool autoDead = false;
        /// <summary>
        /// 是否限定造成伤害的总次数
        /// </summary>
        public bool hitsLimit = false;

        /// <summary>
        /// 是否在第一次接触时造成伤害
        /// </summary>
        public bool hitFirstTime = true;

        /// <summary>
        /// 对同一对象的最大允许伤害次数
        /// </summary>
        public int damageTimes = 1;
        /// <summary>
        /// 多段伤害触发间隔
        /// </summary>
        public float cd = 0.05f;
        /// <summary>
        /// 当前伤害判定生效次数
        /// </summary>
        public int hits = 0;
        /// <summary>
        /// 最大伤害生效次数
        /// </summary>
        public int maxHits = 1;
        /// <summary>
        /// 最大存活时间
        /// </summary>
        public float maxLiveTime = 1;
        #endregion

        #region 受击效果配置
        /// <summary>
        /// 是否带有受击效果配置（击退效果等）
        /// </summary>
        public bool HitEffect = false;
        /// <summary>
        /// 击退参考效果
        /// </summary>
        public float repPower = 0;
        /// <summary>
        /// 击退方向
        /// </summary>
        public RepelMode repMode = RepelMode.Auto;
        /// <summary>
        /// 自定义击退方向
        /// </summary>
        public Vector2 customDir;
        #endregion

        #endregion

        #region 配置函数
        /// <summary>
        /// 配置成一个静态的伤害体
        /// </summary>
        public void ConfigToStatic()
        {
            singleLimit = false;
            hitsLimit = false;
            autoDead = false;
        }
        /// <summary>
        /// 配置成一个一次性的伤害体（仅能对一个对象造成一次伤害）
        /// </summary>
        /// <param name="_damage">伤害值</param>
        /// <param name="_owner">伤害源</param>
        /// <param name="_time">持续时间</param>
        public void ConfigToCisposable(float _damage, float _time, object _owner)
        {
            damage = _damage;
            owner = _owner;
            autoDead = true;
            hitsLimit = true;
            damageTimes = 1;
            maxLiveTime = _time;
        }
        /// <summary>
        /// 配置成一个可造成多次伤害的伤害体（没有自动销毁）
        /// </summary>
        /// <param name="_damage">单次伤害</param>
        /// <param name="_cd">多段伤害的判定cd</param>
        /// <param name="_owner">伤害源</param>
        public void ConfigToMultiplicity(float _damage, float _cd, object _owner)
        {
            ConfigToStatic();
            damage = _damage;
            cd = _cd;
            owner = _owner;

        }

        /// <summary>
        /// 设置自动死亡
        /// </summary>
        /// <param name="_maxLiveTime">最大存活时间</param>
        public void ConfigAutoDead(float _maxLiveTime)
        {
            autoDead = true;
            maxLiveTime= _maxLiveTime;
        }
        /// <summary>
        /// 设置达到最大能造成的伤害次数上限后死亡
        /// </summary>
        /// <param name="_maxHits">最大次数上限</param>
        public void ConfigHitLimit(int _maxHits)
        {
            hitsLimit = true;
            maxHits = _maxHits;
        }
        /// <summary>
        /// 限制单体最大伤害次数
        /// </summary>
        /// <param name="_maxHits">最大次数</param>
        public void ConfigSingleLimit(int _maxHits)
        {
            singleLimit = true;
            damageTimes = _maxHits;
        }
        /// <summary>
        /// 自定义击退效果
        /// </summary>
        /// <param name="_repPower">击退力量</param>
        /// <param name="_customDir">自定义方向</param>
        public void ConfigRepelEffect(float _repPower, Vector2 _customDir)
        {
            HitEffect = true;
            repMode = RepelMode.Custom;
            repPower = _repPower;
            customDir = _customDir;
        }
        #endregion

        #region 公开函数
        /// <summary>
        /// 获取此伤害体对指定实体的模拟击退向量
        /// </summary>
        /// <param name="aim">目标实体</param>
        /// <returns></returns>
        public Vector2 GetDamageDirction(Entity aim)
        {
            switch (repMode)
            {
                case RepelMode.Off:
                    return Vector2.zero;
                case RepelMode.Auto:
                    return (aim.transform.position - transform.position).normalized * repPower;
                case RepelMode.Custom:
                    return customDir.normalized * repPower;
                case RepelMode.LimX:
                    if (aim.transform.position.x < transform.position.x) { return Vector2.left * repPower; }
                    return Vector2.right * repPower;
                case RepelMode.LimY:
                    if (aim.transform.position.y > transform.position.y) { return Vector2.up * repPower; }
                    return Vector2.down * repPower;
            }
            return Vector2.zero;
        }
        /// <summary>
        /// 添加伤害标签
        /// </summary>
        /// <param name="tag">伤害标签</param>
        public void addDamageTag(string tag)
        {
            damageTag.Add(tag);
        }
        /// <summary>
        /// 清除伤害标签
        /// </summary>
        public void clearDamageTag()
        {
            damageTag.Clear();
        }
        /// <summary>
        /// 获取此伤害体的标签组
        /// </summary>
        /// <returns></returns>
        public List<string> DamageTag()
        {
            return damageTag;
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 对指定目标发送伤害事件
        /// </summary>
        /// <param name="aim"></param>
        private void Hit(Entity aim)
        {
            aim.GetDamage(new DamageEventInfo
            {
                damage = damage,
                hiter = owner
            });
            Entity ow = owner as Entity;
            if (ow != null)
            {
                ow.CauseDamage(new DamageEventInfo
                {
                    damage = damage,
                    hiter = aim
                });
            }
            if (HitEffect)
            {
                Vector2 rp = GetDamageDirction(aim);
                aim.GetRepel(new RepelEventInfo
                {
                    repel = rp,
                    sender = owner
                });
                if (ow != null)
                {
                    ow.CauseRepel(new RepelEventInfo
                    {
                        repel = rp,
                        sender = aim
                    });
                }
            }
        }
        /// <summary>
        /// 对目标对象做伤害判定
        /// </summary>
        /// <param name="aim">目标对象</param>
        private void DamageJudge(Entity aim)
        {
            if (damageTag.Contains(aim.DamageTag))
            {
                if (isInside(aim))//判断是否已拥有计时器
                {
                    if (Damagable(aim))//判断伤害cd是否冷却
                    {
                        Debug.Log("造成伤害");

                        Hit(aim);
                        RestartCD(aim);//重置计时器
                    }
                }
                else
                {
                    if (!hitFirstTime)
                    {
                        Hit(aim);
                    }
                    AddCdLabel(aim);//添加计时器
                }
                timers[aim].times++;
                hits++;
                if (hitsLimit)//对总伤害次数有限制
                {
                    if (hits >= maxHits)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        /// <summary>
        /// 给指定对象 创建计时器
        /// </summary>
        /// <param name="aim">需要创建计时器的对象</param>
        private void AddCdLabel(Entity aim)
        {
            timers.Add(aim, new Timer()
            {
                owner = aim,
                timer = cd
            });
        }
        /// <summary>
        /// 判断指定判定对象 是否在计时器列表内
        /// </summary>
        /// <param name="aim">需要判定的对象</param>
        /// <returns></returns>
        private bool isInside(Entity aim)
        {
            return timers.Keys.Contains(aim);
        }
        /// <summary>
        /// 判断指定判定对象 是否可进行伤害判定（主要判定cd是否冷却，是否达到最大伤害次数）
        /// </summary>
        /// <param name="aim">需要判定的对象</param>
        /// <returns></returns>
        private bool Damagable(Entity aim)
        {
            //Debug.Log($"CDTime:{timers[aim].timer}");

            if (timers[aim].timer <= 0 && (!singleLimit || timers[aim].times < damageTimes)) { return true; }
            return false;
        }
        /// <summary>
        /// 使指定判定对象 的计时器重置
        /// </summary>
        /// <param name="aim"></param>
        private void RestartCD(Entity aim)
        {
            timers[aim].timer = cd;
        }
        #endregion

        #region 
        //进入触发器
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!effective) return;
            Entity entity = collision.GetComponent<Entity>();
            if (entity != null)//如果是实体对象，则加入判定列表
            {
                if (damageTag.Contains(entity.DamageTag))
                {
                    inDamageItems.Add(entity);
                    DamageJudge(entity);
                }
            }
#if debug
            Debug.Log("进入");
#endif
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!effective) return;
            Entity entity = collision.GetComponent<Entity>();
            if (entity != null)//如果是实体对象，则移除判定列表
            {
                inDamageItems.Remove(entity);
            }
#if debug
            Debug.Log("离开");
#endif
        }
        private void OnTriggerStay2D(Collider2D collision)//弃用此方案，因为长时间在范围内不移动则不会触发此事件
        {
            Debug.Log("121231321");
        }

        #endregion

        #region Unity
        private void Start()
        {
            Collider = GetComponent<Collider2D>();
            damageTag.Add("Test");
        }
        private void Update()
        {
            if (!effective) return;
            foreach (Timer timer in timers.Values)
            {
                if (timer.timer > 0) { timer.timer -= Time.deltaTime; }
            }

            foreach (Entity entity in inDamageItems)
            {
                DamageJudge(entity);
            }

            if (autoDead)
            {
                if (maxLiveTime > 0) { maxLiveTime -= Time.deltaTime; }
                if (maxLiveTime <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        #endregion
    }
}
