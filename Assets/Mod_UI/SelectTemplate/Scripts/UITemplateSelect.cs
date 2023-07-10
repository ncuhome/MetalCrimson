using ER.Items;
using System.Collections.Generic;
using UnityEngine;

namespace UI.SelectTemplate
{
    public class UITemplateSelectPanel:MonoBehaviour
    {
        private List<ItemTemplate> tmps;//一级模板表
        private List<ItemTemplate> tmps2;//二级模板表

        private enum Status { MainTMP,ChildTMP,Make,Closed}
        private Status state;//面板状态

        private void Start()
        {
            tmps = ItemTemplateStore.Instance.FindContainsPart("Tags", " MainModel",';');//获取基础模具库
        }
        #region 界面管理
        public void Open()
        {
            state = Status.MainTMP;
        }
        public void Close() 
        {
            state = Status.Closed;
        }

        private void ToSelectTemplate(ItemTemplate tmp)
        {

        }
        private void ToMake()
        {

        }
        #endregion
    }
}