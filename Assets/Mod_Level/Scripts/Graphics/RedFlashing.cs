using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 受伤动画脚本
    /// </summary>
    public class RedFlashing:MonoBehaviour
    {
        private SpriteRenderer[] sprites;
        private Color[] oldColor;
        public Color aimColor;
        public float speed = 1f;
        private float timer;
        private void Awake()
        {
            sprites = GetComponentsInChildren<SpriteRenderer>();
            oldColor = new Color[sprites.Length];
        }

        public void Flash()
        {
            if(timer > 0)
            {
                timer = 0;
                enabled = true;
                return;
            }
            for(int i=0;i<sprites.Length;i++)
            {
                oldColor[i] = sprites[i].color;
            }
            enabled = true;
        }
        private void Update()
        {
            timer += Time.deltaTime * speed;
            float t = timer*2;
            if(t>1)
            {
                t = 2 - t;
            }
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = ColorLerp(oldColor[i],aimColor,t);
            }

            if (timer >= 1)
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].color = oldColor[i];
                }

                timer = 0;
                enabled = false;
            }
        }

        private Color ColorLerp(Color c1,Color c2,float t)
        {
            //Debug.Log($"颜色过渡:t:{t},c1:{c1.r},c2:{c2.r},aim:{Mathf.Lerp(c1.r, c2.r, t)}");
            return new Color(Mathf.Lerp(c1.r,c2.r,t),
                Mathf.Lerp(c1.g, c2.g, t),
                Mathf.Lerp(c1.b, c2.b, t),
                Mathf.Lerp(c1.a, c2.a, t));
        }

    }
}