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
        private LoadTaskResource task = null;//加载资源任务
        public void OnPointerClick(PointerEventData eventData)
        {
            string address = $"pack:mc:component_mold/{GR.GetAddressAll(resource.RegistryName)}";
            Debug.Log($"正在读取注册表:{address}");
            task = GR.Get<LoadTaskResource>(address);//模具类型注册表
            loading = true;
        }

        public override void ResetState()
        {
            Value = null;
            loading = false;
            task = null;
        }

        protected override void OnHide()
        {

        }

        private void Update()
        {
            if(loading)
            {
                if(task.Value.progress_load.done && task.Value.progress_load_force.done)
                {
                    ComponentMaker maker = (ComponentMaker)AM.GetAnchor("ComponentMaker").Owner;
                    maker.NextPage();
                }
            }
        }


    }
}