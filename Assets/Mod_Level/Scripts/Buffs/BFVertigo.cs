using ER.Entity2D;

namespace Mod_Level
{
    /// <summary>
    /// 眩晕buff
    /// </summary>
    public class BFVertigo : MDBuff
    {
        private ATCharacterState state;

        public BFVertigo()
        {
            buffName = "Vertigo";
            repeatType = RepeatType.MoreTime;
            buffTag.Add("debuff");
        }
        public BFVertigo(BuffSetInfo setInfo):base(setInfo)
        {
            buffName = "Vertigo";
            repeatType = RepeatType.MoreTime;
            buffTag.Add("debuff");
        }
        public override void EffectOnEnter()
        {
            state = owner.Owner.GetAttribute<ATCharacterState>();
            //如果目标没有状态属性, 则直接移除该效果
            if (state == null)
            {
                owner.Remove(buffName);
                return;
            }
            state.Vertigo = true;
        }

        public override void EffectOnExit()
        {
            if (state == null) return;
            state.Vertigo = false;
        }

        public override void EffectOnStay()
        {
        }
    }
}