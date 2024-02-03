using UnityEngine;

namespace Mod_Level
{
    public class Door : InteractObject
    {
        [Tooltip("障碍物")]
        [SerializeField]
        private GameObject Block;

        private bool opened = false;//开门状态

        /// <summary>
        /// 玩家靠近时显示UI
        /// </summary>
        public override void OnPlayerDisplay()
        {
            if (opened)
            {
                InteractUIPanel.Instance.DisplayUI(this, new UIInfo()
                {
                    position = (Vector2)transform.position + Vector2.up * 3,
                    text = "[F]关门",
                });
            }
            else
            {
                InteractUIPanel.Instance.DisplayUI(this, new UIInfo()
                {
                    position = (Vector2)transform.position + Vector2.up * 3,
                    text = "[F]开门",
                });
            }
        }

        public override void EnterInteract()
        {
            base.EnterInteract();
            opened = !opened;
            if (opened)
            {
                Block.SetActive(false);
                InteractUIPanel.Instance.DisplayUI(this, new UIInfo()
                {
                    position = (Vector2)transform.position + Vector2.up * 3,
                    text = "[F]关门",
                });
            }
            else
            {
                Block.SetActive(true);
                InteractUIPanel.Instance.DisplayUI(this, new UIInfo()
                {
                    position = (Vector2)transform.position + Vector2.up * 3,
                    text = "[F]开门",
                });
            }
        }

        public override void ExitInteract()
        {
            base.ExitInteract();
        }
    }
}