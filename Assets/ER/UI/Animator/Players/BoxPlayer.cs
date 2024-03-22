using ER;
using ER.UI.Animator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.ER.UI.Animator.Players
{
    /// <summary>
    /// 盒子类动画播放器
    /// 拓展属性:
    /// * type: 子类别
    /// 支持动画:
    /// - 无起点单向平移: no_start_move
    ///     * dir: 方向(Vector2)
    ///     * distance: 距离(float)
    ///     * speed: 移动速度(float)
    /// - 盒子打开动画: box_open
    ///     * dir_open: 方向(Dir4)
    ///     * speed: 动画速度(float)
    /// - 盒子关闭动画: box_close
    ///     * dir_close: 方向(Dir4)
    ///     * speed: 动画速度(float)
    /// </summary>
    public class BoxPlayer : IUIPlayer
    {
        public string Type => "box";

        public bool Update(UIAnimationCD cd,float deltaTime)
        {
            string type = (string)cd["type"];
            switch(type)
            {
                case "no_start_move":
                    NoStartMove(cd,deltaTime);
                    break;
                case "box_open":
                    OpenBox(cd,deltaTime);
                    break;
                case "box_close":
                    CloseBox(cd,deltaTime);
                    break;
            }

            return cd.Status != CDStatus.Playing;
        }

        private void NoStartMove(UIAnimationCD cd, float deltaTime)//无起点平移
        {
            float dis = (float)cd["distance"];
            float speed = (float)cd["speed"];
            float done = (float)cd.GetVar("distance_done",0f);

            float delta_dis = Mathf.Min(speed * deltaTime,dis-done);//防止移动超过要求距离

            done += delta_dis;
            cd.SetVar("distance_done", done);


            Debug.Log($"dis:{dis} done:{done}  move:{delta_dis} delta-time:{deltaTime}");

            Vector2 dir = ((Vector2)cd["dir"]).normalized;
            cd.Owner.position += (Vector3)dir * delta_dis;

            if(done>=dis)//表示动画完毕
            {
                cd.Done();
            }
        }

        private void OpenBox(UIAnimationCD cd,float deltaTime)//盒子打开动画
        {
            Dir4 dir = (Dir4)cd["dir_open"];
            float speed = (float)cd["speed"];
            float progress = (float)cd.GetVar("open_progress",0f);

            float delta_progress = Math.Min(speed * deltaTime, 1 - progress);
            progress += delta_progress;
            cd.SetVar("open_progress", progress);

            Vector2 start;
            Vector2 end;

            switch (dir)
            {
                case Dir4.Up:
                    start= new Vector2(cd.Owner.anchorMax.x, 0);
                    end = new Vector2(cd.Owner.anchorMax.x, 1);
                    cd.Owner.anchorMax =  Vector2.Lerp(start, end, progress);
                    break;
                case Dir4.Left:
                    start = new Vector2(1, cd.Owner.anchorMax.y);
                    end = new Vector2(0, cd.Owner.anchorMax.y);
                    cd.Owner.anchorMin = Vector2.Lerp(start, end, progress);
                    break;
                case Dir4.Right:
                    start = new Vector2(0, cd.Owner.anchorMax.y);
                    end = new Vector2(1, cd.Owner.anchorMax.y);
                    cd.Owner.anchorMax = Vector2.Lerp(start, end, progress);
                    break;
                case Dir4.Down:
                    start = new Vector2(cd.Owner.anchorMax.x, 1);
                    end = new Vector2(cd.Owner.anchorMax.x, 0);
                    cd.Owner.anchorMin = Vector2.Lerp(start, end, progress);
                    break;
            }

            if (progress >= 1)
            {
                cd.Done();
            }
        }

        private void CloseBox(UIAnimationCD cd, float deltaTime)//盒子关闭动画
        {
            Dir4 dir = (Dir4)cd["dir_close"];
            float speed = (float)cd["speed"];
            float progress = (float)cd.GetVar("open_progress", 0);

            float delta_progress = Math.Min(speed * deltaTime, 1 - progress);
            progress += delta_progress;
            cd.SetVar("open_progress", progress);

            Vector2 start;
            Vector2 end;

            switch (dir)
            {
                case Dir4.Up:
                    start = new Vector2(cd.Owner.anchorMax.x, 0);
                    end = new Vector2(cd.Owner.anchorMax.x, 1);
                    cd.Owner.anchorMin = Vector2.Lerp(start, end, progress);
                    break;
                case Dir4.Left:
                    start = new Vector2(1, cd.Owner.anchorMax.y);
                    end = new Vector2(0, cd.Owner.anchorMax.y);
                    cd.Owner.anchorMax = Vector2.Lerp(start, end, progress);
                    break;
                case Dir4.Right:
                    start = new Vector2(0, cd.Owner.anchorMax.y);
                    end = new Vector2(1, cd.Owner.anchorMax.y);
                    cd.Owner.anchorMin = Vector2.Lerp(start, end, progress);
                    break;
                case Dir4.Down:
                    start = new Vector2(cd.Owner.anchorMax.x, 1);
                    end = new Vector2(cd.Owner.anchorMax.x, 0);
                    cd.Owner.anchorMax = Vector2.Lerp(start, end, progress);
                    break;
            }

            if (progress >= 1)
            {
                cd.Done();
            }
        }
    }
}
