namespace Mod_Level
{
    /// <summary>
    /// 敌人状态
    /// </summary>
    public class ATEnemyState:ATCharacterState
    {
        #region 初始化

        public ATEnemyState():base()
        { 
            AttributeName = nameof(ATEnemyState); 

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        #endregion 初始化
    }
}