using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class MDMove : MDAction
    {
        private ATCharacterState ownerState;
        private Rigidbody2D body;

        [SerializeField]
        [Tooltip("前移动检测区域")]
        private ATButtonRegion region_front;

        [SerializeField]
        [Tooltip("后移动检测区域")]
        private ATButtonRegion region_back;
        private ATEnvironmentDetector detector;

        private bool inAir = false;//是否处于空中状态: 空中不可奔跑
        private bool movable_front = true;//是否可移动
        private bool movable_back = true;//是否可移动
        private ATCharacterState.Direction move_dir;//当前移动方向 

        /// <summary>
        /// 是否是跑步状态
        /// </summary>
        private bool run;

        public bool Run
        {
            get => run;
            set
            {
                if (enabled)
                {
                    run = value;
                }
            }
        }

        public MDMove()
        { actionName = "Move"; controlType = ControlType.Bool; }

        public override void Initialize()
        {
            ownerState = manager.Owner.GetAttribute<ATCharacterState>();
            body = manager.Owner.GetComponent<Rigidbody2D>();
            detector = manager.Owner.GetAttribute<ATEnvironmentDetector>();
            if (detector == null) { Debug.LogError("未找到角色的环境检测器"); }
            detector.OnAirEvent += () => { inAir = true; };
            detector.OnLandEvent+= () => { inAir = false; };

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
            run = false;
            if (keys != null && keys.Length>1)
            {
                if (keys[1] == "run")
                {
                    if(!inAir) run = true;
                }
            }
            ChangeDirection(move_dir);
            enabled = true;
            state = ActionState.Acting;
        }

        private void ChangeDirection(ATCharacterState.Direction dir)
        {
            //如果不可以控制朝向则中断
            if (!ownerState.ControlDir) return;
            //Debug.Log($"移动朝向:{move_dir}");
            switch (dir)
            {
                case ATCharacterState.Direction.Right:
                    ownerState.direction = ATCharacterState.Direction.Right;
                    manager.Owner.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;

                case ATCharacterState.Direction.Left:
                    ownerState.direction = ATCharacterState.Direction.Left;
                    manager.Owner.transform.eulerAngles = new Vector3(0, 180, 0);
                    break;
            }
        }

        protected override void StopAction(params string[] keys)
        {
            enabled = false;
            state = ActionState.Disable;
            run = false;
        }

        /// <summary>
        /// 检测当前移动方向是否可移动
        /// </summary>
        /// <returns></returns>
        private bool isMovable()
        {
            if (ownerState.direction == move_dir)//检测前方是否可移动
            {
                return movable_front;
            }
            //检测后方是否可移动
            return movable_back;
        }

        private void Update()
        {
            if (!isMovable()) return;
            if (ownerState.direction != move_dir)
            {
                ChangeDirection(move_dir);
            }
            switch (move_dir)
            {
                case ATCharacterState.Direction.Right:
                    if (body.velocity.x < ownerState["Speed"])
                    {
                        if (run)
                        {
                            body.position += new Vector2(ownerState["Speed"], 0) * 2 * Time.deltaTime;
                        }
                        else
                        {
                            body.position += new Vector2(ownerState["Speed"], 0) * Time.deltaTime;
                        }
                    }
                    break;

                case ATCharacterState.Direction.Left:
                    if (body.velocity.x > -ownerState["Speed"])
                    {
                        if (run)
                        {
                            body.position += new Vector2(-ownerState["Speed"], 0) * 2 * Time.deltaTime;
                        }
                        else
                        {
                            body.position += new Vector2(-ownerState["Speed"], 0) * Time.deltaTime;
                        }
                    }
                    break;
            }
        }

        protected override void BreakAction(params string[] keys)
        {
            enabled = false;
            run = false;
            state = ActionState.Disable;
        }
    }
}