
namespace Noised.Core.IOC
{
    /// <summary>
    ///	 Noised core depedency injection container
    /// </summary>
    static class IoC
    {
        //Noised uses the LightCore implementation by default
        private static readonly IDIContainer container = new LightCoreDIContainer();

        #region Properties

        /// <summary>
        ///     The DI Container 
        /// </summary>
        public static IDIContainer Container
        {
            get
            {
                return container;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the dependency injection-container
        /// </summary>
        public static void Build()
        {
            container.Build();
        }

        /// <summary>
        ///	Get the Service for type T
        /// </summary>
        /// <remarks>Shortcut for IoC.Container.Get()</remarks>
        /// <return>The service fot type T</return>
        public static T Get<T>()
        {
            return container.Get<T>();
        }

        #endregion
    };
}
