using ER.Entity2D;
using UnityEngine;

namespace Mod_Battle
{
    public class MDJump : MDAction
    {
        private ATCharacterState state;
        private ATEnvironmentDetector detector;
        public MDJump() { actionName = "Jump"; }
        public override void Initialize()
        {
            state = manager.Owner.GetAttribute<ATCharacterState>();
            detector =manager.Owner.GetAttribute<ATEnvironmentDetector>();
            if(detector == null) { Debug.LogError("未找到角色的环境检测器"); }
        }
        public override void StartAction()
        {
            if(detector.Type == ATEnvironmentDetector.EnvironmentType.Land)
                manager.Owner.GetComponent<Rigidbody2D>().velocity = Vector2.up * state.jump;
        }
        public override void StopAction()
        {

        }
    }
}