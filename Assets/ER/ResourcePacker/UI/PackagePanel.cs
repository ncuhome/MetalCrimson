using ER.Parser;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.ResourcePacker
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

        private static string UITextResourceKey = "UI_language";

        /// <summary>
        /// 资源包文件夹路径
        /// </summary>
        public string PacksPath = string.Empty;

        /// <summary>
        /// 资源包信息表
        /// </summary>
        private List<ResourcePackInfo> packs = new List<ResourcePackInfo>();

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

            //更新UI文本
            void UITextInit()
            {
                UpdateUIText(titleAssetText, "asset_list", "Available Resources");
                UpdateUIText(titleLoadText, "load_list", "Resources to be Applied");
                UpdateUIText(warningText, "text_warning", "Warning: The language pack and game version do not match and may not function properly");
                UpdateUIText(CancelButton.GetComponentInChildren<TMP_Text>(), "button_cancel", "Restore");
                UpdateUIText(OpenFolderButton.GetComponentInChildren<TMP_Text>(), "button_open_folder", "Open Resource Folder");
                UpdateUIText(ApplyButton.GetComponentInChildren<TMP_Text>(), "button_apply", "Apply");//更新应用按钮

                DisplayDefaultInfo();
            }
            GameResource.Instance.ELoad(GameResource.ResourceType.INI, UITextInit, UITextResourceKey);
        }

        [ContextMenu("关闭面板")]
        public void ClosePanel()
        {
            ClearAssetPanel();
            ClearLoadPanel();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 更新显示面板
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfoDisplay(ResourcePackInfo info, Sprite sprite)
        {
            
            infoImage.sprite = sprite;

            void UIUpdate()
            {

                string title = GameResource.Instance.GetTextPart(UITextResourceKey, "text_info_title");
                string author = GameResource.Instance.GetTextPart(UITextResourceKey, "text_info_author"); ;
                string version = GameResource.Instance.GetTextPart(UITextResourceKey, "text_info_version");
                string description = GameResource.Instance.GetTextPart(UITextResourceKey, "text_info_description");

                if (title == null || title == string.Empty) { title = "Title"; }
                if (author == null || author == string.Empty) { author = "Author"; }
                if (version == null || version == string.Empty) { version = "Version"; }
                if (description == null || description == string.Empty) { description = "Description"; }
                StringBuilder sb = new StringBuilder();

                sb.Append("<b>");
                sb.Append(title);
                sb.Append(": </b>");
                sb.Append(info.PackName);
                titleText.text = sb.ToString();
                sb.Clear();

                sb.Append("<b>");
                sb.Append(author);
                sb.Append(": </b>");
                sb.Append(info.PackAuthor);
                authorText.text = sb.ToString();
                sb.Clear();

                sb.Append("<b>");
                sb.Append(version);
                sb.Append(": </b>");
                sb.Append(info.PackVersion);
                versionText.text = sb.ToString();
                sb.Clear();

                sb.Append("<b>");
                sb.Append(description);
                sb.Append(": </b>");
                sb.Append(info.PackDescription);
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

            GameResource.Instance.ELoad(GameResource.ResourceType.INI, UIUpdate, UITextResourceKey);
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
        public void StartDrag(ResourcePackInfo info, PackItem item)
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
            UpdateInfoDisplay(ResourcePackInfo.Empty, DefImage);
        }

        /// <summary>
        /// 打开资源文件夹
        /// </summary>
        private void OpenAssetFolder()
        {
            ObjectExpand.ExplorePath(PacksPath);
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
            ResourcePackInfo[] packs = GetLoadPack();
            Debug.Log($"packs count:{packs.Length}");

            //更新自定资源包加载 配置文件
            INIWriter writer = new INIWriter();
            writer.AddSection("url");
            foreach (var pack in packs)
            {
                writer.AddPair("url", pack.PackName, pack.PackPath);
            }
            writer.Save(ResourceIndexer.custom_config_path);

            //重启资源索引器 并 清空当前资源管理器的资源缓存
            ResourceIndexer.Instance.Init();
            GR.Clear();
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

            //重新读取 自定义资源包 的配置文件
            //并复原这些资源包的信息
            if (File.Exists(ResourceIndexer.custom_config_path))
            {
                INIParser parser = new INIParser();
                parser.ParseINIFile(ResourceIndexer.custom_config_path);
                Dictionary<string, string> urls = parser.GetSection("url");
                ResourcePackInfo[] history = new ResourcePackInfo[urls.Count];
                int i = 0;
                foreach (var url in urls)
                {
                    history[i] = ResourcePack.GetInfo(url.Value);
                    i++;
                }
                for (i = 0; i < history.Length; i++)
                {
                    pitems.Add(Instantiate(LoadItemPrefab, LoadPanel.transform).GetComponent<PackItem>());
                }
                for (i = 0; i < pitems.Count; i++)
                {
                    Debug.Log($"childCount:{LoadPanel.transform.childCount} ,i:{i},history:{history.Length}");
                    pitems[i].UpdateInfo(history[i]);
                }
            }
            else
            {
                INIWriter writer = new INIWriter();
                writer.AddSection("url");
                writer.Save(ResourceIndexer.custom_config_path);
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
            CheckPath();
            ResourcePackInfo[] infos = GetPackInfos();

            ClearAssetPanel();
            //创建新的选项

            List<PackItem> pitems = new List<PackItem>();
            Debug.Log($"更新资源列表:{infos.Length}");
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
        private ResourcePackInfo[] GetLoadPack()
        {
            ResourcePackInfo[] infos = new ResourcePackInfo[LoadPanel.transform.childCount - 1];
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
            ResourcePackInfo[] infos = GetLoadPack();
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
            string txt = GameResource.Instance.GetTextPart(GameResource.GetINIKeyAll(UITextResourceKey, keyName));
            if (txt == null || txt == string.Empty)
                return defaultValue;
            return txt;
        }

        #endregion 内部方法

        /// <summary>
        /// 检查并更新资源包列表
        /// </summary>
        public void CheckPath()
        {
            packs.Clear();
            if (PathExist(PacksPath) && IsDirectory(PacksPath))//包路径是否为一个文件夹
            {
                string[] subdirectories = Directory.GetDirectories(PacksPath);
                foreach (string subdirectory in subdirectories)
                {
                    string path = ERinbone.Combine(subdirectory, ResourcePack.IllustrationFileName);

                    if (File.Exists(path))
                    {
                        packs.Add(ResourcePack.GetInfo(subdirectory));
                    }
                }
            }
        }

        public ResourcePackInfo[] GetPackInfos()
        {
            return packs.ToArray();
        }

        #region 静态工具方法

        public static bool PathExist(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsDirectory(string path)
        {
            FileAttributes attributes = File.GetAttributes(path);
            return (attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        #endregion 静态工具方法

        #region Unity

        private float timer = 0f;

        protected override void Awake()
        {
            base.Awake();
            ApplyButton.onClick.AddListener(Apply);
            CancelButton.onClick.AddListener(Cancel);
            OpenFolderButton.onClick.AddListener(OpenAssetFolder);
            RefrashButton.onClick.AddListener(UpdateAssets);
            PacksPath = ERinbone.ResourcePath;
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