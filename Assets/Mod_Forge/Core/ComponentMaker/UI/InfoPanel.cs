using UnityEngine;
using Mod_Resource;
using TMPro;
using ER;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using System;

namespace Mod_Forge
{
    public class InfoPanel:MonoBehaviour
    {
        [SerializeField]
        private TMP_Text txt_name;
        [SerializeField]
        private TMP_Text txt_io;
        [SerializeField]
        private TMP_Text txt_cost;
        [SerializeField]
        private TMP_Text txt_description;

        [Tooltip("羁绊容器")]
        [SerializeField]
        private RectTransform BoxC;
        [SerializeField]
        [Tooltip("描述文本容器")]
        private GameObject BoxB;

        private ObjectPool pool_leff;//羁绊对象池

        private List<PLeffItem> pLeffItems = new List<PLeffItem>();//当前被提取出的预制体

        private RectTransform rectf;//自身
        private ContentSizeFitter fitter;//自身
        private Vector2 size_tmp;//高度缓存
        private float prograss;//动画进度
        private bool follow_mouse;//跟随鼠标
        [SerializeField]
        private float animation_speed = 10;


        /// <summary>
        /// 输入直径
        /// </summary>
        private int in_diameter;
        /// <summary>
        /// 输出直径
        /// </summary>
        private int out_diameter;

        private void ClearLinkageEffect()
        {
            for(int i=0;i<pLeffItems.Count;i++)
            {
                pLeffItems[i].Destroy();
                pLeffItems[i].transform.SetParent(null);
            }
            pLeffItems.Clear();
        }
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="infos"></param>
        public void UpdateInfo(ItemInfos infos)
        {
            ClearLinkageEffect();
            txt_name.text = infos.name;
            if (infos.moreInfos)
            {
                BoxB.SetActive(true);
                txt_io.text = $"I/O: {infos.inD}/{infos.outD}";
                txt_cost.text = $"cost: {infos.cost}";
                txt_description.text = $"description: {infos.description}";
            }
            BoxB.SetActive(false);
            for (int i = 0; i < infos.linkageEffects.Length; i++)
            {
                LinkageEffectStack stack = infos.linkageEffects[i];
                PLeffItem itemn = (PLeffItem)pool_leff.GetObject();
                itemn.Value = stack;
                itemn.transform.SetParent(BoxC);
                pLeffItems.Add(itemn);
            }
        }
        /// <summary>
        /// 显示
        /// </summary>
        public void Open()
        {
            Debug.Log("显示信息框");
            size_tmp = rectf.sizeDelta;
            Debug.Log($"size:{size_tmp.x},{size_tmp.y}");
            fitter.enabled = false;//关闭自适应高度
            prograss = 0;
            follow_mouse = true;
            enabled = true;
          
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            Debug.Log("关闭信息框");
            follow_mouse = false;
            rectf.position = Vector3.left * 3000;
            enabled = false;
        }

        private void SetPos(Vector2 pos)
        {
            float nx = pos.x;
            float ny = pos.y;
            //TMP_Text test =((GameObject)AM.GetAnchor("TestText").Owner).GetComponent<TMP_Text>();
            //test.text = $"x:{nx} y:{ny} | end_x:{pos.x + size_tmp.x}  end_y:{pos.y - size_tmp.y} | w/2:{Screen.width}  h/2:{0}";
            if (pos.y - size_tmp.y < 0)//越过下边界, 需要翻转显示到上方
            {
                ny += size_tmp.y;
            }
            if(pos.x+size_tmp.x>Screen.width)//越过右边界,翻转至左方
            {
                nx-=size_tmp.x;
            }
            rectf.position = new Vector3(nx,ny,0);//更新位置
        }


        private void Awake()
        {
            this.RegisterAnchor("InfoPanel");
            size_tmp = new Vector2(300,400);
            rectf = GetComponent<RectTransform>();
            fitter = GetComponent<ContentSizeFitter>();
            pool_leff = ObjectPoolManager.Instance.GetPool("PoolLeffStack");
        }

        private void Update()
        {
            if(prograss<1)
            {
                prograss += Time.deltaTime* animation_speed;
                rectf.sizeDelta = new Vector2(size_tmp.x,size_tmp.y* MathF.Min(1f, prograss));
            }
            else
            {
                prograss = 1;
                rectf.sizeDelta = new Vector2(size_tmp.x, size_tmp.y);
                fitter.enabled = true;
            }
            if(follow_mouse)
            {
                SetPos(Input.mousePosition);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct ItemInfos
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 羁绊栏
        /// </summary>
        public LinkageEffectStack[] linkageEffects;
        /// <summary>
        /// 是否显示更多信息
        /// </summary>
        public bool moreInfos;
        /// <summary>
        /// IN端口径
        /// </summary>
        public int inD;
        /// <summary>
        /// OUT口径
        /// </summary>
        public int outD;
        /// <summary>
        /// 消耗
        /// </summary>
        public float cost;
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
    }
}