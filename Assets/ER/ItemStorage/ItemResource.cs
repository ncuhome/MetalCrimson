using ER.Resource;

namespace ER.ItemStorage
{
    public class ItemResource : IItemResource
    {
        private string registryName;
        private DescriptionInfo[] descriptions;
        private string displayName;
        private bool stackable;
        private int amountMax;

        public string RegistryName => registryName;

        public DescriptionInfo[] Descriptions => descriptions;

        public string DisplayName => displayName;

        public bool Stackable => stackable;

        public int AmountMax => amountMax;
    }
}