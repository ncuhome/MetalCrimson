namespace Common
{
    public static class AnchorExpand
    {
        /// <summary>
        /// 为此对象创建一个虚拟访问锚点
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="anchorName">锚点名称</param>
        public static void RegisterAnchor(this object obj,string anchorName)
        {
            VirtAnchor anchor = new VirtAnchor(anchorName);
            anchor.SetOwner(obj);
            AnchorManager.Instance.AddAnchor(anchor);
        }
    }
}