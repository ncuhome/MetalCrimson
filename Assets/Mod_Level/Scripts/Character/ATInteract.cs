using ER.Entity2D;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 与可互动物体发动互动(玩家版)
    /// 依赖: ATRegion
    /// </summary>
    public class ATInteract : MonoAttribute
    {
        public ATRegion region;
        private List<InteractObject> list = new List<InteractObject>();
        private int index = 0;//选择交互对象索引

        /// <summary>
        /// 当前选中的交互对象
        /// </summary>
        public InteractObject Selected
        {
            get
            {
                if (index < 0 || index >= list.Count)
                {
                    index = 0;
                    return null;
                }
                else
                {
                    return list[index];
                }
            }
        }

        public ATInteract()
        { AttributeName = nameof(ATInteract); }

        public override void Initialize()
        {
            region.EnterEvent += AddList;
            region.ExitEvent += RemoveList;
        }

        private void AddList(Collider2D c)
        {
            Debug.Log("添加新的交互物体");
            InteractObject obj = c.GetComponent<InteractObject>();
            if (obj != null)
            {
                list.Add(obj);
                Arange();
            }
        }

        private void RemoveList(Collider2D c)
        {
            Debug.Log("移除新的交互物体");
            InteractObject obj = c.GetComponent<InteractObject>();
            if (obj != null)
            {
                if (list.Contains(obj))
                {
                    list.Remove(obj);
                    Arange();
                }
                if(obj == InteractUIPanel.Instance.nowInteractObject)//如果当前交互正在显示提示UI, 则关闭
                {
                    InteractUIPanel.Instance.DisplayUI(null, null);
                }
            }
        }

        /// <summary>
        /// 按距离从近到远排序
        /// </summary>
        private void Arange()
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int k = i + 1; k < list.Count; k++)
                {
                    float dis_1 = (list[i].transform.position - transform.position).magnitude;
                    float dis_2 = (list[k].transform.position - transform.position).magnitude;
                    if (dis_2 < dis_1)
                    {
                        var tmp = list[i];
                        list[i] = list[k];
                        list[k] = tmp;
                    }
                }
            }
            if(Selected!=null)
            {
                Selected.OnPlayerDisplay();
            }
        }

        /// <summary>
        /// 切换下一个交换对象
        /// </summary>
        public void NextInteract()
        {
            index++;
            if (index >= list.Count)
            {
                index = 0;
            }
        }
    }
}