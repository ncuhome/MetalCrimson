using ER.Parser;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.TextPacker
{
    /// <summary>
    /// 设置语言包的面板
    /// </summary>
    public class PackagePanel : MonoSingleton<PackagePanel>
    {
        #region 组件

        [Header("预制体")]
        [SerializeField]
        [Tooltip("物品预制体")]
        private GameObject ItemPrefab;

        [SerializeField]
        [Tooltip("检测区域预制体")]
        private GameObject RegionPrefab;

        [SerializeField]
        [Tooltip("加载区的物品预制体")]
        private GameObject LoadItemPrefab;

        [SerializeField]
        [Tooltip("信息区的RectTransform")]
        private RectTransform InfosPanel;

        [Header("面板控件")]
        [SerializeField]
        [Tooltip("资源列表")]
        private GameObject AssetPanel;

        [SerializeField]
        [Tooltip("加载列表")]
        private GameObject LoadPanel;

        [SerializeField]
        [Tooltip("信息图片")]
        private Image infoImage;

        [SerializeField]
        [Tooltip("语言包标题")]
        private TMP_Text titleText;

        [SerializeField]
        [Tooltip("语言包版本")]
        private TMP_Text versionText;

        [SerializeField]
        [Tooltip("语言包作者")]
        private TMP_Text authorText;

        [SerializeField]
        [Tooltip("语言包描述")]
        private TMP_Text descriptionText;

        [SerializeField]
        [Tooltip("悬浮选项")]
        private PackInfoPanel infoPanel;

        [Tooltip("加载检测区域")]
        [SerializeField]
        private Region region;

        [Header("按钮 | 文本")]
        [Tooltip("\"应用\"按钮")]
        [SerializeField]
        private Button ApplyButton;

        [Tooltip("\"恢复\"按钮")]
        [SerializeField]
        private Button CancelButton;

        [Tooltip("\"打开资源文件夹\"按钮")]
        [SerializeField]
        private Button OpenFolderButton;

        [Tooltip("\"刷新资源\"按钮")]
        [SerializeField]
        private Button RefrashButton;

        [SerializeField]
        [Tooltip("警告文本")]
        private TMP_Text warningText;

        [SerializeField]
        [Tooltip("资源标题文本")]
        private TMP_Text titleAssetText;

        [SerializeField]
        [Tooltip("加载标题文本")]
        private TMP_Text titleLoadText;

        /// <summary>
        /// 当前选中的区域
        /// </summary>
        public Region catchRegion;

        [Header("默认资源")]
        [Tooltip("默认图片")]
        [SerializeField]
        private Sprite DefImage;

        #endregion 组件

        #region 属性

        private static string TextSectionName = "UI.language";

        public string DefaultPackPath = Application.streamingAssetsPath + "/Language/DefualtLanguage";

        /// <summary>
        /// 语言包路径
        /// </summary>
        public string packsPath = Application.streamingAssetsPath + "/Language";

        /// <summary>
        /// 替换器
        /// </summary>
        private TextReplacer replacer = new TextReplacer();

        private List<LanguagePackInfo> history = new List<LanguagePackInfo>();//原先的语言包配置
        private bool draging = false;//正在拖拽
        private List<Region> regions = new List<Region>();//检测区域对象池

        #endregion 属性

        #region 公开方法

        /// <summary>
        /// 显示设置面板
        /// </summary>
        [ContextMenu("打开面板")]
        public void OpenPanel()
        {
            gameObject.SetActive(true);
            timer = -1;
            UpdateAssets();//更新资源列表
            timer = -1;
            Cancel();//回到当前资源方案
            TextManager.Pack.Load(TextSectionName);

            #region 更新UI文本

            UpdateUIText(titleAssetText, "asset_list", "Available Resources");
            UpdateUIText(titleLoadText, "load_list", "Resources to be Applied");
            UpdateUIText(warningText, "text_warning", "Warning: The language pack and game version do not match and may not function properly");
            UpdateUIText(CancelButton.GetComponentInChildren<TMP_Text>(), "button_cancel", "Restore");
            UpdateUIText(OpenFolderButton.GetComponentInChildren<TMP_Text>(), "button_open_folder", "Open Resource Folder");
            UpdateUIText(ApplyButton.GetComponentInChildren<TMP_Text>(), "button_apply", "Apply");//更新应用按钮

            DisplayDefaultInfo();

            #endregion 更新UI文本
        }

        [ContextMenu("关闭面板")]
        public void ClosePanel()
        {
            TextManager.Pack.UnLoad(TextSectionName);
            ClearAssetPanel();
            ClearLoadPanel();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 更新显示面板
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfoDisplay(LanguagePackInfo info, Sprite sprite)
        {
            infoImage.sprite = sprite;

            TextManager.Pack.Load("UI.language");
            string title = TextManager.Pack["UI.language.text_info_title"];
            string author = TextManager.Pack["UI.language.text_info_author"];
            string version = TextManager.Pack["UI.language.text_info_version"];
            string description = TextManager.Pack["UI.language.text_info_description"];

            if (title == null || title == string.Empty) { title = "Title"; }
            if (author == null || author == string.Empty) { author = "Author"; }
            if (version == null || version == string.Empty) { version = "Version"; }
            if (description == null || description == string.Empty) { description = "Description"; }
            StringBuilder sb = new StringBuilder();

            sb.Append("<b>");
            sb.Append(title);
            sb.Append(": </b>");
            sb.Append(info.LanguagePackName);
            titleText.text = sb.ToString();
            sb.Clear();

            sb.Append("<b>");
            sb.Append(author);
            sb.Append(": </b>");
            sb.Append(info.LanguagePackAuthor);
            authorText.text = sb.ToString();
            sb.Clear();

            sb.Append("<b>");
            sb.Append(version);
            sb.Append(": </b>");
            sb.Append(info.LanguagePackVersion);
            versionText.text = sb.ToString();
            sb.Clear();

            sb.Append("<b>");
            sb.Append(description);
            sb.Append(": </b>");
            sb.Append(info.LanguagePackDescription);
            descriptionText.text = sb.ToString();

            LayoutRebuilder.ForceRebuildLayoutImmediate(InfosPanel);//强制刷新竖直布局, 不刷新会有显示bug
            if (IsMateVersion())
            {
                versionText.color = Color.green;
                warningText.gameObject.SetActive(false);
            }
            else
            {
                versionText.color = Color.red;
                warningText.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 检测是否匹配当前版本(未完善)
        /// </summary>
        /// <returns></returns>
        public static bool IsMateVersion()
        {
            return true;
        }

        /// <summary>
        /// 开始拖拽选项
        /// </summary>
        /// <param name="info"></param>
        /// <param name="item"></param>
        public void StartDrag(LanguagePackInfo info, PackItem item)
        {
            draging = true;
            infoPanel.UpdateInfo(info, item);
            infoPanel.OpenPanel();
            region.gameObject.SetActive(true);
            ArrangeRegion();
        }

        #endregion 公开方法

        #region 内部方法

        /// <summary>
        /// 向用户发送信息
        /// </summary>
        private void SeedMessage(string text)
        {
            Debug.Log(text);
        }

        /// <summary>
        /// 移除资源列表中的所有选项
        /// </summary>
        private void ClearAssetPanel()
        {
            PackItem[] pitems = AssetPanel.transform.GetComponentsInChildren<PackItem>();
            foreach (var pitem in pitems)
            {
                pitem.ClosePanel();
            }
        }

        /// <summary>
        /// 移除加载列表中的所有选项
        /// </summary>
        private void ClearLoadPanel()
        {
            PackItem[] pitems = LoadPanel.transform.GetComponentsInChildren<PackItem>();
            foreach (var pitem in pitems)
            {
                pitem.ClosePanel();
            }
        }

        /// <summary>
        /// 像信息栏显示为默认状态
        /// </summary>
        private void DisplayDefaultInfo()
        {
            infoImage.sprite = DefImage;
            UpdateInfoDisplay(LanguagePackInfo.Empty, DefImage);
        }

        /// <summary>
        /// 打开资源文件夹
        /// </summary>
        private void OpenAssetFolder()
        {
            ERTool.ExplorePath(packsPath);
        }

        /// <summary>
        /// 应用当前加载配置
        /// </summary>
        private void Apply()
        {
            if (timer > 0)
            {
                SeedMessage("操作过于频繁, 请稍后重试~");
                return;
            }
            timer = 0.5f;
            LanguagePackInfo[] packs = GetLoadPack();
            history.Clear();
            LanguagePack lp = new LanguagePack(DefaultPackPath);
            Debug.Log($"packs count:{packs.Length}");
            foreach (var pack in packs)
            {
                Debug.Log(pack.LanguagePackName);
                lp.LoadPack(pack.LanguagePackPath);
                history.Add(pack);
            }
            TextManager.Pack = lp;
        }

        /// <summary>
        /// 回到原先的加载配置
        /// </summary>
        private void Cancel()
        {
            if (timer > 0)
            {
                SeedMessage("操作过于频繁, 请稍后重试~");
                return;
            }
            timer = 0.5f;
            //先销毁旧的资源选项卡
            if (AssetPanel == null)
            {
                Debug.LogError("资源列表绑定出错!");
                return;
            }

            ClearLoadPanel();
            List<PackItem> pitems = new List<PackItem>();
            for (int i = 0; i < history.Count; i++)
            {
                pitems.Add(Instantiate(LoadItemPrefab, LoadPanel.transform).GetComponent<PackItem>());
            }

            //更新信息
            for (int i = 0; i < pitems.Count; i++)
            {
                Debug.Log($"childCount:{LoadPanel.transform.childCount} ,i:{i},history:{history.Count}");
                pitems[i].UpdateInfo(history[i]);
            }
        }

        /// <summary>
        /// 更新资源列表
        /// </summary>
        [ContextMenu("刷新资源列表")]
        private void UpdateAssets()
        {
            if (timer > 0)
            {
                SeedMessage("操作过于频繁, 请稍后重试~");
                return;
            }
            timer = 0.5f;
            //更新新的信息
            replacer.LanguagePackPath = packsPath;
            replacer.CheckPath();
            LanguagePackInfo[] infos = replacer.GetPackInfos();

            ClearAssetPanel();
            //创建新的选项

            List<PackItem> pitems = new List<PackItem>();
            Debug.Log($"更新语言资源列表:{infos.Length}");
            for (int i = 0; i < infos.Length; i++)
            {
                pitems.Add(Instantiate(ItemPrefab, AssetPanel.transform).GetComponent<PackItem>());
            }
            //更新信息
            for (int i = 0; i < pitems.Count; i++)
            {
                pitems[i].UpdateInfo(infos[i]);
            }
        }

        /// <summary>
        /// 给列表检测区域排序
        /// </summary>
        private void ArrangeRegion()
        {
            //激活最顶部的检测区
            //LoadPanel.transform.GetChild(0).gameObject.SetActive(true);

            int used = 0;

            for (int i = 1; i < LoadPanel.transform.childCount; i += 2)
            {
                regions[used].transform.SetParent(LoadPanel.transform, false);
                regions[used].transform.SetSiblingIndex(i);
                regions[used].gameObject.SetActive(true);
                used++;
            }
        }

        /// <summary>
        /// 取消所有检测区域
        /// </summary>
        private void CancelAllRegion()
        {
            Debug.Log("取消所有检测区域");
            //LoadPanel.transform.GetChild(0).gameObject.SetActive(false);
            for (int i = 0; i < regions.Count; i++)
            {
                if (regions[i].gameObject.activeSelf)
                {
                    regions[i].transform.SetParent(transform, false);
                    regions[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 获取加载列表的语言包信息
        /// </summary>
        /// <returns></returns>
        private LanguagePackInfo[] GetLoadPack()
        {
            LanguagePackInfo[] infos = new LanguagePackInfo[LoadPanel.transform.childCount - 1];
            for (int i = 0; i < infos.Length; i++)
            {
                PackItem pitem = LoadPanel.transform.GetChild(i + 1).GetComponent<PackItem>();
                if (pitem == null) continue;
                infos[i] = pitem.Info;
            }
            return infos;
        }

        /// <summary>
        /// 将资源提交到加载区
        /// </summary>
        private PackItem AddLoadAsset()
        {
            //防止重复添加
            LanguagePackInfo[] infos = GetLoadPack();
            foreach (var info in infos)
            {
                if (info == infoPanel.Info)
                {
                    return null;
                }
            }
            //添加新的包到加载区
            PackItem pitem = Instantiate(LoadItemPrefab, LoadPanel.transform).GetComponent<PackItem>();
            pitem.UpdateInfo(infoPanel.Info);
            //同步显示区信息
            UpdateInfoDisplay(infoPanel.Info, infoPanel.Image);

            while (infos.Length >= regions.Count)
            {
                regions.Add(Instantiate(RegionPrefab, transform).GetComponent<Region>());
            }
            return pitem;
        }

        /// <summary>
        /// 改变资源顺序
        /// </summary>
        private void MoveAssetPosition(int index)
        {
            infoPanel.Item.transform.SetSiblingIndex(index);
        }

        /// <summary>
        /// 拖拽动作提交
        /// </summary>
        private void DragCofirm()
        {
            //关闭拖拽检测和检测区域
            draging = false;
            infoPanel.ClosePanel();
            region.gameObject.SetActive(false);

            //如果在空白处直接停止判定
            if (catchRegion == null)
            {
                CancelAllRegion();
                return;
            }

            if (infoPanel.Item.load)//加载区的物品
            {
                if (catchRegion == region)//如果拖到底框的话
                {
                    MoveAssetPosition(LoadPanel.transform.childCount - 1);
                }
                else
                {
                    MoveAssetPosition(catchRegion.transform.GetSiblingIndex());
                }
            }
            else
            {
                if (catchRegion == region)//如果拖到底框的话
                {
                    AddLoadAsset();
                }
                else
                {
                    PackItem pitem = AddLoadAsset();
                    if (pitem == null)
                    {
                        CancelAllRegion();
                        return;
                    }
                    infoPanel.Item = pitem;
                    MoveAssetPosition(catchRegion.transform.GetSiblingIndex());
                }
            }
            catchRegion = null;
            CancelAllRegion();
        }

        /// <summary>
        /// 更新控件的文本内容
        /// </summary>
        /// <param name="text"></param>
        /// <param name="kayName"></param>
        /// <param name="defaultValue"></param>
        private void UpdateUIText(TMP_Text text, string kayName, string defaultValue)
        {
            if (text == null) return;
            text.text = GetText(kayName, defaultValue);
        }

        private string GetText(string keyName, string defaultValue)
        {
            string txt = TextManager.Pack[TextSectionName, keyName];
            if (txt == null || txt == string.Empty)
                return defaultValue;
            return txt;
        }

        #endregion 内部方法

        #region Unity

        private float timer = 0f;

        protected override void Awake()
        {
            base.Awake();
            ApplyButton.onClick.AddListener(Apply);
            CancelButton.onClick.AddListener(Cancel);
            OpenFolderButton.onClick.AddListener(OpenAssetFolder);
            RefrashButton.onClick.AddListener(UpdateAssets);
        }

        private void Update()
        {
            if (draging)
            {
                infoPanel.transform.position = Input.mousePosition + new Vector3(2, -2, 0); ;
                if (Input.GetMouseButtonUp(0))
                {
                    DragCofirm();
                }
            }
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
        }

        #endregion Unity
    }
}