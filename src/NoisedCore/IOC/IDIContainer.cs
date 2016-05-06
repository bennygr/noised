namespace Noised.Core.IOC
{
    /// <summary>
    ///     A DI Container used by Noised
    /// </summary>
    public interface IDIContainer
    {
        /// <summary>
        ///     Build thes container an all it's dependecies	
        /// </summary>
        void Build();

        /// <summary>
        ///     Gets a dependency of type T from the container
        /// </summary>
        T Get<T>();
    };
}
