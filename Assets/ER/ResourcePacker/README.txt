[资源包管理器]
# 1:资源包管理器读写依赖于 Unity 第三方插件: Addressable
# 2:初始化配置需要:
	*config.ResourceIndexer  (ini)(记录默认材质路径)
	*默认的材质包
	*StreamingAssets/config/res_indexer (自定材质配置文件)(这个可选, 如果缺少, 在运行时会自动补全, 但是 StreamingAssets/config 必须存在)
	