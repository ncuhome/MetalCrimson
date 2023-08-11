﻿// Ignore Spelling: mana

using ER.Entity2D;

namespace Mod_Battle
{
    public class ATCharacterState : MonoAttribute
    {
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

        #endregion 相关

        #region 基础属性

        /// <summary>
        /// 基础生命上限
        /// </summary>
        public float defHealth;

        /// <summary>
        /// 基础耐力上限
        /// </summary>
        public float defStamina;

        /// <summary>
        /// 基础魔力上限
        /// </summary>
        public float defMana;

        /// <summary>
        /// 基础角色重量
        /// </summary>
        public float defWeight;

        /// <summary>
        /// 基础移动速度
        /// </summary>
        public float defSpeed;

        /// <summary>
        /// 基础力量（用于做伤害参考值）
        /// </summary>
        public float defPower;

        /// <summary>
        /// 基础技能CD减免
        /// </summary>
        public float defCDReduction;

        /// <summary>
        /// 基础防御值
        /// </summary>
        public float defDefense;

        #endregion 基础属性

        #region 属性

        /// <summary>
        /// 当前角色架势
        /// </summary>
        public Posture posture = Posture.Front;

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
        public float defense;

        #endregion 属性
    }
}