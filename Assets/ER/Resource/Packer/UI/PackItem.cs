using ER.Control;
using ER.Parser;
using ER.UI;
using ER.UI.Animator;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ER.ResourcePacker
{
    /// <summary>
    /// 包选项单元
    /// </summary>
    public class PackItem : MonoBehaviour, IHandler
    {
        #region 属性

        private RectTransform rectTransform;
        private ResourcePackInfo info;

        public ResourcePackInfo Info
        { get { return info; } }

        [Header("组件绑定")]
        [SerializeField]
        [Tooltip("封面图片")]
        private Image image;

        [SerializeField]
        [Tooltip("包标题")]
        private TMP_Text titleText;

        [SerializeField]
        [Tooltip("包版本")]
        private TMP_Text versionText;

        [SerializeField]
        [Tooltip("包作者")]
        private TMP_Text authorText;

        [SerializeField]
        [Tooltip("背景")]
        private RectTransform Background;

        [SerializeField]
        [Tooltip("移除按钮")]
        private Button button;

        public Image ImageUsing
        { get { return image; } }

        public TMP_Text TitleText
        { get { return titleText; } }

        public TMP_Text VersionText
        { get { return versionText; } }

        public TMP_Text AuthorText
        { get { return authorText; } }

        [Header("默认资源")]
        [SerializeField]
        [Tooltip("默认图片")]
        private Sprite defSprite;

        [SerializeField]
        [Tooltip("默认标题")]
        private string defTitle = "未知语言包";

        [Header("其他设置")]
        [Range(10, 99)]
        [SerializeField]
        [Tooltip("最大显示长度")]
        private int maxLength = 16;

        [SerializeField]
        [Tooltip("正常状态颜色")]
        private Color color_ok = Color.green;

        [SerializeField]
        [Tooltip("异常状态颜色")]
        private Color color_warming = Color.red;

        public bool load = false;

        #endregion 属性

        #region 动画cd

        private UIAnimationCD cd_panel;
        private UIAnimationCD cd_text_1_fade;
        private UIAnimationCD cd_text_2_fade;
        private UIAnimationCD cd_text_3_fade;
        private UIAnimationCD cd_button_fade;

        [SerializeField]
        [Tooltip("动画速率")]
        private float animation_speed = 5f;

        [SerializeField]
        [Tooltip("动画速率")]
        private float animation_fade_speed = 5f;

        #endregion 动画cd

        #region 功能

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            cd_panel = Background.CreateUICD("left_open_x1");
            cd_text_1_fade = titleText.rectTransform.CreateUICD("gradient_x1");
            cd_text_1_fade["origin"] = titleText;
            cd_text_2_fade = versionText.rectTransform.CreateUICD("gradient_x2");
            cd_text_2_fade["origin"] = versionText;
            cd_text_3_fade = authorText.rectTransform.CreateUICD("gradient_x3");
            cd_text_3_fade["origin"] = authorText;

            if (button != null)
            {
                button.onClick.AddListener(ClosePanel);
                cd_button_fade = button.GetComponent<RectTransform>().CreateUICD("gradient_x4");
                cd_button_fade.Type = "gradient";
                cd_button_fade["origin"] = button.image;
                cd_button_fade["speed"] = animation_fade_speed;
                cd_button_fade["type"] = "gradient_alpha";

                button.image.color = button.image.color.ModifyAlpha(0);
                cd_button_fade.Register();
            }

            titleText.color = titleText.color.ModifyAlpha(0);
            versionText.color = versionText.color.ModifyAlpha(0);
            authorText.color = authorText.color.ModifyAlpha(0);

            cd_panel["speed"] = animation_fade_speed;
            cd_text_1_fade["speed"] = animation_fade_speed;
            cd_text_2_fade["speed"] = animation_fade_speed;
            cd_text_3_fade["speed"] = animation_fade_speed;

            cd_panel.Type = "box";
            cd_text_1_fade.Type = "gradient";
            cd_text_2_fade.Type = "gradient";
            cd_text_3_fade.Type = "gradient";

            cd_text_1_fade["type"] = "gradient_alpha";
            cd_text_2_fade["type"] = "gradient_alpha";
            cd_text_3_fade["type"] = "gradient_alpha";

            cd_panel.Register();
            cd_text_1_fade.Register();
            cd_text_2_fade.Register();
            cd_text_3_fade.Register();


        }

        private void Start()
        {
            OpenPanel();
        }

        public string LimitLength(string inputString)
        {
            if (Encoding.Default.GetByteCount(inputString) <= maxLength)
            {
                return inputString;
            }
            else
            {
                int byteLength = 0;
                StringBuilder sb = new StringBuilder();

                foreach (char c in inputString)
                {
                    byteLength += Encoding.Default.GetByteCount(c.ToString());

                    if (byteLength <= maxLength)
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        sb.Append("...");
                        break;
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// 同步显示信息
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfo(ResourcePackInfo info)
        {
            this.info = info;
            if (info.ImagePath != null)
            {
                string path = ERinbone.Combine(info.PackPath, info.ImagePath);
                path.Replace('/', '\\');

                //Debug.Log($"语言包所在的路径:{info.PackPath}");
                //Debug.Log($"图片路径:{path};存在:{File.Exists(path)}");
                if (File.Exists(path))
                {
                    Sprite sp = ObjectExpand.LoadTextureByIO(path).TextureToSprite();
                    image.sprite = sp;
                }
                else
                {
                    //Debug.Log("设置为默认图片");
                    image.sprite = defSprite;
                }
            }
            else
            {
                //Debug.Log("设置为默认图片");
                image.sprite = defSprite;
            }
            StringBuilder sb = new StringBuilder(info.PackName);
            titleText.text = LimitLength(sb.ToString());

            sb.Clear();
            sb.Append("ver:");
            sb.Append(info.PackVersion);
            versionText.text = LimitLength(sb.ToString());
            if (PackagePanel.IsMateVersion())
            {
                versionText.color = color_ok.ModifyAlpha(versionText.color.a);
            }
            else
            {
                versionText.color = color_warming.ModifyAlpha(versionText.color.a);
            }

            sb.Clear();
            sb.Append(info.PackAuthor);
            authorText.text = LimitLength(sb.ToString());
        }

        [ContextMenu("打开面板")]
        public void OpenPanel()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            cd_panel["type"] = "box_open";
            cd_panel["dir_open"] = Dir4.Left;
            cd_panel.SetCallback(() =>
            {
                cd_text_1_fade.Start();
                cd_text_2_fade.Start();
                cd_text_3_fade.Start();
                cd_button_fade?.Start();
            });

            cd_text_1_fade["start"] = 0f;
            cd_text_2_fade["start"] = 0f;
            cd_text_3_fade["start"] = 0f;
            

            cd_text_1_fade["end"] = 1f;
            cd_text_2_fade["end"] = 1f;
            cd_text_3_fade["end"] = 1f;

            if (cd_button_fade != null)
            {
                cd_button_fade["start"] = 0f;
                cd_button_fade["end"] = 1f;
            }

            cd_text_1_fade.SetCallback(null);

            cd_panel.Start();
        }

        public void ClosePanel()
        {
            if (button != null) button.onClick.RemoveAllListeners();

            cd_panel["type"] = "box_close";
            cd_panel["dir_open"] = Dir4.Right;
            cd_panel.SetCallback(() =>
            {
                Destroy(gameObject);
            });

            cd_text_1_fade["start"] = 1f;
            cd_text_2_fade["start"] = 1f;
            cd_text_3_fade["start"] = 1f;


            cd_text_1_fade["end"] = 0f;
            cd_text_2_fade["end"] = 0f;
            cd_text_3_fade["end"] = 0f;


            cd_text_1_fade.SetCallback(()=>cd_panel.Start());

            if (cd_button_fade != null)
            {
                cd_button_fade["start"] = 1f;
                cd_button_fade["end"] = 0f;
                cd_button_fade.Start();
            }

            cd_text_1_fade.Start();
            cd_text_2_fade.Start();
            cd_text_3_fade.Start();


        }

        #endregion 功能

        #region 鼠标控制

        public void OnBeginDrag(PointerEventData eventData)
        {
            PackagePanel.Instance.StartDrag(info, this);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PackagePanel.Instance.UpdateInfoDisplay(info, image.sprite);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }

        public void OnPointerMove(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        #endregion 鼠标控制
    }
}