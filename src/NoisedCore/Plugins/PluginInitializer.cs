using Noised.Logging;
using Noised.Core.IOC;

namespace Noised.Core.Plugins
{
    /// <summary>
    ///		Plugin initializing data
    /// </summary>
    public class PluginInitializer
    {
		/// <summary>
		///		Gets the service for type T
		/// </summary>
        public T Get<T>()
        {
            return IocContainer.Get<T>();
        }

        /// <summary>
        ///		Logging access
        /// </summary>
        public ILogging Logging{ get; internal set; }
    };
}
