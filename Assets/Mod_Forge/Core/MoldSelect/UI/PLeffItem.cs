// Ignore Spelling: Leff

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
    /// 羁绊堆标签预制体
    /// </summary>
    public class PLeffItem : Water, IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField]
        private Image img;
        [SerializeField]
        private TMP_Text text;
        private LinkageEffectStack resource;
        public LinkageEffectStack Value
        {
            get => resource;
            set
            {
                resource = value;
                RLinkageEffect leff = GR.Get<RLinkageEffect>(resource.registryName);
                if (leff != null)
                {
                    img.sprite = leff.Sprite.Value;
                    text.text = $"{leff.DisplayName} {resource.level}";
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
            img.sprite = null;
            text.text = null;
        }

        protected override void OnHide()
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("鼠标进入");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("鼠标离开");
        }
    }
}