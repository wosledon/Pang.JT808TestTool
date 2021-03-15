# JTT808协议终端配置/测试工具

基于DotNet 5的WPF程序

无情的缝合机器, 体力活全干了!🤣🤣🤣🤣

##### **需要注意的是:**
因为是基于项目写的工具, 暂时还没改, 如果想使用需要按照以下方式:
1. 由网关转发至Kafka
2. 工具接入的是Kafka (后面会直接集成Socket Server和Socket Client)
      (1) 需要主题`jt808`用于传递设备发来的消息
      (2) 需要主题`sessionOffline`由网关通知模拟器设备下线了(这个不想用的话也行, 直接重启)
3. 上行命令格式: 1Byte+源数据, 1Byte是暂时保留的, 暂时目的是用作协议区分, 可以直接引用项目里的`PMPlatform.RelayProtocols`来格式化上行数据和下行数据.


### 实现的功能
1. 显示设备连接信息
2. 查看设备参数
3. 接收设备消息并提供格式化
4. 设置设备参数
5. 过滤设备消息
6. 一些固定的配置也可以在`App.config`中修改

![image](https://github.com/DonPangPang/Pang.JT808TestTool/blob/master/tool.png)

如果觉得麻烦可以自行下载修改, 或者等我改...🥰

希望能有人生中的第一个star✨✨✨😃
