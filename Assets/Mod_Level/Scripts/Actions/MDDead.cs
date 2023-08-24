using ER.Entity2D;

namespace Mod_Level
{
    public class MDDead : MDAction
    {
        public MDDead() { actionName = "Dead";layer = "Normal"; }
        public override void Initialize()
        {
            ATCharacterState state = manager.Owner.GetAttribute<ATCharacterState>();
            state.health.DeadEvent += Dead;
        }

        private void Dead(ValueEventInfo info)
        {
            manager.Action("Dead" );
        }

        public override bool ActionJudge()
        {
            return true;
        }

        protected override void StartAction(params string[] keys)
        {
            
        }

        protected override void StopAction(params string[] keys)
        {
            
        }
    }
}