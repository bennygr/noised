namespace Noised.Core.Config
{
    /// <summary>
    ///		A source which supplies raw data representing configuration properties
    /// </summary>
    public interface IConfigSource
    {
        /// <summary>
        ///		Gets raw configuration data
        /// </summary>
        string GetSourceData();
    };
}
