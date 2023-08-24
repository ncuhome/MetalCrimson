// Ignore Spelling: Armor

using ER.Entity2D;

namespace Mod_Level
{
    /// <summary>
    /// 霸体buff
    /// </summary>
    public class BFSuperArmor : MDBuff
    {
        private ATCharacterState state;

        public BFSuperArmor()
        {
            buffName = "SuperArmor(";
            repeatType = RepeatType.MoreTime;
            buffTag.Add("buff");
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
            state.SuperArmor = true;
        }

        public override void EffectOnExit()
        {
            if (state == null) return;
            state.SuperArmor = false;
        }

        public override void EffectOnStay()
        {
        }
    }
}