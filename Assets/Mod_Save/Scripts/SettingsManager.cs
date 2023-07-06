using ER.Parser;
using Mod_Console;
using System;
using System.Collections.Generic;
using System.IO;
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
            settingsPath = Path.Combine(Application.streamingAssetsPath,"setting.ini");
            INIHD = new();
            UpdateSettings();
        }
        #endregion
        /// <summary>
        /// 设置文件所在的路径
        /// </summary>
        public string settingsPath;

        private INIHandler INIHD;

        /// <summary>
        /// 更新设置，从本地文件读取
        /// </summary>
        public void UpdateSettings()
        {
            if(!File.Exists(settingsPath))
            {
                File.Create(settingsPath).Close();
            }
            INIHD.ParseINIFile(settingsPath);


            if (INIHD.GetSection("settings") == null)
            {
                INIHD.AddSection("settings");
            }
        }

        public void SaveSettings()
        {
            ConsolePanel.Instance.Print("正在写入配置文件...");
            INIHD.Save(settingsPath);
        }

        /// <summary>
        /// 添加/修改的配置内容
        /// </summary>
        public void AddSetting(string key,string value)
        {
            INIHD.AddPair("settings",key,value);
        }
        public void RemoveSetting(string key)
        {
            INIHD.DeletePair("Settings", key);
        }
        public string this[string key]
        {
            get
            {
                return INIHD.GetValue("settings", key);
            }
            set
            {
                INIHD.AddPair("settings", key, value);
            }
        }
        /// <summary>
        /// 获取设置字符串
        /// </summary>
        public string GetSettingsTxt()
        {
            return INIHD.GetSaveString();
        }
    }
}
