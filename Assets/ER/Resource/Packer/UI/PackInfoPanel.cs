using ER.Parser;
using ER.UI;
using ER.UI.Animator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ER.ResourcePacker
{
    /// <summary>
    /// 包选项简化面板(信息面板)
    /// </summary>
    public class PackInfoPanel : MonoBehaviour
    {
        private RectTransform rectTransform;
        private ResourcePackInfo info;
        private PackItem item;

        public PackItem Item
        { get => item; set => item = value; }

        public ResourcePackInfo Info
        { get { return info; } }

        [Header("组件绑定")]
        [SerializeField]
        [Tooltip("封面图片")]
        private Image image;

        public Sprite Image { get => image.sprite; }

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
        private RectTransform Background;

        #region 动画cd
        private UIAnimationCD cd_panel;
        private UIAnimationCD cd_text_1_fade;
        private UIAnimationCD cd_text_2_fade;
        private UIAnimationCD cd_text_3_fade;

        [SerializeField]
        [Tooltip("动画速率")]
        private float animation_speed = 3f;

        [SerializeField]
        [Tooltip("动画速率")]
        private float animation_text_speed = 9f;

        #endregion 动画cd

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

            titleText.color = titleText.color.ModifyAlpha(0);
            versionText.color = versionText.color.ModifyAlpha(0);
            authorText.color = authorText.color.ModifyAlpha(0);

            cd_panel["speed"] = animation_speed;
            cd_text_1_fade["speed"] = animation_text_speed;
            cd_text_2_fade["speed"] = animation_text_speed;
            cd_text_3_fade["speed"] = animation_text_speed;

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

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 同步显示信息
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfo(ResourcePackInfo info, PackItem item)
        {
            this.info = info;
            this.item = item;
            image.sprite = item.ImageUsing.sprite;
            titleText.text = item.TitleText.text;
            versionText.text = item.VersionText.text;
            authorText.text = item.AuthorText.text;
            RectTransform tr = item.GetComponent<RectTransform>();
            Debug.Log($"RectTransform is null:{tr == null}  rectTransform is null:{rectTransform == null}");
            rectTransform.position = Input.mousePosition + new Vector3(2, -2, 0);
            rectTransform.offsetMax = tr.offsetMax;
            rectTransform.offsetMin = tr.offsetMin;
        }

        public void OpenPanel()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            titleText.color = titleText.color.ModifyAlpha(0);
            versionText.color = versionText.color.ModifyAlpha(0);
            authorText.color = authorText.color.ModifyAlpha(0);

            cd_panel["type"] = "box_open";
            cd_panel["dir_open"] = Dir4.Left;
            cd_panel.SetCallback(() =>
            {
                cd_text_1_fade.Start();
                cd_text_2_fade.Start();
                cd_text_3_fade.Start();
            });

            cd_text_1_fade["start"] = 0f;
            cd_text_2_fade["start"] = 0f;
            cd_text_3_fade["start"] = 0f;


            cd_text_1_fade["end"] = 1f;
            cd_text_2_fade["end"] = 1f;
            cd_text_3_fade["end"] = 1f;

            cd_text_1_fade.SetCallback(null);

            cd_panel.Start();
        }

        public void ClosePanel()
        {
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


            cd_text_1_fade.SetCallback(() => cd_panel.Start());

            cd_text_1_fade.Start();
            cd_text_2_fade.Start();
            cd_text_3_fade.Start();


        }

        private void OnValidate()
        {
            if (cd_panel == null || cd_text_1_fade == null || cd_text_2_fade == null || cd_text_3_fade == null) return;
            cd_panel["speed"] = animation_speed;
            cd_text_1_fade["speed"] = animation_text_speed;
            cd_text_2_fade["speed"] = animation_text_speed;
            cd_text_3_fade["speed"] = animation_text_speed;
        }
    }
}