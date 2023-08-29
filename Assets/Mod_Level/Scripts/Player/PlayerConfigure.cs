using ER;
using ER.Items;
using ER.Save;

namespace Mod_Level
{
    /// <summary>
    /// 玩家配置类:
    /// 用于玩家初始化 以及 记录玩家的信息
    /// </summary>
    public class PlayerConfigure : MonoSingleton<PlayerConfigure>, ISavable
    {
        public string Identifier => "player_configure";

        private ItemStore itemStore;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            //加入存档信息列表
            SaveWrapper.Instance.RegisterObject(this);
        }

        public SaveEntry GetSaveEntry()
        {
            SaveEntry saves = new SaveEntry();
            saves.identifier = Identifier;
            return saves;
        }

        public void Restore(SaveEntry saveEntry)
        {
        }
    }
}