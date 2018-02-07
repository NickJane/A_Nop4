1. 新建mvc项目Nop.Test, 文件夹选择在Presentation/Nop.Web下
2. 写在项目. 并且把Nop.Test文件夹改名为Test.
3. 重新加载Nop.Test, 并修改生成里面的输出路径为..\bin\. 为了编译的时候把dll资源输出到主web项目的bin目录下
4. 删除global.axcx, 增加TestAreaRegistration.cs并且修改里面的相关配置.
5. Nop.Web.Framework里面的nopViewEngine增加相关配置
5. 主项目的RouteConfig 需要在默认的路由设置里面加上namespace, 避免action冲突