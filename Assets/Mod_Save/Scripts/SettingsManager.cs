using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Save
{
    /// <summary>
    /// 设置管理类
    /// </summary>
    public class SettingsManager:MonoBehaviour
    {
        #region 单例模式
        public static SettingsManager Instance { get; private set; } = null;
        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(this); }
            settingsPath = Application.persistentDataPath + "";
        }
        #endregion
        /// <summary>
        /// 设置文件所在的路径
        /// </summary>
        public string settingsPath;
    }
}
