using System.Collections.Generic;

namespace CoreHal.Reader.Loading
{
    /// <summary>
    /// Represents a Hal response loader responsible for taking the raw data retrieved from an API and 
    /// transforming it into a format accepted by CoreHal.
    /// </summary>
    public interface IHalResponseLoader
    {
        /// <summary>
        /// Loads the provided raw response data into an <see cref="Dictionary{string, object} "/> which is the 
        /// allowed format for the receipt of data into a <see cref="HalResource"/>.
        /// </summary>
        /// <param name="rawResponse">The raw Hal+Json data string obtained by calling an API that returns data that adheres to the Hal+Json format.</param>
        /// <returns>The raw response data loaded into an <see cref="Dictionary{string, object} "/>.</returns>
        /// <remarks>
        /// The resulting <see cref="Dictionary{string, object} "/> must NOT 
        /// 1) Contain any custom reference types.
        /// 2) Use dictionary types that are not <see cref="Dictionary{string, object} "/> for complex properties.
        /// </remarks>
        IDictionary<string, object> Load(string rawResponse);
    }
}