// Ignore Spelling: mana Armor

using ER.Entity2D;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Mod_Level
{
    public class ATCharacterState : MonoAttribute
    {
        #region 组件

        private Animator animator;
        private ATActionManager actionManager;

        #endregion 组件

        #region 初始化

        public ATCharacterState()
        { AttributeName = nameof(ATCharacterState); }

        public override void Initialize()
        {
            health.Owner = owner;
            health.Initialize();
            stamina.Owner = owner;
            stamina.Initialize();
            mana.Owner = owner;
            mana.Initialize();

            ATAnimator ator = owner.GetAttribute<ATAnimator>();
            if (animator != null)
            {
                animator = ator.Animator;
            }
            else
            {
                Debug.Log("一般获取属性动画机失败");
                owner.CreateDelegation("ATAnimator", (IAttribute at) =>
                {
                    Debug.Log("成功获取动画机");
                    animator = (at as ATAnimator).Animator;
                });
            }

            actionManager = owner.GetAttribute<ATActionManager>();
            if (actionManager == null)
            {
                owner.CreateDelegation("ATActionManager", (IAttribute at) =>
                {
                    actionManager = at as ATActionManager;
                });
            }

            health.DeadEvent += (ValueEventInfo info) =>
            {
                Dead = true;
            };
        }

        #endregion 初始化

        #region 相关

        /// <summary>
        /// 架势类型
        /// </summary>
        public enum Posture
        { Up, Front, Down }

        /// <summary>
        /// 方向枚举
        /// </summary>
        public enum Direction
        { Left, Right }

        /// <summary>
        /// 交互状态
        /// </summary>
        public enum InteractState
        {
            /// <summary>
            /// 无交互
            /// </summary>
            None,

            /// <summary>
            /// 请求响应
            /// </summary>
            Wait,

            /// <summary>
            /// 正在交互
            /// </summary>
            Interacting,

            /// <summary>
            /// 交互结束
            /// </summary>
            Cancel,
        }

        #endregion 相关

        #region 属性

        [Header("属性 - 基础")]
        [Tooltip("生命值")]
        public float defHealth;

        [Tooltip("精力值")]
        public float defStamina;

        [Tooltip("魔力值")]
        public float defMana;

        [Tooltip("移动速度")]
        public float defSpeed;

        [Tooltip("冷却系数")]
        public float defCDMultiply;

        [Tooltip("防御")]
        public float defDefence;

        [Tooltip("防御系数")]
        public float defDefenceMultiply;

        [Tooltip("重量")]
        public float defWeight;

        [Tooltip("韧性抗性")]
        public float defTenacity;

        [Tooltip("护甲")]
        public float defArmor;

        [Tooltip("护甲等级")]
        public float defArmorLevel;

        #endregion 属性

        #region 属性

        [Header("当前属性")]
        [Tooltip("角色架势 - 不要修改")]
        [SerializeField]
        private Posture posture = Posture.Front;

        /// <summary>
        /// 面朝方向
        /// </summary>
        public Direction direction;

        /// <summary>
        /// 角色生命值（体力）
        /// </summary>
        public ATHealth health;

        /// <summary>
        /// 角色耐性值（精力）
        /// </summary>
        public ATValue stamina;

        /// <summary>
        /// 角色魔力值
        /// </summary>
        public ATValue mana;

        /// <summary>
        /// 移动速度
        /// </summary>
        public float speed;

        /// <summary>
        /// 跳跃力度
        /// </summary>
        public float jump;

        /// <summary>
        /// 角色重量
        /// </summary>
        public float weight;

        /// <summary>
        /// 力量（用于做伤害参考值）
        /// </summary>
        public float power;

        /// <summary>
        /// 技能CD减免
        /// </summary>
        public float CDReduction;

        /// <summary>
        /// 防御值
        /// </summary>
        public float defence;

        /// <summary>
        /// 交互状态
        /// </summary>
        public InteractState interact = InteractState.None;

        /// <summary>
        /// 是否可控制动作
        /// </summary>
        private bool control_act = true;

        /// <summary>
        /// 是否可控制朝向
        /// </summary>
        private bool control_dir = true;

        /// <summary>
        /// 是否死亡
        /// </summary>
        private bool dead = false;

        /// <summary>
        /// 是否霸体
        /// </summary>
        private bool superArmor = false;

        /// <summary>
        /// 是否处于战斗状态
        /// </summary>
        public bool fighting = false;

        /// <summary>
        /// 是否眩晕
        /// </summary>
        private bool vertigo = false;

        /// <summary>
        /// 是否霸体
        /// </summary>
        public bool SuperArmor
        {
            get => superArmor;
            set
            {
                superArmor = value;
                animator.SetBool("superArmor", value);
            }
        }

        /// <summary>
        /// 是否眩晕
        /// </summary>
        public bool Vertigo
        {
            get => vertigo;
            set
            {
                vertigo = value;
                animator.SetBool("vertigo", value);
            }
        }

        public float postureSpeed = 5f;//架势切换速度

        /// <summary>
        /// 是否可控制动作
        /// </summary>
        public bool ControlAct
        {
            get => control_act;
            set
            {
                control_act = value;
                animator.SetBool("control", value);
            }
        }

        /// <summary>
        /// 是否可控制朝向
        /// </summary>
        public bool ControlDir
        {
            get => control_dir;
            set => control_dir = value;
        }

        public bool Dead
        {
            get => dead;
            set
            {
                dead = value;
                animator.SetBool("dead", value);
            }
        }

        public Posture ActPosture
        {
            get => posture;
            set
            {
                posture = value;
                if(stopTag != null)
                    StopCoroutine(stopTag);
                switch (value)
                {
                    case Posture.Up:
                        stopTag = StartCoroutine(PostureChange(animator.GetFloat("posture"), 3));
                        break;
                    case Posture.Front:
                        stopTag = StartCoroutine(PostureChange(animator.GetFloat("posture"), 2));
                        break;
                    case Posture.Down:
                        stopTag = StartCoroutine(PostureChange(animator.GetFloat("posture"), 1));
                        break;

                    default:
                        break;
                }
            }
        }

        private Coroutine stopTag;//协程标记(用于关闭协程)
        private IEnumerator PostureChange(float start,float end)
        {
            float timer = 0;
            while(true)
            {
                timer += Time.deltaTime* postureSpeed;
                animator.SetFloat("posture", Mathf.Lerp(start, end, timer));
                yield return 0;

                if (timer >= 1) yield break;
            }
        }

        /// <summary>
        /// 实体死亡, 销毁对象
        /// </summary>
        public void EntityDestroy()
        {
            Destroy(owner.gameObject);
        }

        #endregion 属性

    }
}