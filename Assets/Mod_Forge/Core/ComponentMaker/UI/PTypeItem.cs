using UnityEngine;
using Mod_Resource;
using ER;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using ER.Resource;

namespace Mod_Forge
{
    /// <summary>
    /// 模板预制体
    /// </summary>
    public class PTypeItem : Water,IPointerClickHandler
    {
        [SerializeField]
        private Image img;
        [SerializeField]
        private TMP_Text text;
        private RComponentMoldType resource;
        public RComponentMoldType Value
        {
            get => resource;
            set
            {
                resource = value;
                if (resource != null)
                {
                    img.sprite = resource.Sprite.Value;
                    text.text = resource.DisplayName;
                }
                else
                {
                    img.sprite = null;
                    text.text = null;
                }
            }
        }

        private bool loading = false;//是否正在加载资源
        private LoadTaskResource task_1 = null;//加载资源任务
        private LoadTaskResource task_2 = null;//加载资源任务
        public void OnPointerClick(PointerEventData eventData)
        {
            if (loading) return;
            string address_1 = $"pack:mc:compm/{GR.GetAddress(resource.RegistryName)}";
            string address_2 = $"pack:mc:comp/{GR.GetAddress(resource.RegistryName)}";
            Debug.Log($"正在读取注册表:{address_1}");
            task_1 = GR.Get<LoadTaskResource>(address_1);//模具类型注册表
            task_2 = GR.Get<LoadTaskResource>(address_2);//模具注册表
            GR.AddLoadTask(task_1.Value);
            GR.AddLoadTask(task_2.Value);
            loading = true;
        }

        public override void ResetState()
        {
            Value = null;
            loading = false;
            task_1 = null;
            task_2 = null;
        }

        protected override void OnHide()
        {

        }

        private void Update()
        {
            if(loading)
            {
                if(task_1.Value.progress_load.done && task_1.Value.progress_load_force.done && task_2.Value.progress_load.done && task_2.Value.progress_load_force.done)
                {
                    enabled = false;
                    loading = false;
                    Debug.Log("资源加载完毕");
                    ComponentMaker maker = (ComponentMaker)AM.GetAnchor("ComponentMaker").Owner;
                    maker.SelectMoldType(task_1);
                }
            }
        }


    }
}