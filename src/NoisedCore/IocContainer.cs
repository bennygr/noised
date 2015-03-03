using LightCore;
using LightCore.Lifecycle;
using Noised.Core.Plugins;
using Noised.Logging;

/// <summary>
///		Noised core depedency injection container
/// </summary>
public static class IocContainer
{
	#region Fields
	
	private static IContainer container; 
	
	#endregion

	#region Methods
	
	/// <summary>
	///		Creates the dependency injection-container
	/// </summary>
	public static void Build()
	{
		ContainerBuilder builder = new ContainerBuilder();
	
		//Logging
		builder.Register<ILogging,Logger>().
			ControlledBy<SingletonLifecycle>();
		//Plugins
		builder.Register<IPluginLoader,PluginLoader>().
			ControlledBy<SingletonLifecycle>();
	
		container = builder.Build();
	}
	
	/// <summary>
	///		Get the Service for type T
	/// </summary>
	/// <return>The service fot type T</return>
	public static T Get<T>()
	{
		return container.Resolve<T>();
	}
	
	#endregion
};
