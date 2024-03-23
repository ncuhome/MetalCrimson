// Ignore Spelling: color

using ER;
using ER.UI.Animator;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mod_Common.Message
{
    /// <summary>
    /// 消息管理器
    /// </summary>
    public class MessageManager : MonoBehaviour
    {
        [SerializeField]
        private ObjectPool pool_messageItem;

        [SerializeField]
        private RectTransform Mask;

        private RectTransform self;

        [Tooltip("元素间的间隔")]
        public float interval = 10;

        public float animation_speed = 50;

        private List<MessageItem> items = new List<MessageItem>();//已经取出的项目

        private Queue<MessageItem> waits = new Queue<MessageItem>();//等待队列


        /// <summary>
        /// 创建信息, 信息内容会被缓存在合适的时机发送
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Message message)
        {
            MessageItem item = (MessageItem)pool_messageItem.GetObject();
            item.transform.SetParent(Mask);
            item.transform.localPosition = Vector3.zero;

            item.SetMessage(message.sprite, message.text);
            item.SetImageColor(message.sprite_color);
            item.SetTextColor(message.text_color);

            if (waits.Count == 0) timer = 0.5f;
            waits.Enqueue(item);
        }

        [ContextMenu("发送调试消息")]
        private void _CreateMessage()
        {
            Invoke("test", 0.5f);
        }

        private void test()
        {
            Message ms = Message.Empty;
            int index = (int)(Time.time * 1000) % 10;
            switch (index)
            {
                case 0: ms.text = "芙兰赛高!"; break;
                case 1: ms.text = "233!"; break;
                case 2: ms.text = "测试语句3"; break;
                case 3: ms.text = "失败的Man"; break;
                case 4: ms.text = "MasterSpark!"; break;
                case 5: ms.text = "dddddd"; break;
                case 6: ms.text = ":P"; break;
                case 7: ms.text = "蓝~蓝~路!"; break;
                case 8: ms.text = "我永远喜欢矢田寺成美.jpg"; break;
                case 9: ms.text = "矢田寺成美永远都不喜欢你.jpg"; break;
            }
            AddMessage(ms);
        }

        private bool send_ok = true;//是否启用动画播放(一把锁,防止在播放动画时插入新的动画)
        private float timer;//发送内置计时器, 确保项目已经缓存好大小
        TMP_Text debug;
        /// <summary>
        /// 发送已经缓存的信息
        /// </summary>
        private void SendMessage()
        {
            debug.text = $"waits.count:{waits.Count}, send_ok:{send_ok}, timer:{timer} items.count:{items.Count}";
            if (waits.Count <= 0) return;//没有缓存直接跳过
            if (!send_ok) return;//如果正在发送消息则不执行发送操作
            if (timer > 0) return;
            timer = 0.5f;//重置计时器
            send_ok = false;//上锁
            MessageItem item = waits.Dequeue();//取出消息
            if (items.Count == 0)//如果没有需要等待的项目则直接执行发送
            {
                Debug.Log("直接触发");
                items.Add(item);
                item.Display();
                send_ok = true;//解锁
            }
            else
            {
                int count = items.Count;
                Progress progress = new Progress(count);
                progress.OnDone += () =>
                {
                    //完成前置任务的移动
                    Debug.Log("任务完成触发");
                    items.Add(item);
                    item.Display();
                    send_ok = true;//解锁
                };
                List<MessageItem> remove = new List<MessageItem>();//待移除的对象
                for (int i = 0; i < count; i++)
                {
                    MessageItem it = items[i];
                    if (it.Visible)//如果是有效的则需要执行上升动画
                    {
                        if (InVisibleBox(it))//如果在可视区域
                        {
                            it.Move(Vector2.up, it.Self.sizeDelta.y + interval, progress.AddProgress);
                        }
                        else
                        {
                            remove.Add(it);
                            it.Destroy();
                            progress.AddProgress();
                        }
                    }
                    else//否则移除
                    {
                        remove.Add(it);
                        progress.AddProgress();
                    }
                }
                for(int i=0;i<remove.Count;i++)
                {
                    items.Remove(remove[i]);
                }
            }

        }
        /// <summary>
        /// 判断指定对象是否在显示区域内
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool InVisibleBox(MessageItem it)
        {
            return it.Self.localPosition.y <= self.sizeDelta.y + 100f && it.Self.localPosition.y >= 0;
        }
        private void Awake()
        {
            debug = ((MonoAnchor)AM.GetAnchor("DebugText")).GetComponent<TMP_Text>();
            self = GetComponent<RectTransform>();
            MessageItem.animation_speed = animation_speed;
            this.RegisterAnchor("MessageManager");
        }
        private void Update()
        {
            if (timer > 0) timer -= Time.deltaTime;
            SendMessage();
        }

        /// <summary>
        /// 消息结构
        /// </summary>
        public struct Message
        {
            public Sprite sprite;//消息图片
            public Color sprite_color;//图片色相
            public string text;//消息文字
            public Color text_color;//文字色相

            public static Message Empty
            {
                get => new Message()
                {
                    sprite = null,
                    sprite_color = Color.white,
                    text = string.Empty,
                    text_color = Color.white
                };
            }
        }

        private void OnDestroy()
        {
            AM.DeleteAnchor("MessageManager");
        }
    }
}