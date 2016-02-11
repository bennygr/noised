using Noised.Core.IOC;

namespace Noised.Core.DB.Sqlite
{
    internal class SqliteDbFactory : IDbFactory
    {
        #region Implementation of IDbFactory

        public IUnitOfWork GetUnitOfWork()
        {
            return IocContainer.Get<IUnitOfWork>();
        }

        #endregion
    }
}
