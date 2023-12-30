using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class MDDash : MDAction
    {
        [SerializeField]
        [Tooltip("前移动检测区域")]
        private ATButtonRegion region_front;

        [SerializeField]
        [Tooltip("后移动检测区域")]
        private ATButtonRegion region_back;

        private ATEnvironmentDetector detector;
        private ATCharacterState ownerState;
        private Rigidbody2D body;

        private bool inAir = false;//是否处于空中状态: 空中不可奔跑
        private bool movable_front = true;//是否可移动
        private bool movable_back = true;//是否可移动
        private float dash_distance = 6f;//冲刺距离设定(测试)
        private float dash_distance_back = 6f;//冲刺距离设定(测试)
        private ATCharacterState.Direction move_dir;//当前移动方向

        public MDDash()
        { actionName = "Dash"; controlType = ControlType.Trigger; }

        public override void Initialize()
        {
            ownerState = manager.Owner.GetAttribute<ATCharacterState>();
            body = manager.Owner.GetComponent<Rigidbody2D>();
            detector = manager.Owner.GetAttribute<ATEnvironmentDetector>();
            if (detector == null) { Debug.LogError("未找到角色的环境检测器"); }
            detector.OnAirEvent += () => { inAir = true; };
            detector.OnLandEvent += () => { inAir = false; };

            region_front.touchEvent += () => { movable_front = false; };
            region_front.notTouchEvent += () => { movable_front = true; };
        }

        public override bool ActionJudge()
        {
            return true;
        }

        protected override void BreakAction(params string[] keys)
        {
            enabled = false;
        }

        protected override void StartAction(params string[] keys)
        {
            prograss_distance = 0f;
            enabled = true;
            move_dir = ownerState.direction;
            if (manager.GetActionState("Defence") == ActionState.Acting)
            {
                switch (move_dir)
                {
                    case ATCharacterState.Direction.Left:
                        move_dir = ATCharacterState.Direction.Right;
                        break;

                    case ATCharacterState.Direction.Right:
                        move_dir = ATCharacterState.Direction.Left;
                        break;
                }
            }
        }

        protected override void StopAction(params string[] keys)
        {
            enabled = false;
        }

        private float prograss_distance = 0f;

        private void Update()
        {
            if (!movable_front) return;
            Vector2 dash = new Vector2(ownerState["Speed"], 0) * 5 * Time.deltaTime;
            body.velocity = Vector2.zero;
            switch (move_dir)
            {
                case ATCharacterState.Direction.Left:
                    body.position -= dash;
                    break;

                case ATCharacterState.Direction.Right:
                    body.position += dash;
                    break;
            }
            prograss_distance += dash.magnitude;
            if (prograss_distance >= dash_distance)
                enabled = false;
        }
    }
}