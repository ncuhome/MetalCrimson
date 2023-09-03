using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 受伤效果:
    /// 每间隔[healCD]秒, 使宿主受到 10*[buff_level] 的生命值的伤害
    /// </summary>
    public class BFDamage : MDBuff
    {
        private ATHealth health;
        private float timer = 0f;
        private float damageCD = 1f;

        /// <summary>
        /// 治疗间隔
        /// </summary>
        public float DamageCD { get => damageCD; set => damageCD = value; }

        public BFDamage()
        {
            buffName = "Damage";
            repeatType = RepeatType.MoreTime;
            buffTag.Add("debuff");
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
            timer = damageCD;
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
                timer += damageCD;
            }
        }

        private void Heal()
        {
            health.ModifyValue(-10 * level, this);
        }
    }
}