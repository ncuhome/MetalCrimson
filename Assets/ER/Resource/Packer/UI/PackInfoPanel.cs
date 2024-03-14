using ER.Parser;
using ER.UI;
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

        private UIAnimationInfo cd_panel;
        private UIAnimationInfo cd_text_1_fade;
        private UIAnimationInfo cd_text_2_fade;
        private UIAnimationInfo cd_text_3_fade;

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
            cd_panel = UIAnimator.CreateAnimationInfo(Background);
            cd_text_1_fade = UIAnimator.CreateAnimationInfo(titleText.rectTransform);
            cd_text_2_fade = UIAnimator.CreateAnimationInfo(versionText.rectTransform);
            cd_text_3_fade = UIAnimator.CreateAnimationInfo(authorText.rectTransform);

            cd_panel.speed = animation_speed;
            cd_text_1_fade.speed = animation_text_speed;
            cd_text_2_fade.speed = animation_text_speed;
            cd_text_3_fade.speed = animation_text_speed;
            titleText.color = titleText.color.ModifyAlpha(0);
            versionText.color = versionText.color.ModifyAlpha(0);
            authorText.color = authorText.color.ModifyAlpha(0);
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

            cd_panel.type = UIAnimator.AnimationType.BoxOpen_Left;
            cd_panel.callBack = () =>
            {
                UIAnimator.Instance.StartAnimation(cd_text_1_fade);
                UIAnimator.Instance.StartAnimation(cd_text_2_fade);
                UIAnimator.Instance.StartAnimation(cd_text_3_fade);
            };
            cd_text_1_fade.type = UIAnimator.AnimationType.FadeIn;
            cd_text_1_fade.callBack = null;
            cd_text_2_fade.type = UIAnimator.AnimationType.FadeIn;
            cd_text_3_fade.type = UIAnimator.AnimationType.FadeIn;

            UIAnimator.Instance.StartAnimation(cd_panel);
        }

        public void ClosePanel()
        {
            cd_panel.type = UIAnimator.AnimationType.BoxClose_Right;
            cd_panel.callBack = () => { gameObject.SetActive(false); };

            cd_text_1_fade.type = UIAnimator.AnimationType.FadeOut;
            cd_text_1_fade.callBack = () =>
            {
                UIAnimator.Instance.StartAnimation(cd_panel);
            };
            cd_text_2_fade.type = UIAnimator.AnimationType.FadeOut;
            cd_text_3_fade.type = UIAnimator.AnimationType.FadeOut;

            UIAnimator.Instance.StartAnimation(cd_text_1_fade);
            UIAnimator.Instance.StartAnimation(cd_text_2_fade);
            UIAnimator.Instance.StartAnimation(cd_text_3_fade);
        }

        private void OnValidate()
        {
            if (cd_panel == null || cd_text_1_fade == null || cd_text_2_fade == null || cd_text_3_fade == null) return;
            cd_panel.speed = animation_speed;
            cd_text_1_fade.speed = animation_text_speed;
            cd_text_2_fade.speed = animation_text_speed;
            cd_text_3_fade.speed = animation_text_speed;
        }
    }
}