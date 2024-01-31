using ER.Entity2D;

namespace Mod_Level
{
    /// <summary>
    /// 僵直buff
    /// </summary>
    public class BFRigidity : MDBuff
    {
        private ATCharacterState state;

        public BFRigidity()
        {
            buffName = "Rigidity";
            repeatType = RepeatType.MoreTime;
            buffTag.Add("debuff");
        }
        public BFRigidity(BuffSetInfo setInfo):base(setInfo) 
        {
            buffName = "Rigidity";
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
            state.ControlAct = false;
        }

        public override void EffectOnExit()
        {
            if (state == null) return;
            state.ControlAct = true;
        }

        public override void EffectOnStay()
        {
        }
    }
}