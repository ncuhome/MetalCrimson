using ER;
using System.Transactions;

namespace Mod_Level
{
    /// <summary>
    /// 交互UI显示管理面板
    /// </summary>
    public class InteractUIPanel:MonoSingleton<InteractUIPanel>
    {
        public InteractObject nowInteractObject = null;
        public void DisplayUI(InteractObject obj, UIInfo info)
        {
            if(nowInteractObject!=null)
            {

            }
        }
    }

}