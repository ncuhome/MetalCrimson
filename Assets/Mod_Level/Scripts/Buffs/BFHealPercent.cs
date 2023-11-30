using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 治疗效果:
    /// 每间隔[healCD]秒, 治疗宿主 1*[buff_level] 百分比的生命值
    /// </summary>
    public class BFHealPercent : MDBuff
    {
        private ATHealth health;
        private float timer = 0f;
        private float healCD = 1f;


        /// <summary>
        /// 治疗间隔
        /// </summary>
        public float HealCD { get => healCD; set => healCD = value; }

        public BFHealPercent()
        {
            buffName = "HealPercent";
            repeatType = RepeatType.MoreTime;
            buffTag.Add("buff");
        }

        public override void EffectOnEnter()
        {
            var state = owner.Owner.GetAttribute<ATCharacterState>();
            if (state == null)
            {
                owner.Remove(buffName);
                return;
            }
            health = state.health;
            if (health == null)
            {
                owner.Remove(buffName);
                return;
            }
            timer = healCD;
        }

        public override void EffectOnExit()
        {

        }

        public override void EffectOnStay()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Heal();
                timer += healCD;
            }
        }

        private void Heal()
        {
            health.ModifyValue(health.Max * 0.01f * level, this);
        }
    }
}