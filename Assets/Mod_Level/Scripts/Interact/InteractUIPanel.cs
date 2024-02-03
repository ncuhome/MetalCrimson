using ER;
using System.Transactions;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace Mod_Level
{
    /// <summary>
    /// 交互UI显示管理面板
    /// </summary>
    public class InteractUIPanel:MonoSingleton<InteractUIPanel>
    {
        [Tooltip("UI字体高度")]
        public float front_height = 5f;
        public InteractObject nowInteractObject = null;
        [SerializeField]
        private TMP_Text text;
        public void DisplayUI(InteractObject obj, UIInfo info)
        {
            if (obj == null)
            {
                nowInteractObject = null;
                if(text.gameObject.activeSelf)
                    text.gameObject.SetActive(false);
            }
            else
            {
                nowInteractObject = obj;
                if(info==null)
                {
                    Debug.Log("无交互UI消息");
                    return;
                }
                text.text = info.text;
                text.gameObject.SetActive(true);
                text.transform.position = new Vector3(info.position.x, info.position.y, -front_height);
            }

        }
    }

}