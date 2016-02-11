using Noised.Core.IOC;

namespace Noised.Core.DB
{
    internal class DbFactory : IDbFactory
    {
        #region Implementation of IDbFactory

        public IUnitOfWork GetUnitOfWork()
        {
            return IocContainer.Get<IUnitOfWork>();
        }

        #endregion
    }
}
