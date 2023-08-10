using ER.Entity2D;
using UnityEngine;

namespace Mod_Battle
{
    public class MDMoveLeft : MDAction
    {
        private ATCharacterState state;
        public MDMoveLeft() { actionName = "MoveLeft"; }
        public override void Initialize()
        {
            state = manager.Owner.GetAttribute<ATCharacterState>();
        }
        public override void StartAction()
        {
            enabled = true;
        }
        public override void StopAction()
        {
            enabled = false;
        }
        private void Update()
        {
            manager.Owner.transform.position += new Vector3(-1, 0, 0) * state.speed * Time.deltaTime;
        }
    }
}