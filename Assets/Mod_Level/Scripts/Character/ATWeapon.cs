using ER;
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

        public Transform bone_weapon_interface;//武器骨骼:weapon_interface transform


        public ATWeapon() { AttributeName = nameof(ATWeapon); }
        public override void Initialize()
        {
            
        }
        #endregion

        /// <summary>
        /// 更新武器属性(未写完)
        /// </summary>
        public void UpdateInfo(WeaponInfo info)
        {
            if (bone_weapon_interface == null) return;
            //销毁旧的武器部件
            for(int i=0;i<bone_weapon_interface.childCount;i++)
            {
                Destroy(bone_weapon_interface.GetChild(i).gameObject);
            }
            for(int i=0;i<info.components.Count;i++)
            {
                ComponentStruct component = info.components[i];
                GameObject cp = PrefabManager.Instance[component.NameTmp];
                cp.transform.localPosition = component.position;
                cp.transform.rotation = component.rotation;
                cp.transform.localScale = component.scale;
            }
        }
    }
}