using ER.Entity2D;
using System.Buffers;
using UnityEngine;

namespace Mod_Level
{
    public class MDMoveLeft : MDAction
    {
        private ATCharacterState state;
        private ATEnvironmentDetector env;
        private Rigidbody2D body;
        [SerializeField]
        [Tooltip("移动检测区域")]
        private ATButtonRegion region;
        private bool movable = true;
        private bool moving = false;
        public MDMoveLeft() { actionName = "MoveLeft"; }
        public override void Initialize()
        {
            state = manager.Owner.GetAttribute<ATCharacterState>();
            body = manager.Owner.GetComponent<Rigidbody2D>();
            env = manager.Owner.GetAttribute<ATEnvironmentDetector>();
            env.OnAirEvent += () =>
            {
                if (enabled)
                {
                    enabled = false;
                }
            };
            env.OnLandEvent += () =>
            {
                if(moving && !enabled)
                {
                    enabled = true;
                }
            };
            region.notTouchEvent += () => { movable = false; };
            region.notTouchEvent += () => { movable = true; };
        }
        public override void StartAction()
        {
            state.direction = ATCharacterState.Direction.Left;
            manager.Owner.transform.eulerAngles = new Vector3(0,180,0);
            if (env.Type == ATEnvironmentDetector.EnvironmentType.Land)
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
            if (!movable) return;
            if (body.velocity.x > -state.speed)
            {
                body.velocity = new Vector2(-state.speed, body.velocity.y);
            }
        }
    }
}