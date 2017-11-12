准备材料有
Isettings
	globalSetting.cs 
	App_Data/globalsetting.xml文件

ISettingService
	SettingService 对材料的加工, 使用了很多nopcommerce的内置支持方法


DependencyRegistrar中
	    public class SettingsSource : IRegistrationSource
		{
			static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
				"BuildRegistration",
				BindingFlags.Static | BindingFlags.NonPublic);

			public IEnumerable<IComponentRegistration> RegistrationsFor(
					Service service,
					Func<Service, IEnumerable<IComponentRegistration>> registrations)
			{
				var ts = service as TypedService;
				if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
				{
					var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
					yield return (IComponentRegistration)buildMethod.Invoke(null, null);
				}
			}
			static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
			{
				return RegistrationBuilder
					.ForDelegate((c, p) =>
					{
						//uncomment the code below if you want load settings per store only when you have two stores installed.
						//var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
						//    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

						//although it's better to connect to your database and execute the following SQL:
						//DELETE FROM [Setting] WHERE [StoreId] > 0
						return c.Resolve<ISettingService>().LoadSetting<TSettings>();
					})
					.InstancePerLifetimeScope()
					.CreateRegistration();
			}

			public bool IsAdapterForIndividualComponents { get { return false; } }
		}
	

	//使用globalsetting配置文件
            builder.RegisterType<SettingService>().As<ISettingService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_per_request"))
                .InstancePerLifetimeScope();
            builder.RegisterSource(new SettingsSource());