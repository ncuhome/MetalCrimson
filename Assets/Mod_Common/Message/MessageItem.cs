// Ignore Spelling: Color

using ER;
using ER.UI.Animator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

namespace Mod_Common.Message
{
    /// <summary>
    /// 消息项目
    /// </summary>
    public class MessageItem:Water
    {

        /// <summary>
        /// 消息停留时间
        /// </summary>
        public static float keep_time=3f;

        public static float animation_speed = 300f;

        [SerializeField]
        private TMP_Text text;
        [SerializeField]
        private Image image;
        [SerializeField]
        private RectTransform self;

        private float timer;

        private UIAnimationCD anicd;
        private UIAnimationCD anicd_0;

        private bool inited = false;
        private bool visible = false;
        /// <summary>
        /// 是否可视
        /// </summary>
        public bool Visible=>visible;
        /// <summary>
        /// 该对象的Recttransform对象
        /// </summary>
        public RectTransform Self => self;

        /// <summary>
        /// 设置显示内容
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="text"></param>
        public void SetMessage(Sprite sprite,string text)
        {
            image.sprite = sprite;
            this.text.text = text;

            LayoutRebuilder.MarkLayoutForRebuild(this.text.rectTransform);
            LayoutRebuilder.MarkLayoutForRebuild(self);
        }
        /// <summary>
        /// 设置图片色相
        /// </summary>
        /// <param name="c"></param>
        public void SetImageColor(Color c)
        {
            image.color = c;
        }
        /// <summary>
        /// 设置文字色相
        /// </summary>
        /// <param name="c"></param>
        public void SetTextColor(Color c) 
        {
            text.color = c;
        }

        public override void ResetState()
        {
            image.sprite = null;
            image.color = Color.white;
            text.text = string.Empty;
            text.color = Color.white;
            timer = keep_time;
            enabled= false;
            visible = false;
        }
        /// <summary>
        /// 移动该对象(UI动画)
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="callback"></param>
        public void Move(Vector2 dir,float distance,Action callback=null)
        {
            Debug.Log("移动距离:" + distance);
            anicd["distance"] = distance;
            anicd["dir"] = dir;
            anicd.SetCallback(callback);
            anicd.Start();
        }
        /// <summary>
        /// 移动该对象(UI动画), 使用0号cd
        /// </summary>
        private void Move_0(Vector2 dir, float distance, Action callback = null)
        {
            Debug.Log("0_移动距离:"+distance);
            anicd_0["distance"] = distance;
            anicd_0["dir"] = dir;
            anicd_0.SetCallback(callback);
            anicd_0.Start();
        }

        public void Display()
        {
            visible = true;
            Move_0(Vector2.right, self.sizeDelta.x, () =>
            {
                Debug.Log("完全显示");
                timer = keep_time;
                enabled = true;
            });
        }
        


        public void Hide()
        {
            Debug.Log("隐藏消息");
            Move_0(Vector2.left, self.sizeDelta.x, () =>
            {
                Debug.Log("完全隐藏");
                Destroy();
            });
        }

        protected override void OnHide()
        {
            visible = false;
            enabled = false;
        }

        public void Init()
        {
            inited = true;

            anicd_0 = self.CreateUICD("box_open_z0");
            anicd_0.Type = "box";
            anicd_0.auto_destroy = false;
            anicd_0["type"] = "no_start_move";
            anicd_0["speed"] = animation_speed;
            anicd_0.Register();

            anicd = self.CreateUICD("box_open_z1");
            anicd.Type = "box";
            anicd.auto_destroy = false;
            anicd["type"] = "no_start_move";
            anicd["speed"] = animation_speed;
            anicd.Register();

            enabled = false;
        }

        private void Awake()
        {
            if (!inited) Init();
        }

        private void Update()
        {
            if(timer>0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Hide();
                enabled = false;
            }
        }
    }
}
