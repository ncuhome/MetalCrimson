using UnityEngine;
using Mod_Resource;
using ER;
using UnityEngine.UI;
using TMPro;
using ER.ItemStorage;
using UnityEngine.EventSystems;
using ER.Resource;
using Unity.VisualScripting;

namespace Mod_Forge
{
    /// <summary>
    /// 材料预制体
    /// </summary>
    public class PMaterialItem : Water, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image img;
        [SerializeField]
        private TMP_Text text;
        private ItemStack resource;
        public ItemStack Value
        {
            get => resource;
            set
            {
                resource = value;
                if (resource != null)
                {
                    RMaterial m =(RMaterial)resource.Resource;
                    img.sprite = m.Sprite.Value;
                    text.text = resource.DisplayName;
                }
                else
                {
                    img.sprite = null;
                    text.text = null;
                }
            }
        }

        public override void ResetState()
        {
            Value = null;
        }

        protected override void OnHide()
        {

        }

        private float timer = 0.5f;
        private bool enter = false;
        private InfoPanel info_panel;
        private bool displayed = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("鼠标进入");
            enter = true;
            displayed = false;
            timer = 0.5f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("鼠标离开");
            enter = false;
            info_panel.Close();
        }

        private void Start()
        {
            info_panel = (InfoPanel)AM.GetAnchor("InfoPanel").Owner;
        }

        private void Update()
        {
            if (enter)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    if (displayed) return;
                    RMaterial material =(RMaterial)resource.Resource;
                    info_panel.UpdateInfo(new ItemInfos()
                    {
                        linkageEffects = material.Leffs,
                        moreInfos = false,
                        name = resource.DisplayName
                    });
                    info_panel.Open();
                    displayed = true;
                }
            }
        }
    }
}