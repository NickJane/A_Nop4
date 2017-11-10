
	Static EngineContent 静态类,
		 网站的Global.asax程序的Application_start事件中
		 调用EngineContext.Initialize(false); 单例模式生成全局一个NopEngine对象, 并注入所有的依赖
		 然后调用的时候根据Resolve的生命周期设置获得对象.

		 需要注意的是, webapi不作为独立项目而作为一个area存在的时候, 有点小坑, 于是在Engine里面特殊处理
		 try
        {
            //注册webapi容器
            Assembly webApiAssembly = Assembly.Load("Nop.WebAPI");//直接写死webapi的程序集名称, 不再动态加载
            builder.RegisterApiControllers(webApiAssembly);//注册api容器的实现
        }
        catch 
        {
            //单元测试的时候，加载不到这个dll
        }