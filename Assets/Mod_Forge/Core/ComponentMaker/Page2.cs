using ER.ItemStorage;
using ER.Resource;
using Mod_Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mod_Forge
{
    public partial class ComponentMaker
    {
        private bool acceptMaterialInput;//接受材料输入, 用于控制 page-2 中材料拖拽输入的开关
        private ItemStack selectedMaterial;//当前被拖拽的 材料 对象
        private List<ItemStack> storing = new List<ItemStack>();

        public bool AcceptMaterialInput { get => acceptMaterialInput; set => acceptMaterialInput = value; }
        public ItemStack SelectedMaterial { get => selectedMaterial; set => selectedMaterial = value; }


        /// <summary>
        /// 执行材料输入
        /// </summary>
        public void InputMaterial()
        {
            if (acceptMaterialInput)
            {
                storing.Add(SelectedMaterial);
            }
        }

        /// <summary>
        /// 清空输入材料暂存
        /// </summary>
        public void ClearMaterialStoring()
        {
            for(int i=0;i<storing.Count;i++)
            {
                ItemContainer container = Forge.Instance.Materials;
            }
            storing.Clear();
        }

        public void CreateComponent()
        {

        }
    }
}
