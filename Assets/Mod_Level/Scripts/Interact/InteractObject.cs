using ER.Entity2D;
using System.Globalization;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 可交互物体
    /// </summary>
    public class InteractObject : MonoBehaviour, IInteractive
    {
        /// <summary>
        /// 是否正在交互
        /// </summary>
        private bool interacting = false;
        public bool Interacting=> interacting;
        private void Awake()
        {
        }
        /// <summary>
        /// 玩家靠近时显示UI
        /// </summary>
        public virtual void OnPlayerDisplay()
        {
            InteractUIPanel.Instance.DisplayUI(this, new UIInfo()
            {
                 position = (Vector2)transform.position,
                  text = "[F]交互",
            });
        }
        public virtual void EnterInteract()
        {
            interacting = true;
        }

        public virtual void ExitInteract()
        {
            interacting = false;
        }

    }
}