namespace ER.Resource
{
    public class TextResource : IResource
    {
        private string registryName;
        private string text;
        public string RegistryName { get => registryName; }
        /// <summary>
        /// Text 资源对象
        /// </summary>
        public string Value => text;

        public static explicit operator string(TextResource source)
        {
            return source.Value;
        }
        public TextResource(string _registryName, string origin)
        {
            text = origin;
            registryName = _registryName;
        }
    }
}