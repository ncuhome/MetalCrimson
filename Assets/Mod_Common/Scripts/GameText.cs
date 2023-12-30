/// <summary>
/// 游戏图层名称类
/// </summary>
public static class GameLayerText
{
    /// <summary>
    /// 玩家实体图层
    /// </summary>
    public const string L_PLAYER = "LPlayer";
    /// <summary>
    /// 敌人实体图层
    /// </summary>
    public const string L_ENEMY = "LEnemy";
    /// <summary>
    /// 障碍物图层: 与实体绝对交互
    /// </summary>
    public const string L_BLOCK = "LBlock";
    /// <summary>
    /// 机关物图层
    /// </summary>
    public const string L_MECHANISM = "LMechanism";
    /// <summary>
    /// 实体图层: 
    /// </summary>
    public const string L_ENTITY = "LEntity";
    /// <summary>
    /// 物品交互图层: 与实体不交互, 仅与掉落交互
    /// </summary>
    public const string L_ITEM_INTERACTIVE = "LItemInteractive";
    /// <summary>
    /// 武器效果图层: 
    /// </summary>
    public const string L_WEAPON = "LWeapon";
}
/// <summary>
/// 游戏标签文本类
/// </summary>
public static class GameTagText
{
    /// <summary>
    /// 炉子标签
    /// </summary>
    public const string F_FURNACE = "FBlastFurnace";
    /// <summary>
    /// 铁砧标签
    /// </summary>
    public const string F_ANVIL = "FAnvil";
    /// <summary>
    /// 模板物体标签
    /// </summary>
    public const string F_MODEL = "FModel";
    /// <summary>
    /// 武器攻击区域标签
    /// </summary>
    public const string FL_ATTACK_REGION = "FLAttackRegion";
    /// <summary>
    /// 武器防御区域标签
    /// </summary>
    public const string FL_DEFENCE_REGION = "FLDefenceRegion";
    /// <summary>
    /// 武器的攻击区域和防御区域复合标签
    /// </summary>
    public const string FL_ATTACK_DEFENCE_REGION = "FLAttackDefenceRegion";
    /// <summary>
    /// 玩家标签
    /// </summary>
    public const string L_PLAYER = "LPlayer";
    /// <summary>
    /// 敌人标签
    /// </summary>
    public const string L_ENEMY = "LEnemy";
    /// <summary>
    /// 障碍物标签
    /// </summary>
    public const string L_BLOCK = "LBlock";
    /// <summary>
    /// 可摧毁的障碍物标签
    /// </summary>
    public const string L_DESTROYABLE_BLOCK = "LDestroyableBlock";
    /// <summary>
    /// 功能性物体标签
    /// </summary>
    public const string L_TOOL = "LTool";
    /// <summary>
    /// 掉落物物体标签
    /// </summary>
    public const string L_COLLECTABLE = "LCollectable";
}