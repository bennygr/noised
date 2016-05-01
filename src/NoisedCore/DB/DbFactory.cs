using Noised.Core.IOC;

namespace Noised.Core.DB
{
    /// <summary>
    /// DbFactory creates instances of IUnitOfWork to access the Database
    /// </summary>
    internal class DbFactory : IDbFactory
    {
        #region Implementation of IDbFactory

        /// <summary>
        /// Creates a new instance of the current IUnitOfWork implementation
        /// </summary>
        /// <returns>A new instance of the current IUnitOfWork implementation</returns>
        public IUnitOfWork GetUnitOfWork()
        {
            return IoC.Get<IUnitOfWork>();
        }

        #endregion
    }
}
