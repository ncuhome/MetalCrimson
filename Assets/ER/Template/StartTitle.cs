using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ER.Template
{
    /// <summary>
    /// 开始界面模板
    /// </summary>
    public class StartTitle : MonoBehaviour, MonoInit
    {
        [Header("按钮")]
        [SerializeField]
        private Button bt_start;
        [SerializeField]
        private Button bt_seting;
        [SerializeField]
        private Button bt_exit;
        [SerializeField]
        private Button bt_staff;
        [Header("场景加载配置")]
        [Tooltip("开始游戏跳转场景名称")]
        public string GameSceneName = "GameMain";
        [Tooltip("开始游戏跳转场景名称")]
        public string SettingSceneName = "GameSetting";
        [Tooltip("开始游戏跳转场景名称")]
        public string StaffSceneName = "GameStaff";

        private void Awake()
        {
            
        }
        #region 按钮响应
        private void BT_StartGame()
        {
            Debug.Log("开始游戏");
            SceneManager.Instance.LoadScene(GameSceneName);
        }
        private void BT_CloseGame()
        {
            Debug.Log("退出游戏");
            Application.Quit();
        }
        private void BT_Settings()
        {
            Debug.Log("设置界面");
            SceneManager.Instance.CoverScene(SettingSceneName);
        }
        private void BT_Staff()
        {
            Debug.Log("职员表");
            SceneManager.Instance.CoverScene(StaffSceneName);
        }

        public void Init()
        {

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            if (!enabled)
                enabled = true;
            bt_start.onClick.AddListener(BT_StartGame);
            bt_seting.onClick.AddListener(BT_Settings);
            bt_exit.onClick.AddListener(BT_CloseGame);
            bt_staff.onClick.AddListener(BT_Staff);

            SceneManager.Instance.LoadScene(Application.streamingAssetsPath);

            MonoLoader.InitCallback();
        }
        #endregion
    }

}