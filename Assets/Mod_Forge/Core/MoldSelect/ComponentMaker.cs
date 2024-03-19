using ER;
using ER.ItemStorage;
using ER.Resource;
using Mod_Forge;
using Mod_Resource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

namespace Mod_Forge
{

    public class ComponentMaker : MonoBehaviour
    {
        [Tooltip("本页面")]
        [SerializeField]
        private GameObject Self;
        [Tooltip("物品容器")]
        [SerializeField]
        private GameObject Content;
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
             
        private ObjectPool pool_PMoldItem;//对象池-模具预制体
        private ObjectPool pool_PTypeItem;//对象池-模具类型预制体
        private ObjectPool pool_PGroup;//对象池-元素分组预制体
        private ObjectPool pool_PMaterial;//对象池-材料预制体

        private List<PGroup> groups = new List<PGroup>();
        private List<PMaterialItem> materialList = new List<PMaterialItem>();

        [Header("按钮")]
        public Button bt_close;
        public Button bt_return;


        private LoadTask type_all;
        private LoadTask mold_all;

        public static int lay_max;//一列元素最大数量

        private bool inited = false;//是否完成初始化
        private LanguageResource lang;//相关文本资源

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
            if(page>0)
            {
                page--;
                UpdatePage();
            }
        }
        /// <summary>
        /// 下一个页面
        /// </summary>
        public void NextPage()
        {
            if(page<3)
            {
                page++;
                UpdatePage();
            }
        }

        /// <summary>
        /// 获取最末尾的group, 如果没有则创建
        /// </summary>
        /// <returns></returns>
        private PGroup GetLastGroup()
        {
            if(groups.Count== 0)
            {
                PGroup g = (PGroup)pool_PGroup.GetObject();
                groups.Add(g);
                return g;
            }
            return groups[groups.Count-1];
        }
        /// <summary>
        /// 获取末尾可用的group, 如果没有则创建
        /// </summary>
        /// <returns></returns>
        private PGroup GetLastUGroup()
        {
            if (groups.Count == 0)
            {
                PGroup g = (PGroup)pool_PGroup.GetObject();
                groups.Add(g);
                return g;
            }
            PGroup lg = groups[groups.Count-1];
            if(lg.IsFull())
            {
                lg = (PGroup)pool_PGroup.GetObject();
                groups.Add(lg);
                return lg;
            }
            return lg;
        }
        /// <summary>
        /// 清空所有元素组
        /// </summary>
        private void ClearGroup()
        {
            for(int i =0;i< groups.Count;i++)
            {
                groups[i].Destroy();
            }
            groups.Clear();
        }

        /// <summary>
        /// 跳转更新至模具选择页面
        /// </summary>
        public void UpdateToMoldPage(LoadTaskResource pack)
        {
            ClearGroup();
            string[] loads = pack.Value.load;
            for (int i=0;i< loads.Length;i++)
            {
                PMoldItem item = (PMoldItem)pool_PMoldItem.GetObject();
                item.Value = GR.Get<RComponentMold>(loads[i]);
                GetLastUGroup().Add(item);
            }
            
        }

        /// <summary>
        /// 更新页面内容
        /// </summary>
        public void UpdatePage()
        {
            switch (page)
            {
                case 0:
                    ClearGroup();
                    RComponentMoldType[] types = GR.GetAll<RComponentMoldType>("cmt");
                    for (int i = 0; i < types.Length; i++)
                    {
                        PTypeItem item = (PTypeItem)pool_PTypeItem.GetObject();
                        item.Value = types[i];
                        GetLastUGroup().Add(item);
                    }
                    break;
                case 1:
                    
                    break;
                case 2:
                    ClearGroup();
                    ItemContainer materials = Forge.Instance.Materials;
                    for(int i=0;i< materials.StackCount; i++)
                    {
                        PMaterialItem item = (PMaterialItem)pool_PMaterial.GetObject();
                        item.Value = (ItemStack)materials.GetStack(i);
                        materialList.Add(item);
                    }
                    break;
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

        private void Awake()
        {
            this.RegisterAnchor("ComponentMaker");

            bt_close?.onClick.AddListener(Close);
            bt_return.onClick.AddListener(Return);

            
            LoadTaskResource task_1 = GR.Get<LoadTaskResource>("pack:mc:cmt_all");//模具类型注册表
            LoadTaskResource task_2 = GR.Get<LoadTaskResource>("pack:mc:compm_all");//模具类型注册表
            type_all = task_1.Value;
            mold_all = task_2.Value;
            GR.AddLoadTask(type_all);
            GR.AddLoadTask(mold_all);

            GR.Load(()=>
            {
                lang = GR.Get<LanguageResource>("lang:mc:ui/forge/component_maker");
                UpdateUI();
            }, true,"lang:mc:ui/forge/component_maker");

            Dictionary<string, string> screen_settings = (Dictionary<string, string>)GameSettings.Instance.GetSettings("screen");
            string resl = screen_settings["resolution"];//分辨率;
            lay_max = GameConst.SELECT_MOLD_LAYOUT_COUNT_2K;
            if (resl == "K4")
            {
                lay_max = GameConst.SELECT_MOLD_LAYOUT_COUNT_4K;
            }
            inited = false;
        }

        private void Start()
        {
            pool_PMoldItem = ObjectPoolManager.Instance.GetPool("PoolMold");
            pool_PTypeItem = ObjectPoolManager.Instance.GetPool("PoolMoldType");
            pool_PGroup = ObjectPoolManager.Instance.GetPool("PoolGroup");
            pool_PMaterial = ObjectPoolManager.Instance.GetPool("PoolMaterial");
        }

        private void Update()
        {
            if(!inited)
            {
                inited = type_all.progress_load.done && mold_all.progress_load.done;
                if(inited)
                {
                    Debug.Log("加载资源完成");
                    page = 0;
                    UpdatePage();
                }
            }
        }

        private void OnDestroy()
        {
            AM.DeleteAnchor("ComponentMaker");
        }
    }

}