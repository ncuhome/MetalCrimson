using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 传送门
    /// </summary>
    public class Portal:MonoBehaviour
    {
        [SerializeField]
        [Tooltip("检测距离")]
        private float length = 10;
        /// <summary>
        /// 传送目的地
        /// </summary>
        [SerializeField]
        private Transform aimPosition;
        [SerializeField]
        private int aimLayerIndex = 7;

        private void Update()
        {
            Debug.DrawRay(transform.position, Vector2.up*length);
            //注意，使用射线遮罩时，目标图层的索引需要转化成二进制数才能有效
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, length,1<<aimLayerIndex);
            if(hit.collider != null)
            {
                Debug.Log("射线检测：" + hit.collider.name);
                if (hit.collider.tag == "Player")
                {
                    Debug.Log("检测到玩家在门旁");
                    ATCharacterState state = hit.collider.GetComponent<Entity>().GetAttribute<ATCharacterState>();
                    Debug.Log("state is null:");
                    if(state!=null)
                    {
                        Debug.Log($"交互状态：{state.interact}");
                        if(state.interact == ATCharacterState.InteractState.Wait)
                        {
                            Debug.Log("传送！");
                            hit.collider.transform.position = aimPosition.position;
                        }
                    }
                }
            }
        }
    }
}