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
            Debug.Log($"is null {InputManager.inputActions == null}");

            InputManager.inputActions.Player.MoveLeft.performed += MoveLeft;
            InputManager.inputActions.Player.MoveLeft.canceled += _MoveLeft;
            InputManager.inputActions.Player.MoveRight.performed += MoveRight;
            InputManager.inputActions.Player.MoveRight.canceled += _MoveRight;

            InputManager.inputActions.Player.Attack.performed += Attack;
            InputManager.inputActions.Player.Defense.performed += Defense;
            InputManager.inputActions.Player.Defense.canceled += _Defense;
            InputManager.inputActions.Player.Jump.performed += Jump;

            InputManager.inputActions.Player.Skill1.performed += Skill1;
            InputManager.inputActions.Player.Skill2.performed += Skill2;
            InputManager.inputActions.Player.Interact.performed += Interact;
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

        #endregion 属性

        #region 角色控制

        private void MoveLeft(InputAction.CallbackContext ctx)
        {
            if (state.ControlAct && !state.Vertigo)
                actionManager.Action("Move", "left");
        }

        private void _MoveLeft(InputAction.CallbackContext ctx)
        {
            if (InputManager.inputActions.Player.MoveRight.phase == InputActionPhase.Performed)
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
            if (InputManager.inputActions.Player.MoveLeft.phase == InputActionPhase.Performed)
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
            float angle = 0;
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
                    if (state.posture == ATCharacterState.Posture.Front)
                    {
                        actionManager.Stop("PostureFront");
                        actionManager.Action("PostureUp");
                    }
                    else if (state.posture == ATCharacterState.Posture.Down)
                    {
                        actionManager.Stop("PostureDown");
                        actionManager.Action("PostureUp");
                    }
                }
                else//下
                {
                    if (state.posture == ATCharacterState.Posture.Front)
                    {
                        actionManager.Stop("PostureFront");
                        actionManager.Action("PostureDown");
                    }
                    else if (state.posture == ATCharacterState.Posture.Up)
                    {
                        actionManager.Stop("PostureUp");
                        actionManager.Action("PostureDown");
                    }
                }
            }
            else//前
            {
                if (state.posture == ATCharacterState.Posture.Up)
                {
                    actionManager.Stop("PostureUp");
                    actionManager.Action("PostureFront");
                }
                else if (state.posture == ATCharacterState.Posture.Down)
                {
                    actionManager.Stop("PostureDown");
                    actionManager.Action("PostureFront");
                }
            }
        }

        #endregion Unity
    }
}