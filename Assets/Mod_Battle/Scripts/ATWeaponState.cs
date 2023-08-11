namespace ER.Entity2D
{
    /// <summary>
    /// 武器状态
    /// </summary>
    public class ATWeaponState : MonoAttribute
    {
        #region 初始化

        public ATWeaponState()
        { AttributeName = nameof(ATWeaponState); }

        public override void Initialize()
        {
        }

        #endregion 初始化

        #region 属性

        /// <summary>
        /// 基础质量
        /// </summary>
        public float defWeight;

        /// <summary>
        /// 基础锋利度
        /// </summary>
        public float defSharpness;

        /// <summary>
        /// 基础耐久度
        /// </summary>
        public float defDurability;

        /// <summary>
        /// 修正质量
        /// </summary>
        public float weight;

        /// <summary>
        /// 修正锋利度
        /// </summary>
        public float sharpness;

        /// <summary>
        /// 修正耐久度
        /// </summary>
        public float durability;

        #endregion 属性
    }
}