using ER.UI.Animator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ER.UI.Animator.Players
{
    /// <summary>
    /// 渐变播放器: 支持一些组件的颜色渐变效果
    /// 子类型: type
    /// 支持动画:
    /// - 文字/图片透明度渐变: gradient_alpha
    ///     *origin: UI图片/文字对象 (Image/TMP_Text)
    ///     *start: 变化起始值(float)
    ///     *end: 变化终点值(float)
    ///     *speed: 插值速度(float)
    /// - 文字/图片颜色渐变: gradient_color
    ///     *origin: UI图片/文字对象 (Image/TMP_Text)
    ///     *start_color: 颜色起始值(float)
    ///     *end_color: 颜色终点值(float)
    ///     *speed: 插值速度(float)
    /// </summary>
    public class GradientPlayer:IUIPlayer
    {
        public string Type => "gradient";

        public bool Update(UIAnimationCD cd, float deltaTime)
        {
            string type = (string)cd["type"];
            switch (type)
            {
                case "gradient_alpha":
                    GradientAlpha(cd, deltaTime);
                    break;
                case "gradient_color":
                    GradientColor(cd, deltaTime);
                    break;
            }

            return cd.Status != CDStatus.Playing;
        }

        private void GradientAlpha(UIAnimationCD cd, float deltaTime)
        {
            object origin = cd["origin"];
            if(origin is Image)
            {
                Image img = (Image)origin;
                float start = (float)cd["start"];
                float end = (float)cd["end"];
                float speed = (float)cd["speed"];
                float progress = (float)cd.GetVar("progress", 0f);

                float delta_progress = Math.Min(speed * deltaTime, 1 - progress);
                progress += delta_progress;
                cd.SetVar("progress", progress);

                float alpha = Mathf.Lerp(start, end, progress);
                img.color = new Color(img.color.r,img.color.g,img.color.b, alpha);

                if (progress >= 1)
                {
                    cd.Done();
                }
            }
            else if (origin is TMP_Text)
            {
                TMP_Text txt = (TMP_Text)origin;
                float start = (float)cd["start"];
                float end = (float)cd["end"];
                float speed = (float)cd["speed"];
                float progress = (float)cd.GetVar("progress", 0f);

                float delta_progress = Math.Min(speed * deltaTime, 1 - progress);
                progress += delta_progress;
                cd.SetVar("progress", progress);

                float alpha = Mathf.Lerp(start, end, progress);
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha);

                if (progress >= 1)
                {
                    cd.Done();
                }
            }
            else
            {
                Debug.LogError("缺失渐变目标源");
                cd.Done();
            }
        }

        private void GradientColor(UIAnimationCD cd, float deltaTime)
        {
            object origin = cd["origin"];
            if (origin is Image)
            {
                Image img = (Image)origin;
                Color start = (Color)cd["start_color"];
                Color end = (Color)cd["end_color"];
                float speed = (float)cd["speed"];
                float progress = (float)cd.GetVar("progress", 0f);

                float delta_progress = Math.Min(speed * deltaTime, 1 - progress);
                progress += delta_progress;
                cd.SetVar("progress", progress);

                img.color = Color.Lerp(start, end, progress);

                if (progress >= 1)
                {
                    cd.Done();
                }
            }
            else if (origin is TMP_Text)
            {
                TMP_Text txt = (TMP_Text)origin;
                Color start = (Color)cd["start_color"];
                Color end = (Color)cd["end_color"];
                float speed = (float)cd["speed"];
                float progress = (float)cd.GetVar("progress", 0f);

                float delta_progress = Math.Min(speed * deltaTime, 1 - progress);
                progress += delta_progress;
                cd.SetVar("progress", progress);

                txt.color = Color.Lerp(start, end, progress);

                if (progress >= 1)
                {
                    cd.Done();
                }
            }
            else
            {
                Debug.LogError("缺失渐变目标源");
                cd.Done();
            }
        }
    }
}
