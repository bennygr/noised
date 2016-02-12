namespace Noised.Core.DB
{
    /// <summary>
    /// Interface of a Factory for creating instances of IUnitOfWork to access the Database
    /// </summary>
    public interface IDbFactory
    {
        /// <summary>
        /// Creates a new instance of the current IUnitOfWork implementation
        /// </summary>
        /// <returns>A new instance of the current IUnitOfWork implementation</returns>
        IUnitOfWork GetUnitOfWork();
    }
}
