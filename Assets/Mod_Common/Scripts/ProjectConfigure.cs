
using ER;
using ER.Items;
using ER.Save;
using ER.Template;
using Mod_Level;
using Mod_Rouge;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mod_Common
{
    /// <summary>
    /// 项目配置器，用于对整个项目进行有序的初始化(应当首先初始化)
    /// </summary>
    public class ProjectConfigure : MonoSingleton<ProjectConfigure>,MonoInit
    {
        [Tooltip("存档目录路径")]
        public string SavePath;

        [Tooltip("地图配置文件路径")]
        public string MapSettingsPath;

        /// <summary>
        /// 预设设置
        /// </summary>
        private Dictionary<string,object> settings = new Dictionary<string,object>();

        public void Init()
        {
            
        }

        protected override void Awake()
        {
            base.Awake();
            Init();
            SavePath = Path.Combine(Application.streamingAssetsPath, "Saves");
            MapSettingsPath = Path.Combine(Application.streamingAssetsPath, "mapsettings.ini");
        }

        private void Start()
        {
            //启用预设设置选项
            //Settings();

            //设置使用的指令解释器
            //ConsolePanel.interpreter = new AInterpreter();
            //设置存档目录
            SaveManager.Instance.savePackPath = SavePath;
            //设置存档解析器为Json解析器
            SaveWrapper.Instance.interpreter = new JsonInterpreter();
            //初始化所有静态仓库
            TemplateStoreManager.Instance.Load();
        }
    }
}