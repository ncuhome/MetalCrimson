using ER.Parser;
using UnityEngine;

namespace ER.TextPacker
{
    /// <summary>
    /// 设置语言包的面板
    /// </summary>
    public class PackagePanel : MonoSingleton<PackagePanel>
    {
        #region 组件

        /// <summary>
        /// 语言包路径
        /// </summary>
        public string packsPath = Application.streamingAssetsPath + "/Language";
        /// <summary>
        /// 替换器
        /// </summary>
        private TextReplacer replacer = new TextReplacer();

        [SerializeField]
        [Tooltip("资源列表")]
        private GameObject AssetPanel;

        [SerializeField]
        [Tooltip("加载列表")]
        private GameObject LoadPanel;

        [SerializeField]
        [Tooltip("物品预制体")]
        private GameObject ItemPrefab;

        [SerializeField]
        [Tooltip("加载区的物品预制体")]
        private GameObject LoadItemPrefab;

        #endregion 组件

        /// <summary>
        /// 显示设置面板
        /// </summary>
        public void DisplayPanel()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 更新资源列表
        /// </summary>
        [ContextMenu("刷新资源列表")]
        private void UpdateAssets()
        {
            //更新新的信息
            replacer.LanguagePackPath = packsPath;
            replacer.CheckPath();
            LanguagePackInfo[] infos = replacer.GetPackInfos();
            Debug.Log($"更新语言资源列表:{infos.Length}");
            //先销毁旧的资源选项卡
            if (AssetPanel == null)
            {
                Debug.LogError("资源列表绑定出错!");
                return;
            }
            while (infos.Length < AssetPanel.transform.childCount)//选项卡更多则需要销毁
            {
                Destroy(AssetPanel.transform.GetChild(0));
            }
            while (infos.Length > AssetPanel.transform.childCount)//选项卡更少则需要增加
            {
                Instantiate(ItemPrefab, AssetPanel.transform);
            }
            //更新信息
            for (int i = 0; i < AssetPanel.transform.childCount; i++)
            {
                PackItem pitem = AssetPanel.transform.GetChild(i).GetComponent<PackItem>();
                pitem.UpdateInfo(infos[i]);
            }
        }

        /// <summary>
        /// 打开资源文件夹
        /// </summary>
        public void OpenAssetFolder()
        {
            ERTool.ExplorePath(packsPath);
        }
        private void OnEnable()
        {
            UpdateAssets();
        }

        
    }
}