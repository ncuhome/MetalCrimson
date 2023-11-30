using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 治疗效果:
    /// 每间隔[healCD]秒, 治疗宿主 10*[buff_level] 的生命值
    /// </summary>
    public class BFHeal : MDBuff
    {
        private ATHealth health;
        private float timer = 0f;
        private float healCD = 1f;

        /// <summary>
        /// 治疗间隔
        /// </summary>
        public float HealCD { get => healCD; set => healCD = value; }

        public BFHeal()
        {
            buffName = "Heal";
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
            health.ModifyValue(10 * level, this);
        }
    }
}