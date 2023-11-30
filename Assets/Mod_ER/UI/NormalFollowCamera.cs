using UnityEngine;

namespace ER.UI
{
    public class NormalFollowCamera : MonoBehaviour
    {
        /// <summary>
        /// 目标坐标
        /// </summary>
        [SerializeField]
        private Transform aim;

        /// <summary>
        /// 是否开启跟随
        /// </summary>
        public bool follow = true;

        /// <summary>
        /// 最大相机速度
        /// </summary>
        public float maxSpeed = 100;

        /// <summary>
        /// 最小相机速度
        /// </summary>
        public float minSpeed = 0.01f;

        /// <summary>
        /// 前进倍率参数，这个值越大，单帧移动的步长越小
        /// </summary>
        public float step = 5;

        private void Awake()
        {
        }

        private void Update()
        {
            if (follow)
            {
                Vector2 pos = aim.position;
                Vector2 delta = pos - (Vector2)transform.position;
                Vector2 move = delta / step;
                /*
                if (move.magnitude < minSpeed * 10) return;
                if(move.magnitude > maxSpeed)
                {
                    move = move.normalized * maxSpeed;
                }
                else if(move.magnitude < minSpeed)
                {
                    move = move.normalized * minSpeed;
                }*/
                transform.position += new Vector3(move.x, move.y, 0);
            }
        }
    }
}