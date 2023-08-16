using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class MDJump : MDAction
    {
        private ATCharacterState state;
        private ATEnvironmentDetector detector;
        private Rigidbody2D body;
        public MDJump() { actionName = "Jump"; }
        public override void Initialize()
        {
            state = manager.Owner.GetAttribute<ATCharacterState>();
            detector =manager.Owner.GetAttribute<ATEnvironmentDetector>();
            if(detector == null) { Debug.LogError("未找到角色的环境检测器"); }
            body = manager.Owner.GetComponent<Rigidbody2D>();
        }
        public override bool ActionJudge()
        {
            return true;
        }
        public override void StartAction()
        {
            if(detector.Type == ATEnvironmentDetector.EnvironmentType.Land)
                body.velocity = new Vector2(body.velocity.x, state.jump);
        }
        public override void StopAction()
        {

        }
    }
}