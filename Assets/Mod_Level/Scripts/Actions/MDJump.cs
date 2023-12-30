using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class MDJump : MDAction
    {
        private ATCharacterState ownerState;
        private ATEnvironmentDetector detector;
        private Rigidbody2D body;

        public MDJump()
        { actionName = "Jump"; controlType = ControlType.Trigger; }

        public override void Initialize()
        {
            ownerState = manager.Owner.GetAttribute<ATCharacterState>();
            detector = manager.Owner.GetAttribute<ATEnvironmentDetector>();
            if (detector == null) { Debug.LogError("未找到角色的环境检测器"); }
            body = manager.Owner.GetComponent<Rigidbody2D>();
        }

        public override bool ActionJudge()
        {
            Debug.Log($"是否可跳跃:{detector.Type == ATEnvironmentDetector.EnvironmentType.Land}");
            return true;
        }

        protected override void StartAction(params string[] keys)
        {
            if (detector.Type == ATEnvironmentDetector.EnvironmentType.Land)
                body.velocity = new Vector2(body.velocity.x, ownerState["Jump"]);
        }

        protected override void StopAction(params string[] keys)
        {
        }

        protected override void BreakAction(params string[] keys)
        {
        }
    }
}