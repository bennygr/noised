using System;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Commands;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class CoreCommandBundlePlugin : ICommandBundle
    {
        #region Constructor

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="pluginInitializer">initalizer</param>
        public CoreCommandBundlePlugin(PluginInitializer pluginInitializer) { }

        #endregion

        #region IDisposable

        public void Dispose() { }

        #endregion
    };
}
