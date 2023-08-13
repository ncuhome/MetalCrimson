using UnityEngine;

namespace ER.Entity
{
    /// <summary>
    /// 击退箱
    /// </summary>
    public class ATRepelBox : StaticAttribute
    {
        /// <summary>
        /// 实体刚体
        /// </summary>
        public Rigidbody2D rigidbody;
        /// <summary>
        /// 击退倍率
        /// </summary>
        public float multiplier = 1;
        /// <summary>
        /// 被攻击时是否发生镜头震动
        /// </summary>
        public bool shake = false;
        /// <summary>
        /// 镜头震动力度倍率
        /// </summary>
        public float shakePowerRate = 0.25f;
        /// <summary>
        /// 镜头震动时间
        /// </summary>
        public float shakeTime = 0.25f;

        public ATRepelBox(Entity _owner) : base(_owner)
        {
            owner.GetRepelEvent += GetRepel;
            rigidbody = owner.SelfRigidbody;
        }

        public void GetRepel(RepelEventInfo info)
        {
            rigidbody.velocity = info.repel * multiplier;
        }

        public override void Initialization()
        {
        }
    }
}
