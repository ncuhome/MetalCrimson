using ER.Entity2D;

namespace Mod_Level
{
    /// <summary>
    /// 使用物品
    /// </summary>
    public class MDUseItem:MDAction
    {
        public MDUseItem() { actionName = "UseItem"; controlType =  ControlType.Trigger; }

        public override void Initialize()
        {
            
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

        protected override void BreakAction(params string[] keys)
        {
            throw new System.NotImplementedException();
        }
    }
}