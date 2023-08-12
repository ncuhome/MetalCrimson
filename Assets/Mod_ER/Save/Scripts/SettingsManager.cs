
using ER;
using ER.Parser;
using Mod_Console;
using System.IO;
using UnityEngine;

namespace ER.Save
{
    /// <summary>
    /// 设置管理类
    /// </summary>
    public class SettingsManager : MonoSingleton<SettingsManager>
    {
        protected override void Awake()
        {
            base.Awake();
            settingsPath = Path.Combine(Application.streamingAssetsPath, "settings.ini");
            INIHD = new();
            UpdateSettings();
        }

        /// <summary>
        /// 设置文件所在的路径
        /// </summary>
        private string settingsPath;

        private INIHandler INIHD;

        /// <summary>
        /// 更新设置，从本地文件读取
        /// </summary>
        public void UpdateSettings()
        {
            if (!File.Exists(settingsPath))
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
            INIHD.Save(settingsPath);
        }

        /// <summary>
        /// 添加/修改的配置内容
        /// </summary>
        public void AddSetting(string key, string value)
        {
            INIHD.AddPair("settings", key, value);
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

        private void OnDestroy()
        {
            SaveSettings();
        }
    }
}