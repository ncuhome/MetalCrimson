# 资源系统
## 功能
- 支持资源覆盖(外部资源包添加)
- 使用注册名(registryName)对资源进行管理
## 注意事项
1. 资源包管理器读写依赖于 Unity 第三方插件: Addressable
2. 本包提供三种基础资源 sprite, text, audioClip, 如果需要后续添加新的资源类型, 则新类型需要实现 IResource 接口
3. 每种资源都有对应的 资源加载器, 这些资源加载器 实现于 IResourceLoader, 新添加资源类型, 也需要添加配套的 资源加载器
4. 每种资源都有唯一的资源头用于标识不同的资源类型, 例如

    - 文本资源:txt
	- 精灵图资源: img
	- 音频片段资源: wav
5. 资源注册名格式: <资源头>:<模组名>:<地址> , 例如:

    - item:mc:air
	- txt:erinbone:ui_text

	需要注意的是: 资源名必须只能有 小写字母+数字+"_"+"/" 组成
6. 本地资源有一定的存放要求, 其资源地址应当由 资源头 和 地址组成

    例如: 

	- "item:mc:air" 资源, 其对应资源文件的 Addressable 引用地址应当为 res/item/air
	- "img:mc:water" 资源, 其对应资源文件的 Addressable 引用地址应当为 res/img/water

	
## 示例部分

这里还支持批量加载或卸载资源. 在进入一些特殊情景时, 可以读取预设文件(.json), 其中记录资源缓存变更信息, 然后根据该文件信息处理资源加载
```CSharp
//假设 json 就是预设文件的内容(json格式)
//这里将它转换为 LoadTaskInfo
LoadTaskInfo info = JsonConvert.DeserializeObject<LoadTaskInfo>(json);
//直接将 LoadTaskInfo 交给 GameResource 加载, 其中 LoadTaskInfo 中的 progress_load 和 progress_load_force 会实时反应加载进度
GR.AddLoadTask(new LoadTask(info));
```
	
	

	