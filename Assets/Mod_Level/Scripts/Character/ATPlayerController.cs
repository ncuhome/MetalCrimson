﻿using ER.Entity2D;
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

            state = owner.GetAttribute<ATPlayerState>();
            if (state == null) Debug.LogError("未找到角色的状态管理器:<ATPlayerState>");

            interacter = owner.GetAttribute<ATInteract>();

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

            InputManager.InputActions.Player.Dash.performed += Dash;
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
        private ATPlayerState state;
        /// <summary>
        /// 交互器
        /// </summary>
        private ATInteract interacter;

        /// <summary>
        /// 辅助线绘制
        /// </summary>
        public LineRenderer line;
        [Tooltip("瞄准线基准点偏移量")]
        public Vector2 start_offset;

        #endregion 属性

        #region 角色控制

        private float LastTime_left = -1;
        private float LastTime_right = -1;

        

        private void MoveLeft(InputAction.CallbackContext ctx)
        {
            if (state.ControlAct && !state.Vertigo)
            {
                if (Time.time-LastTime_left < 0.2f)
                {
                    actionManager.Action("Move", "left","run");
                }
                else
                {
                    actionManager.Action("Move", "left");
                }
                LastTime_left = Time.time;
            }
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
            {
                if (Time.time - LastTime_right < 0.2f)
                {
                    actionManager.Action("Move", "right", "run");
                }
                else
                {
                    actionManager.Action("Move", "right");
                }
                LastTime_right = Time.time;
            }
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
            if (state.ControlAct && !state.Vertigo)
            {
                state.ControlDir = false;
                actionManager.Action("Attack");
            }
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
                Debug.Log("执行交互");
                var obj = interacter.Selected;
                if(obj!=null)
                {
                    obj.EnterInteract();//执行交互
                }
            }
        }
        public void Dash(InputAction.CallbackContext ctx)
        {
            if (state.ControlAct && !state.Vertigo)
                actionManager.Action("Dash");
        }

        private void PostureUp()
        {
            state.ActPosture = ATPlayerState.Posture.Up;
        }

        private void PostureFront()
        {
            state.ActPosture = ATPlayerState.Posture.Front;
        }

        private void PostureDown()
        {
            state.ActPosture = ATPlayerState.Posture.Down;
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
            start -= new Vector3(start_offset.x, start_offset.y, lineHeight);
            end = new Vector3(end.x, end.y, start.z);
            line.SetPosition(0, start);
            line.SetPosition(1, end);
            Vector3 delta = end - start;
            float angle;
            if (state.direction == ATPlayerState.Direction.Right)
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
                    if (state.ActPosture == ATPlayerState.Posture.Front || state.ActPosture == ATPlayerState.Posture.Down)
                    {
                        PostureUp();
                    }
                }
                else//下
                {
                    if (state.ActPosture == ATPlayerState.Posture.Front || state.ActPosture == ATPlayerState.Posture.Up)
                    {
                        PostureDown();
                    }
                }
            }
            else//前
            {
                if (state.ActPosture == ATPlayerState.Posture.Up || state.ActPosture == ATPlayerState.Posture.Down)
                {
                    PostureFront();
                }
            }
        }

        #endregion Unity
    }
}