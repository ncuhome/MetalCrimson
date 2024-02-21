using Unity.VisualScripting;

namespace Mod_Item
{
    /// <summary>
    /// 物品标签
    /// </summary>
    public class ItemTag:IResource
    {
        private string registryName;
        public string RegistryName { get => registryName; set => registryName=value; }


    }
}