using UnityEngine;

namespace ER.UI
{
    /// <summary>
    /// 玩家跟随镜头
    /// </summary>
    public class RigidbodyFollowCamera : MonoBehaviour
    {
        #region 属性

        /// <summary>
        /// 跟随对象
        /// </summary>
        public Rigidbody2D aim;

        public Rigidbody2D self;
        public bool Xlock = false;//X轴锁定
        public bool Ylock = false;//Y轴锁定
        public Vector2 limitMin;//限制（最小）
        public Vector2 limitMax;//限制（最大）
        public float speedMax = 50;//镜头最大移动速度
        public bool transition = true;//是否启用过渡

        public float height = 30;//镜头高度
        public float speed = 0;//当前镜头速度
        public bool lockMaxSpeed = true;//锁定最大镜头移动速度
        public float maxSpeed = 60;//最大镜头速度

        public bool follow = true;
        public Vector2 aimPosition = Vector2.zero;

        public float Rate = 3;//镜头移动倍率

        private bool shaking = false;//是否是震动状态
        public float power = 1;//震动力度
        public float timer = 0;//效果持续时间
        public int status = 0;//震动状态标记

        #endregion 属性

        #region 功能函数

        /// <summary>
        /// 一般镜头震动
        /// </summary>
        public void ShakeNormal()
        {
            shaking = true;
            timer = 1;
            status = 0;
        }

        /// <summary>
        /// 镜头震动
        /// </summary>
        /// <param name="power">震动力度</param>
        /// <param name="time">震动时间</param>
        public void Shake(float _power, float _time)
        {
            shaking = true;
            power = _power;
            timer = _time;
            status = 0;
        }

        #endregion 功能函数

        #region 内部函数

        /// <summary>
        /// 获取随机二维向量
        /// </summary>
        /// <param name="ef">模长调整</param>
        /// <returns></returns>
        private Vector2 RandomVector2(float ef)
        {
            return new Vector2(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f) * ef;
        }

        private void Follow()
        {
            if (aim != null)
            {
                if (follow)
                {
                    aimPosition = aim.position;
                }
            }
            if (Xlock)
            {
                if (aimPosition.x < limitMin.x)
                {
                    aimPosition.x = limitMin.x;
                }
                else if (aimPosition.x > limitMax.x)
                {
                    aimPosition.x = limitMax.x;
                }
            }
            if (Ylock)
            {
                if (aimPosition.y < limitMin.y)
                {
                    aimPosition.y = limitMin.y;
                }
                else if (aimPosition.y > limitMax.y)
                {
                    aimPosition.y = limitMax.y;
                }
            }

            Vector2 move = aimPosition - (Vector2)transform.position;
            speed = move.magnitude * Rate;

            //Debug.Log("相机跟随中");
            if (transition)
            {
                if (lockMaxSpeed && speed > maxSpeed) { speed = maxSpeed; }
                self.velocity = move.normalized * speed;
            }
            else
            {
                transform.position = aim.transform.position + Vector3.back * height;
            }
        }

        #endregion 内部函数

        #region Unity

        private void Update()
        {
            if (shaking)
            {
                switch (status)
                {
                    case 0:
                        transform.position += (Vector3)RandomVector2(1) * power;
                        status = 1;
                        break;

                    case 1:
                        transform.position += (Vector3)RandomVector2(0.5f) * power;
                        status = 2;
                        break;

                    case 2:
                        transform.position += (Vector3)RandomVector2(0.3f) * power;
                        status = 3;
                        break;

                    case 3:
                        transform.position += (Vector3)RandomVector2(0.8f) * power;
                        status = 0;
                        break;
                }
                timer -= Time.unscaledDeltaTime;
                if (timer < 0) { shaking = false; }
            }
            if (aim != null)
            {
                Follow();
            }
        }

        #endregion Unity
    }
}