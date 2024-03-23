using ER;
using ER.ItemStorage;
using ER.Resource;
using Mod_Resource;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Forge
{

    public partial class ComponentMaker : MonoBehaviour
    {
        public static int lay_max;//一列元素最大数量

        private ObjectPool pool_PMoldItem;//对象池-模具预制体
        private ObjectPool pool_PTypeItem;//对象池-模具类型预制体
        private ObjectPool pool_PGroup;//对象池-元素分组预制体
        private ObjectPool pool_PMaterial;//对象池-材料预制体

        private List<PGroup> groups = new List<PGroup>();//使用中的 分组对象
        private List<PMaterialItem> materialList = new List<PMaterialItem>();//使用中的 材料对象

        private LanguageResource lang;//相关文本资源

        private LoadTaskResource selected;//用于缓存记录玩家选择了哪一种模具类型, 以及该类型下的所有模具
        private RComponentMold selectedMold;//用于缓存记录玩家选择了哪一种模具

        /// <summary>
        /// 选择指定模具类型
        /// </summary>
        /// <param name="s"></param>
        public void SelectMoldType(LoadTaskResource s)
        {
            selected = s;
            UpdateToMoldPage();
        }
        /// <summary>
        /// 选择指定模具
        /// </summary>
        public void SelectMold(RComponentMold mold)
        {
            selectedMold = mold;
            UpdateToMakerPage();

        }


        #region 私有方法

        /// <summary>
        /// 获取最末尾的group, 如果没有则创建
        /// </summary>
        /// <returns></returns>
        private PGroup GetLastGroup()
        {
            if (groups.Count == 0)
            {
                PGroup g = (PGroup)pool_PGroup.GetObject();
                groups.Add(g);
                return g;
            }
            return groups[groups.Count - 1];
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
                g.transform.SetParent(Content);
                return g;
            }
            PGroup lg = groups[groups.Count - 1];
            if (lg.IsFull())
            {
                lg = (PGroup)pool_PGroup.GetObject();
                lg.transform.SetParent(Content);
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
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].Destroy();
            }
            groups.Clear();
        }
        #endregion


        private LoadTask task_1;//缓存加载任务
        private LoadTask task_2;//缓存加载任务
        private int init = 0;//初始化阶段
        private bool inited = true;//是否完成初始化
        private void Awake()
        {
            string lang_path = "lang:mc:ui/forge/component_maker";
            this.RegisterAnchor("ComponentMaker");

            _LoadTask1();

            GR.Load(()=>
            {
                lang = GR.Get<LanguageResource>(lang_path);
                UpdateUI();
            }, true,lang_path);

            UIInit();
        }

        private void _LoadTask1()
        {
            task_1 = GR.Get<LoadTaskResource>("pack:mc:all/cmt_demand").Value;//模具类型注册表
            task_2 = GR.Get<LoadTaskResource>("pack:mc:all/compm_demand").Value;//模具类型注册表
            GR.AddLoadTask(task_1);
            GR.AddLoadTask(task_2);
            init = 1;
        }
        private void _LoadTask2()
        {
            task_1 = GR.Get<LoadTaskResource>("pack:mc:all/cmt").Value;//模具类型注册表
            task_2 = GR.Get<LoadTaskResource>("pack:mc:all/compm").Value;//模具类型注册表
            GR.AddLoadTask(task_1);
            GR.AddLoadTask(task_2);
            init = 2;
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
            switch (init)
            {
                case 1:
                    if(task_1.progress_load.done && task_2.progress_load.done)
                    {
                        Debug.Log("加载模具前置资源完毕");
                        _LoadTask2();
                    }
                    break;
                    
                case 2:
                    if (task_1.progress_load.done && task_2.progress_load.done)
                    {
                        Debug.Log("加载模具资源完毕");
                        page = 0;
                        UpdateToTypePage();
                        init = -1;
                    }
                    break;
            }
        }

        private void OnDestroy()
        {
            AM.DeleteAnchor("ComponentMaker");
        }
    }

}