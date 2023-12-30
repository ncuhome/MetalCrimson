using ER;
using UnityEngine;

namespace Mod_Level
{
    public class LevelConfigure : MonoBehaviour,SceneConfigure
    {
        public string SceneName => "LevelScene";
        public void Initialize()
        {
            Debug.Log("关卡场景初始化");
        }
    }
}