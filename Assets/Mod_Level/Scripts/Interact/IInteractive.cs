namespace Mod_Level
{
    public interface IInteractive
    {
        /// <summary>
        /// 进入交互状态
        /// </summary>
        public void EnterInteract();
        /// <summary>
        /// 离开交互状态
        /// </summary>
        public void ExitInteract();

    }
}