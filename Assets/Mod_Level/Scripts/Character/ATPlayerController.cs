using ER.Entity2D;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mod_Level
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    public class ATPlayerController : MonoAttribute
    {
        #region 初始化

        public ATPlayerController()
        { AttributeName = nameof(ATPlayerController); }

        public override void Initialize()
        {
            actionManager = owner.GetAttribute<ATActionManager>();

            if (actionManager == null) Debug.LogError("未找到角色的动作管理器:<ATActionManager>");

            state = owner.GetAttribute<ATCharacterState>();
            if (state == null) Debug.LogError("未找到角色的状态管理器:<ATPlayerState>");
            Debug.Log($"is null {InputManager.InputActions == null}");

            /*
            ATAnimator at = null;
            if(owner.TryGetAttribute("ATAnimator",ref at,(IAttribute _at)=>
            {
                animator = ((ATAnimator)_at).Animator;
            }))
            {
                animator = at.Animator;
            }*/

            InputManager.InputActions.Player.MoveLeft.performed += MoveLeft;
            InputManager.InputActions.Player.MoveLeft.canceled += _MoveLeft;
            InputManager.InputActions.Player.MoveRight.performed += MoveRight;
            InputManager.InputActions.Player.MoveRight.canceled += _MoveRight;

            InputManager.InputActions.Player.Attack.performed += Attack;
            InputManager.InputActions.Player.Defense.performed += Defense;
            InputManager.InputActions.Player.Defense.canceled += _Defense;
            InputManager.InputActions.Player.Jump.performed += Jump;

            InputManager.InputActions.Player.Skill1.performed += Skill1;
            InputManager.InputActions.Player.Skill2.performed += Skill2;
            InputManager.InputActions.Player.Interact.performed += Interact;

            region_up.time = -1f;
            region_up.actor = owner;
            region_up.actionName = "PostureUpDefense";
            region_up.actionType = "PassiveDefense";
            region_up.GetComponent<ATActionResponse>().JudgeBreak = Defense;
            region_up.Initialize();

            region_front.time = -1f;
            region_front.actor = owner;
            region_front.actionName = "PostureUpDefense";
            region_front.actionType = "PassiveDefense";
            region_front.GetComponent<ATActionResponse>().JudgeBreak = Defense;
            region_front.Initialize();

            region_down.time = -1f;
            region_down.actor = owner;
            region_down.actionName = "PostureUpDefense";
            region_down.actionType = "PassiveDefense";
            region_down.GetComponent<ATActionResponse>().JudgeBreak = Defense;
            region_down.Initialize();
        }

        #endregion 初始化

        #region 属性

        public float limitAngle = 60f;

        /// <summary>
        /// 辅助线的高度
        /// </summary>
        public float lineHeight = 2;

        /// <summary>
        /// 动作管理器
        /// </summary>
        private ATActionManager actionManager;

        /// <summary>
        /// 玩家状态
        /// </summary>
        private ATCharacterState state;

        /// <summary>
        /// 辅助线绘制
        /// </summary>
        public LineRenderer line;

        public ATActionRegion region_up;

        public ATActionRegion region_front;

        public ATActionRegion region_down;

        #endregion 属性

        #region 角色控制

        private void MoveLeft(InputAction.CallbackContext ctx)
        {
            if (state.ControlAct && !state.Vertigo)
                actionManager.Action("Move", "left");
        }

        private void _MoveLeft(InputAction.CallbackContext ctx)
        {
            if (InputManager.InputActions.Player.MoveRight.phase == InputActionPhase.Performed)
            {
                MoveRight(ctx);
            }
            else
            {
                actionManager.Stop("Move", "left");
            }
        }

        private void MoveRight(InputAction.CallbackContext ctx)
        {
            if (state.ControlAct && !state.Vertigo)
                actionManager.Action("Move", "right");
        }

        private void _MoveRight(InputAction.CallbackContext ctx)
        {
            if (InputManager.InputActions.Player.MoveLeft.phase == InputActionPhase.Performed)
            {
                MoveLeft(ctx);
            }
            else
            {
                actionManager.Stop("Move", "right");
            }
        }

        private void Attack(InputAction.CallbackContext ctx)
        {
            Debug.Log("按下攻击键");
            Debug.Log($"条件:{state.ControlAct} && {!state.Vertigo}");
            if (state.ControlAct && !state.Vertigo)
                actionManager.Action("Attack");
        }

        private void Defense(InputAction.CallbackContext ctx)
        {
            if (state.ControlAct && !state.Vertigo)
                actionManager.Action("Defence");
        }

        private void _Defense(InputAction.CallbackContext ctx)
        {
            actionManager.Stop("Defence");
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            if (state.ControlAct && !state.Vertigo)
                actionManager.Action("Jump");
        }

        private void Skill1(InputAction.CallbackContext ctx)
        { }

        private void Skill2(InputAction.CallbackContext ctx)
        { }

        private void Interact(InputAction.CallbackContext ctx)
        {
            if (state.ControlAct && !state.Vertigo)
            {
                state.interact = ATCharacterState.InteractState.Wait;
                Debug.Log("玩家开启交互");
                Invoke("_Interact", 0.5f);
            }
        }

        public void _Interact()
        {
            if (state.interact != ATCharacterState.InteractState.Interacting)
            {
                state.interact = ATCharacterState.InteractState.None;
                Debug.Log("玩家关闭交互");
            }
        }

        private void PostureUp()
        {
            state.ActPosture = ATCharacterState.Posture.Up;
            region_up.Reset();
            region_down.gameObject.SetActive(false);
            region_front.gameObject.SetActive(false);
        }

        private void PostureFront()
        {
            state.ActPosture = ATCharacterState.Posture.Front;
            region_front.Reset();
            region_down.gameObject.SetActive(false);
            region_up.gameObject.SetActive(false);
        }

        private void PostureDown()
        {
            state.ActPosture = ATCharacterState.Posture.Down;
            region_down.Reset();
            region_up.gameObject.SetActive(false);
            region_front.gameObject.SetActive(false);
        }

        /// <summary>
        /// 防御判定
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool Defense(ActionInfo info)
        {
            return true;
        }

        #endregion 角色控制

        #region Unity

        private void Awake()
        {
        }

        private void Update()
        {
            Vector3 start = owner.transform.position;
            Vector3 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            start -= new Vector3(0, 0, lineHeight);
            end = new Vector3(end.x, end.y, start.z);
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            Vector3 delta = end - start;
            float angle;
            if (state.direction == ATCharacterState.Direction.Right)
            {
                angle = Vector2.Angle(delta, Vector2.right);
            }
            else
            {
                angle = Vector2.Angle(delta, Vector2.left);
            }
            if (angle > limitAngle / 2)
            {
                if (delta.y > 0)//上
                {
                    if (state.ActPosture == ATCharacterState.Posture.Front || state.ActPosture == ATCharacterState.Posture.Down)
                    {
                        PostureUp();
                    }
                }
                else//下
                {
                    if (state.ActPosture == ATCharacterState.Posture.Front || state.ActPosture == ATCharacterState.Posture.Up)
                    {
                        PostureDown();
                    }
                }
            }
            else//前
            {
                if (state.ActPosture == ATCharacterState.Posture.Up || state.ActPosture == ATCharacterState.Posture.Down)
                {
                    PostureFront();
                }
            }
        }

        #endregion Unity
    }
}