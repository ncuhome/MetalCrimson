using ER;
using ER.Resource;
using Mod_Resource;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mod_Forge
{
    /// <summary>
    /// 模板预制体
    /// </summary>
    public class PMoldItem : Water, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image img;

        [SerializeField]
        private TMP_Text text;

        private RComponentMold resource;

        public RComponentMold Value
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
                    if (resource != null)
                    {
                        RComponent target = (RComponent)GR.Get(resource.TargetName);
                        if (target != null)
                        {
                            info_panel.UpdateInfo(new ItemInfos()
                            {
                                cost = target.Cost,
                                description = resource.Description,
                                inD = target.In_diameter,
                                outD = target.Out_diameter,
                                linkageEffects = target.Leffs,
                                moreInfos = true,
                                name = resource.DisplayName
                            });
                        }
                    }
                    info_panel.Open();
                    displayed = true;
                }
            }
        }
    }
}