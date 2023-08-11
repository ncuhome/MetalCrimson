using ER.Entity2D;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mod_Battle
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

        public void MoveLeft(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
            {
                actionManager.Action("MoveLeft");
            }
            else
            {
                actionManager.Stop("MoveLeft");
            }
        }

        public void MoveRight(InputAction.CallbackContext ctx)
        {
            if(ctx.phase == InputActionPhase.Performed)
            {
                actionManager.Action("MoveRight");
            }
            else
            {
                actionManager.Stop("MoveRight");
            }
        }

        public void Attack(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
            {
                actionManager.Action("Attack");
            }
        }

        public void Defense(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
            {
                actionManager.Action("Defense");
            }
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
            {
                actionManager.Action("Jump");
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
            if(state.direction == ATCharacterState.Direction.Right)
            {
                angle=Vector2.Angle(delta, Vector2.right);
            }
            else
            {
                angle=Vector2.Angle(delta, Vector2.left);
            }
            if (angle > limitAngle / 2)
            {
                if (delta.y > 0)//上
                {
                    actionManager.Action("PostureUp");
                }
                else//下
                {
                    actionManager.Action("PostureDown");
                }
            }
            else//前
            {
                actionManager.Action("PostureFront");
            }
        }

        #endregion Unity
    }
}