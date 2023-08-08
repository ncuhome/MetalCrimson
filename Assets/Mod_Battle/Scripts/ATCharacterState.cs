﻿// Ignore Spelling: mana

using ER.Entity;
using ER.Entity2D;

namespace Mod_Battle
{
    public class ATCharacterState : NormalAttribute
    {
        #region 初始化
        public ATCharacterState() { AttributeName = nameof(ATCharacterState); }
        public override void Initialize()
        {
            
        }
        #endregion

        #region 属性
        /// <summary>
        /// 架势类型
        /// </summary>
        public enum Posture { Up,Front,Down}
        /// <summary>
        /// 当前角色架势
        /// </summary>
        public Posture posture = Posture.Front;
        /// <summary>
        /// 角色生命值（体力）
        /// </summary>
        public ATValue health;
        /// <summary>
        /// 角色耐性值（精力）
        /// </summary>
        public ATValue stamina;
        /// <summary>
        /// 角色魔力值
        /// </summary>
        public ATValue mana;

        #endregion

    }
}