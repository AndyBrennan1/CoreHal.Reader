namespace CoreHal.Reader
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHalResourceLoader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawResponse"></param>
        void Load(string rawResponse);
    }
}