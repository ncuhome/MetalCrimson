// Ignore Spelling: mana Armor

using ER.Entity2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level
{
    public class ATCharacterState : MonoAttribute
    {
        #region 组件

        protected Animator animator;
        private ATActionManager actionManager;

        #endregion 组件

        #region 初始化

        public ATCharacterState()
        {
            AttributeName = nameof(ATCharacterState);
            CreateAttribute("Health");//生命值上限
            CreateAttribute("Stamina");//精力值上限
            CreateAttribute("Mana");//魔法值上限
            CreateAttribute("Speed");//速度
            CreateAttribute("Defence");//防御值
            CreateAttribute("DefenceMultiply");//防御系数
            CreateAttribute("Weight");//重量
            CreateAttribute("Tenacity");//耐性抗性
            CreateAttribute("Armor");//护甲
            CreateAttribute("ArmorLevel");//护甲等级
            CreateAttribute("Jump");//跳跃力
            CreateAttribute("CDReduction");//冷却系数
        }

        public override void Initialize()
        {
            health.Owner = owner;
            health.Initialize();
            this["Health"] = defaultHealth;
            health.SetMax(this["Health", true], null);
            stamina.Owner = owner;
            stamina.Initialize();
            this["Stamina"] = defaultStamina;
            stamina.SetMax(this["Stamina", true], null);
            mana.Owner = owner;
            mana.Initialize();
            this["Mana"] = defaultMana;
            mana.SetMax(this["Mana", true], null);
            this["Speed"] = defaultSpeed;
            this["Jump"] = defaultJump;
            this["Defence"] = 0;
            this["DefenceMultiply"] = 1.0f;
            this["Tenacity"] = 1.0f;

            ATAnimator ator = owner.GetAttribute<ATAnimator>();
            if (animator != null)
            {
                animator = ator.Animator;
            }
            else
            {
                //Debug.Log("一般获取属性动画机失败");
                owner.CreateDelegation("ATAnimator", (IAttribute at) =>
                {
                    //Debug.Log("成功获取动画机");
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
        /// 方向枚举
        /// </summary>
        public enum Direction
        { Left, Right }


        #endregion 相关

        #region 一般属性


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
        public ATStamina stamina;

        /// <summary>
        /// 角色魔力值
        /// </summary>
        public ATValue mana;

        [Header("属性初始化 - 仅在初始值有效")]
        public float defaultSpeed = 30f;
        public float defaultJump = 20f;
        public float defaultHealth = 100f;
        public float defaultMana = 100f;
        public float defaultStamina = 100f;

        #endregion 一般属性

        #region 状态

        /// <summary>
        /// 是否可控制动作
        /// </summary>
        private bool control_act = true;

        /// <summary>
        /// 是否可控制朝向
        /// </summary>
        private bool control_dir = true;
        /// <summary>
        /// 是否可控制攻击
        /// </summary>
        private bool control_attack = true;
        /// <summary>
        /// 是否可控制防御
        /// </summary>
        private bool control_defence = true;
        /// <summary>
        /// 是否可控制技能
        /// </summary>
        private bool control_skill = true;
        /// <summary>
        /// 是否可控制架势
        /// </summary>
        private bool control_posture = true;
        /// <summary>
        /// 是否可控制使用物品
        /// </summary>
        private bool control_use_item = true;

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

        private int fightCount = 0;
        /// <summary>
        /// 敌人数量, 为0时是 和平状态, 否则为战斗状态
        /// </summary>
        public int FightCount
        {
            get => fightCount;
            set
            {
                fightCount = value;
                if (fightCount != 0) fighting = true;
                else fighting = false;
            }
        }

        /// <summary>
        /// 是否眩晕
        /// </summary>
        private bool vertigo = false;

        #endregion 状态

        #region 属性器

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
                if(value)BreakAction();
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
                if (!value) BreakAction();
            }
        }

        private void BreakAction()
        {
            if (actionManager.GetActionState("Attack") != MDAction.ActionState.Disable)
            {
                Debug.Log("中断攻击");
                actionManager.Break("Attack");
            }
        }

        /// <summary>
        /// 是否可控制朝向
        /// </summary>
        public bool ControlDir { get => control_dir; set => control_dir = value; }

        public bool Dead
        {
            get => dead;
            set
            {
                dead = value;
                animator.SetBool("dead", value);
            }
        }

        /// <summary>
        /// 是否可控制攻击
        /// </summary>
        public bool ControlAttack { get => control_attack; set => control_attack = value; }
        /// <summary>
        /// 是否可控制防御
        /// </summary>
        public bool ControlDefence { get => control_defence; set => control_defence = value; }
        /// <summary>
        /// 是否可控制技能
        /// </summary>
        public bool ControlSkill { get => control_skill; set => control_skill = value; }
        /// <summary>
        /// 是否可控制架势
        /// </summary>
        public bool ControlPosture { get => control_posture; set => control_posture = value; }
        /// <summary>
        /// 是否可控制使用物品
        /// </summary>
        public bool ControlUseItem { get => control_use_item; set => control_use_item = value; }



        #endregion 属性器

        #region 动态属性(数字型)

        /// <summary>
        /// 默认属性值
        /// </summary>
        private Dictionary<string, CorrectValue> attributes = new();

        /// <summary>
        /// 创建一个动态属性
        /// </summary>
        /// <param name="attributeName">动态属性名称</param>
        /// <param name="defaultValue">默认值</param>
        public void CreateAttribute(string attributeName, float defaultValue = 0)
        {
            if (attributes.ContainsKey(attributeName))
            {
                Debug.LogWarning($"该属性已经创建: {attributeName}");
                return;
            }
            attributes[attributeName] = new CorrectValue(defaultValue);
        }

        /// <summary>
        /// 移除指定动态属性
        /// </summary>
        /// <param name="attributeName"></param>
        public void RemoveAttribute(string attributeName)
        {
            if (attributes.ContainsKey(attributeName))
            {
                attributes.Remove(attributeName);
            }
        }

        /// <summary>
        /// 添加动态属性的修正委托
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="correct"></param>
        public void AddCorrectDelegate(string attributeName, CorrectValueDelegate correct)
        {
            if (attributes.ContainsKey(attributeName))
            {
                attributes[attributeName].Add(correct);
            }
        }

        /// <summary>
        /// 移除动态属性的修正委托
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="correctTag"></param>
        public void RemoveCorrectDelegate(string attributeName, string correctTag)
        {
            if (attributes.ContainsKey(attributeName))
            {
                attributes[attributeName].Remove(correctTag);
                return;
            }
            Debug.Log($"移除修正失败: {correctTag}");
        }

        /// <summary>
        /// 获取属性值; 只能修改默认值
        /// </summary>
        /// <param name="attributeName">属性名称</param>
        /// <param name="Default">是否获取它的默认值</param>
        /// <returns></returns>
        public float this[string attributeName, bool Default = false]
        {
            get
            {
                if (!attributes.ContainsKey(attributeName))
                {
                    Debug.LogError($"未找到该属性:{attributeName}");
                    return -1;
                }
                var attribute = attributes[attributeName];
                if (Default) return attribute.DefaultValue;
                return attribute.Value;
            }

            set
            {
                if (!attributes.ContainsKey(attributeName))
                {
                    Debug.LogError($"未找到该属性:{attributeName}");
                    return;
                }
                var attribute = attributes[attributeName];
                attribute.DefaultValue = value;
            }
        }

        #endregion 动态属性(数字型)

        #region 其他方法

        /// <summary>
        /// 实体死亡, 销毁对象
        /// </summary>
        public void EntityDestroy()
        {
            Destroy(owner.gameObject);
        }

        #endregion 其他方法
    }
}