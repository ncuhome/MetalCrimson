using ER.Parser;
using UnityEngine;

namespace ER.TextPacker
{
    /// <summary>
    /// 设置语言包的面板
    /// </summary>
    public class PackagePanel:MonoSingleton<PackagePanel>
    {
        #region 组件
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
        #endregion

        /// <summary>
        /// 更新资源列表
        /// </summary>
        [ContextMenu("刷新资源列表")]
        private void UpdateAssets()
        {
            //更新新的信息
            LanguagePackInfo[] infos = replacer.GetPackInfos();

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

            }

        }

        private void OnEnable()
        {
            UpdateAssets();
            replacer.LanguagePackPath = TextManager.Instance.packsPath;
        }
    }
}