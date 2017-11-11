
1. Package
	EntityFramework;	

2. 仓储模式
	接口和实现: 
		IDbContext	提供访问数据库的对象
			NopObjectContext:System.Data.Entity.DbContext,IDbContext
				NopObjectContext(string connectString):base(connectString){ 在构造函数中初始化entity对象}
			提供出两个基础接口, 数据库对象和数据表对象
			new Database Database { get { return base.Database; } }
			public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
			{
				return base.Set<TEntity>();
			}
			并用上述两个对象扩展出使用sql语句的方法.
			重写下面这个方法设置entityframework的工作option
			protected override void OnModelCreating(DbModelBuilder modelBuilder) 

		IRepository<T> where T : class	提供给外层访问数据的接口, 解耦外层和数据访问层
			EfRepository<T> : IRepository<T> where T : class, 
				依赖于IDbContext对象, 通过autofac在构造函数中注入IdbContext的实现
				来完成IRepository接口中定义的方法
			注意学习T的使用

3. unitOfWork工作单元实现事务
	事务的使用
	UnitOfWorkFactory.CurrentUnitOfWork.BeginTransation();
	....
	UnitOfWorkFactory.CurrentUnitOfWork.CommitTransation() or RollBack;

	UnitOfWorkFactory.CurrentUnitOfWork在这个静态属性中, 会获取当前的HttpContext.Current对象, 并缓存进去一个IUnitOfWork的实现实例
	这个实例当然仅仅在这一次http请求中有效.
	我们再来看看Class UnitOfWork在初始化中干了什么
	public UnitOfWork(IDbContext context)
        {
            _context = context;
        }
	UnitOfWork依赖于IDbContext, 在初始化的时候会从Autofac容器中