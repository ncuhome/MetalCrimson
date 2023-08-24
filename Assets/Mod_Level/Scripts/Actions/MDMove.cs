using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class MDMove : MDAction
    {
        private ATCharacterState state;
        private Rigidbody2D body;

        [SerializeField]
        [Tooltip("前移动检测区域")]
        private ATButtonRegion region_front;

        [SerializeField]
        [Tooltip("后移动检测区域")]
        private ATButtonRegion region_back;

        private bool movable_front = true;//是否可移动
        private bool movable_back = true;//是否可移动
        private ATCharacterState.Direction move_dir;//当前移动方向

        public MDMove()
        { actionName = "Move"; layer = "Move"; }

        public override void Initialize()
        {
            state = manager.Owner.GetAttribute<ATCharacterState>();
            body = manager.Owner.GetComponent<Rigidbody2D>();

            region_front.touchEvent += () => { movable_front = false; };
            region_front.notTouchEvent += () => { movable_front = true; };

            region_back.touchEvent += () => { movable_back = false; };
            region_back.notTouchEvent += () => { movable_back = true; };
        }

        public override bool ActionJudge()
        {
            return true;
        }

        protected override void StartAction(params string[] keys)
        {
            if (keys == null || keys[0] == null || keys[0] == string.Empty || keys[0] == "right")
            {
                move_dir = ATCharacterState.Direction.Right;
            }
            else
            {
                move_dir = ATCharacterState.Direction.Left;
            }
            ChangeDirection(move_dir);
            enabled = true;
        }

        private void ChangeDirection(ATCharacterState.Direction dir)
        {
            //如果不可以控制朝向则中断
            if (!state.ControlDir) return;
            Debug.Log($"移动朝向:{move_dir}");
            switch (dir)
            {
                case ATCharacterState.Direction.Right:
                    state.direction = ATCharacterState.Direction.Right;
                    manager.Owner.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;

                case ATCharacterState.Direction.Left:
                    state.direction = ATCharacterState.Direction.Left;
                    manager.Owner.transform.eulerAngles = new Vector3(0, 180, 0);
                    break;
            }
        }

        protected override void StopAction(params string[] keys)
        {
            enabled = false;
        }

        /// <summary>
        /// 检测当前移动方向是否可移动
        /// </summary>
        /// <returns></returns>
        private bool isMovable()
        {
            if (state.direction == move_dir)//检测前方是否可移动
            {
                return movable_front;
            }
            //检测后方是否可移动
            return movable_back;
        }

        private void Update()
        {
            if (!isMovable()) return;
            switch (move_dir)
            {
                case ATCharacterState.Direction.Right:
                    if (body.velocity.x < state.speed)
                    {
                        body.velocity = new Vector2(state.speed, body.velocity.y);
                    }
                    break;

                case ATCharacterState.Direction.Left:
                    if (body.velocity.x > -state.speed)
                    {
                        body.velocity = new Vector2(-state.speed, body.velocity.y);
                    }
                    break;
            }
        }
    }
}