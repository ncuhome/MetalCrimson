using ER.Entity2D;

namespace Mod_Level
{
    public class BFMire : MDBuff
    {
        public BFMire()
        {
            buffName = "Mire";
            repeatType = RepeatType.MoreTime;
            buffTag.Add("debuff");
            buffTag.Add("state");
        }

        public override void EffectOnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void EffectOnExit()
        {
            throw new System.NotImplementedException();
        }

        public override void EffectOnStay()
        {
            throw new System.NotImplementedException();
        }
    }
}