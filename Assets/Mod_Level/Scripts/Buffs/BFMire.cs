using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 沼泽debuff
    /// </summary>
    public class BFMire : MDBuff
    {
        private ATCharacterState state;
        public CorrectValueDelegate jump_lose;
        public CorrectValueDelegate speed_lose;
        [Tooltip("毒药效果")]
        public bool poison = false;
        public float poisonDamage = 10f;

        public BFMire()
        {
            buffName = "Mire";
            repeatType = RepeatType.MoreTime;
            buffTag.Add("debuff");
            buffTag.Add("state");

            jump_lose = new CorrectValueDelegate(JumpCoreect,1,"Mire");
            speed_lose = new CorrectValueDelegate(SpeedCoreect, 1, "Mire");
        }

        private float JumpCoreect(float origin)
        {
            return origin * 0.5f;
        }
        private float SpeedCoreect(float origin)
        {
            return origin * 0.5f;
        }


        public override void EffectOnEnter()
        {
            state = owner.Owner.GetAttribute<ATCharacterState>();
            if(state == null)
            {
                owner.Remove(buffName);
                return;
            }
            Debug.Log("赋予沼泽buff");
            state.AddCorrectDelegate("Jump",jump_lose);
            state.AddCorrectDelegate("Speed",speed_lose);
        }

        public override void EffectOnExit()
        {
            Debug.Log("移除沼泽buff");
            state.RemoveCorrectDelegate("Jump","Mire");
            state.RemoveCorrectDelegate("Speed", "Mire");
        }

        private float timer = 0.5f;
        public float resetTime = 0.5f;
        public override void EffectOnStay()
        {
            //if (!poison) return;
            Debug.Log($"池子计时器:{timer}");
            timer -= Time.deltaTime;
            if(timer <=0)
            {
                timer += resetTime;
                ATHealth health = state.health;
                health.ModifyValue(-poisonDamage,null);
            }
        }
    }
}