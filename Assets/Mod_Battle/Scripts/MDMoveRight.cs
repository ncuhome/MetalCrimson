using ER.Entity2D;
using UnityEngine;

namespace Mod_Battle
{
    public class MDMoveRight : MDAction
    {
        private ATCharacterState state;
        private ATEnvironmentDetector env;
        private bool moving = false;
        private Rigidbody2D body;
        public MDMoveRight() { actionName = "MoveRight"; }
        public override void Initialize()
        {
            state = manager.Owner.GetAttribute<ATCharacterState>();
            body = manager.Owner.GetComponent<Rigidbody2D>();
            env =manager.Owner.GetAttribute<ATEnvironmentDetector>();
            env.OnAirEvent += () => 
            { 
                if(enabled)
                {
                    enabled = false;
                }
            };
            env.OnLandEvent += () =>
            {
                if (moving && !enabled)
                {
                    enabled = true;
                }
            };
        }
        public override void StartAction()
        {
            state.direction = ATCharacterState.Direction.Right;
            manager.Owner.transform.eulerAngles = new Vector3(0, 0, 0);
            if(env.Type == ATEnvironmentDetector.EnvironmentType.Land)
            {
                enabled = true;
            }
            moving = true;
        }
        public override void StopAction()
        {
            enabled = false;
            moving = false;
        }
        private void Update()
        {
            if (body.velocity.x<state.speed)
            {
                body.velocity = new Vector2(state.speed,body.velocity.y);
            }
        }
    }
}