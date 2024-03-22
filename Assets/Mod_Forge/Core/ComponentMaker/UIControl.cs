using ER.ItemStorage;
using ER.Resource;
using Mod_Resource;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mod_Forge
{
    public partial class ComponentMaker
    {
        [Tooltip("本页面")]
        [SerializeField]
        private GameObject Self;
        [Tooltip("物品容器")]
        [SerializeField]
        private RectTransform Content;
        /// <summary>
        /// 页数:
        /// 0:选择模具;类型
        /// 1:选择模具
        /// 2:选择材料
        /// </summary>
        [Tooltip("页数")]
        [SerializeField]
        private int page;
        [Tooltip("标题")]
        [SerializeField]
        private TMP_Text txt_title;

        [Header("按钮")]
        public Button bt_close;
        public Button bt_return;



        /// <summary>
        /// 关闭页面
        /// </summary>
        public void Close()
        {

        }
        /// <summary>
        /// 打开页面
        /// </summary>
        public void Open()
        {
            page = 0;

            UpdateUI();

        }

        /// <summary>
        /// 返回上一页面
        /// </summary>
        public void Return()
        {
            if (page > 0)
            {
                page--;
                switch (page)
                {
                    case 0:
                        UpdateToTypePage();
                        break;
                    case 1:
                        UpdateToMoldPage();
                        break;
                }
            }
        }
        /// <summary>
        /// 下一个页面
        /// </summary>
        public void NextPage()
        {
            if (page < 3)
            {
                page++;
                switch (page)
                {
                    case 1:
                        UpdateToMoldPage();
                        break;
                    case 2:
                        UpdateToMakerPage();
                        break;
                }
            }
        }

        /// <summary>
        /// 更新至模具类型选择页面
        /// </summary>
        public void UpdateToTypePage()
        {
            ClearGroup();
            RComponentMoldType[] types = GR.GetAll<RComponentMoldType>("cmt");
            Debug.Log($"跳转类型选择:{types.Length}");
            for (int i = 0; i < types.Length; i++)
            {
                PTypeItem item = (PTypeItem)pool_PTypeItem.GetObject();
                item.Value = types[i];
                GetLastUGroup().Add(item);
            }
        }
        /// <summary>
        /// 跳转更新至模具选择页面
        /// </summary>
        public void UpdateToMoldPage()
        {
            ClearGroup();
            string[] loads = selected.Value.load;
            Debug.Log($"跳转模具选择:{loads.Length}");
            for (int i = 0; i < loads.Length; i++)
            {
                PMoldItem item = (PMoldItem)pool_PMoldItem.GetObject();
                item.Value = GR.Get<RComponentMold>(loads[i]);
                GetLastUGroup().Add(item);
            }

        }
        /// <summary>
        /// 跳转模具加工页面
        /// </summary>
        public void UpdateToMakerPage()
        {
            ClearGroup();
            ItemContainer materials = Forge.Instance.Materials;
            for (int i = 0; i < materials.StackCount; i++)
            {
                PMaterialItem item = (PMaterialItem)pool_PMaterial.GetObject();
                item.Value = (ItemStack)materials.GetStack(i);
                materialList.Add(item);
            }
        }
        /// <summary>
        /// 更新UI
        /// </summary>
        private void UpdateUI()
        {
            Dictionary<string, string> dic = lang.Value["page"];
            txt_title.text = dic[$"title_{page}"];
            switch (page)
            {
                case 0:
                    bt_return.gameObject.SetActive(false);
                    break;
                case 1:
                    bt_return.gameObject.SetActive(true);
                    break;
                case 2:
                    bt_return.gameObject.SetActive(true);
                    break;
            }
        }
        /// <summary>
        /// UI初始化
        /// </summary>
        private void UIInit()
        {
            bt_close?.onClick.AddListener(Close);
            bt_return.onClick.AddListener(Return);

            Dictionary<string, string> screen_settings = (Dictionary<string, string>)GameSettings.Instance.GetSettings("screen");
            string resl = screen_settings["resolution"];//分辨率;
            lay_max = GameConst.SELECT_MOLD_LAYOUT_COUNT_2K;
            if (resl == "K4")
            {
                lay_max = GameConst.SELECT_MOLD_LAYOUT_COUNT_4K;
            }
        }
    }
}