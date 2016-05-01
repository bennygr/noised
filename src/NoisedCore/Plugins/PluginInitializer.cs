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
        ///	Gets the DI container
        /// </summary>
        public IDIContainer DIContainer
        {
            get
            {
                return  IoC.Container;
            }
        }

        /// <summary>
        ///		Logging access
        /// </summary>
        public ILogging Logging{ get; internal set; }
    };
}
