using ER.Entity2D;
using ER.Items;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 武器
    /// </summary>
    public class ATWeapon : MonoAttribute
    {
        #region 武器属性
        public float weight;//重量
        public float sharpness;//锋利度
        public float durable;//耐久
        public float utility;//耐久性

        private ATActionRegion AttackRegion;
        private ATActionRegion DefenceRegion;
        private ATActionRegion PassiveRegion;

        private ItemVariable weaponItem;//武器对应的物品


        public ATWeapon() { AttributeName = nameof(ATWeapon); }
        public override void Initialize()
        {
            
        }
        #endregion

        /// <summary>
        /// 更新武器属性(未写完)
        /// </summary>
        public void UpdateInfo(Weapon info)
        {

        }
    }
}