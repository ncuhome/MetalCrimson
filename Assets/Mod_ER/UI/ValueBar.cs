using System;
using System.Collections;
using UnityEngine;

namespace ER.UI
{
    public class ValueBar : MonoBehaviour
    {
        #region 组件

        public RectTransform valueBar;//实际值条
        public RectTransform lastValueBar;//变化值条

        #endregion 组件

        private enum status
        { wait, change }

        #region 内部字段

        /// <summary>
        /// 真实值
        /// </summary>
        private float value = 1;

        /// <summary>
        /// 停留值
        /// </summary>
        private float lastValue = 1;

        /// <summary>
        /// 目标停留值
        /// </summary>
        private float aimValue = 1;

        private status st_last = status.wait;
        private status st_value = status.wait;
        private status st_shake = status.wait;
        private float timer_last;
        private float timer_shake = 0;
        private float t_value = 1;//差值计算
        private float t_last = 1;//差值计算
        private Vector2 old_position;//UI旧位置

        #endregion 内部字段

        #region 公开字段|属性

        /// <summary>
        /// 停留值显示时间
        /// </summary>
        public float maxLastTime = 1;

        /// <summary>
        /// 停留值动画速率
        /// </summary>
        public float rate_last = 3;

        /// <summary>
        /// 值动画速率
        /// </summary>
        public float rate_value = 0.5f;

        /// <summary>
        /// 值条动画速度最小值
        /// </summary>
        public float speed_min_value = 0.1f;

        /// <summary>
        /// 值条动画速度最大值
        /// </summary>
        public float speed_max_value = 0.8f;

        /// <summary>
        /// 滞留条动画速度最小值
        /// </summary>
        public float speed_min_last = 1f;

        /// <summary>
        /// 滞留条动画速度最大值
        /// </summary>
        public float speed_max_last = 2f;

        /// <summary>
        /// 震动力度
        /// </summary>
        public float shake_power = 5;

        /// <summary>
        /// 震动时间
        /// </summary>
        public float shake_time = 0.4f;

        /// <summary>
        /// 低于这个值时，才会触发震动动画
        /// </summary>
        public float limit_shake_min = 0.25f;

        /// <summary>
        /// 值的目标值（真实值）
        /// </summary>
        public float Value
        {
            get => aimValue;
            set
            {
                float temp = aimValue;
                aimValue = Mathf.Clamp(value, 0f, 1f);
                if (shake_power > 0 && aimValue < temp && aimValue < limit_shake_min)
                {
                    StartShakeAnimation();
                }
                timer_last = maxLastTime;
                StartValueAnimation();
            }
        }

        private float AValue
        {
            get => value;
            set
            {
                this.value = value;
                valueBar.anchorMax = new Vector2(value, 1);
            }
        }

        private float LastValue
        {
            get => lastValue;
            set
            {
                Debug.Log("变化值 发生改变");
                lastValue = value;
                lastValueBar.anchorMax = new Vector2(value, 1);
            }
        }

        #endregion 公开字段|属性

        #region 内部属性

        private void StartLastAnimation()
        {
            if (st_last != status.change)
            {
                st_last = status.change;
                t_last = 0;
                StartCoroutine(LastAnimation());
            }
        }

        private void StartValueAnimation()
        {
            if (st_value != status.change)
            {
                t_value = 0;
                st_value = status.change;
                StartCoroutine(ValueAnimation());
            }
        }

        private void StartShakeAnimation()
        {
            if (st_shake != status.change)
            {
                st_shake = status.change;
                timer_shake = shake_time;
                old_position = transform.position;
                StartCoroutine(ShakeAnimation());
            }
        }

        private IEnumerator ValueAnimation()
        {
            while (st_value == status.change)
            {
                //Debug.Log($"当前值:{AValue},目标值:{aimValue},差值:{t_value}");
                t_value += Time.deltaTime * rate_value;
                float aim = Mathf.Lerp(AValue, aimValue, t_value);
                float delta = Mathf.Clamp(Mathf.Abs(aim - AValue), speed_min_value * Time.deltaTime, speed_max_value * Time.deltaTime);
                if (aim < AValue) { AValue -= delta; }
                else { AValue += delta; }

                if (LastValue < AValue)
                {
                    LastValue = AValue;
                    timer_last = maxLastTime;
                }
                if (Mathf.Abs(AValue - aimValue) < 0.001) { st_value = status.wait; }
                yield return null;
            }
        }

        private IEnumerator LastAnimation()
        {
            while (st_last == status.change)
            {
                t_last += Time.deltaTime * rate_last;
                float aim = Mathf.Lerp(lastValue, aimValue, t_last);
                float delta = Math.Clamp(Mathf.Abs(aim - lastValue), speed_min_last * Time.deltaTime, speed_max_last * Time.deltaTime);
                if (aim < lastValue) { LastValue -= delta; }
                else { LastValue += delta; }

                if (Mathf.Abs(lastValue - aimValue) < 0.001) { st_last = status.wait; }
                yield return null;
            }
        }

        /// <summary>
        /// 震动效果
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShakeAnimation()
        {
            Debug.Log("震动");
            while (timer_shake > 0)
            {
                timer_shake -= Time.deltaTime;
                transform.position = old_position + RandomVector2(shake_power);
                yield return null;
            }
            transform.position = old_position;
            st_shake = status.wait;
        }

        /// <summary>
        /// 获取随机二维向量
        /// </summary>
        /// <param name="ef">模长调整</param>
        /// <returns></returns>
        private Vector2 RandomVector2(float ef)
        {
            return new Vector2(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f) * ef;
        }

        #endregion 内部属性

        private void Update()
        {
            if (st_last == status.wait)
            {
                if (timer_last > 0)
                {
                    timer_last -= Time.deltaTime;
                }
                else
                {
                    if (aimValue != lastValue)
                    {
                        StartLastAnimation();
                    }
                }
            }
        }
    }
}