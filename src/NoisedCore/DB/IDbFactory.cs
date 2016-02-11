namespace Noised.Core.DB
{
    public interface IDbFactory
    {
        IUnitOfWork GetUnitOfWork();
    }
}
