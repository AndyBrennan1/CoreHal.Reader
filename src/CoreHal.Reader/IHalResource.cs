using System;
using CoreHal.Graph;
using System.Collections.Generic;
using CoreHal.Reader.Mapping;
using CoreHal.Reader.Loading.Exceptions;
using CoreHal.Reader.Mapping.Exceptions;

namespace CoreHal.Reader
{
    /// <summary>
    /// Defines the interface for a resource that has been retrieved from an API that conforms to the application/hal+json media type.
    /// See http://stateless.co/hal_specification.html for details.
    /// </summary>
    public interface IHalResource
    {
        /// <summary>
        /// Gets the collection of embedded (child or related) resources that were fetched along with the main resource data.
        /// </summary>
        IDictionary<string, IEnumerable<HalResource>> EmbeddedItems { get; }

        /// <summary>
        /// Gets the collection of links that are considered useful in the context of the resource the data represents.
        /// </summary>
        IDictionary<string, IEnumerable<Link>> Links { get; }
        
        /// <summary>
        /// Gets the collection of properties for the core resource data.
        /// </summary>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// Accesses the named 'simple' property and returns its value as the specified type.
        /// </summary>
        /// <typeparam name="TProperty">The type to cast the simple property into.</typeparam>
        /// <param name="propertyName">The name of the property to access.</param>
        /// <returns>The value of the named property cast as type <typeparamref name="TProperty"/></returns>
        /// <remarks>
        /// A simple property has no sub properties of it's own.
        /// </remarks>
        TProperty CastSimplePropertyTo<TProperty>(string propertyName);

        /// <summary>
        /// Accesses the named 'complex' property and returns its value as the specified type.
        /// </summary>
        /// <typeparam name="TProperty">The type to cast the simple property into - it must be a reference type that has a parameterless constructor.</typeparam>
        /// <param name="propertyName">The name of the property to access.</param>
        /// <param name="mappings">
        /// A collection of <see cref="KeyValuePair{string, string}"/> which resolves between the epected property name found on <typeparamref name="TProperty"/> 
        /// and the property name that was returned by the API call.
        /// Key = The property name on <typeparamref name="TProperty"/> 
        /// Value = The property name as returned by the API response. 
        /// </param>
        /// <returns>The value of the named property cast as type <typeparamref name="TProperty"/></returns>
        /// <remarks>
        /// A complex property has sub properties of it's own.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <see cref="null"/> has been provided as the property name.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when an empty <see cref="string"/> has been provided as the property name.
        /// </exception>
        /// <exception cref="NoSuchPropertyException">
        /// Thrown when a property that does not exist in the resource proeprties collection has been requested.
        /// </exception>
        /// <exception cref="InvalidComplexPropertyException">
        /// Thrown when the complex property to cast is anything other than <see cref="IDictionary{string, object}"/>. 
        /// Only complex properties that are stored as <see cref="IDictionary{string, object}"/> can be cast into 
        /// an entity of type <typeparamref name="TProperty"/>
        /// </exception>
        TProperty CastComplexPropertyTo<TProperty>(string propertyName, params KeyValuePair<string, string>[] mappings) where TProperty : new();

        /// <summary>
        /// Casts the resource properties into an object of type <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">The entity type to cast the resource properties into.</typeparam>
        /// <returns>The data properties of the resource cast into the specifed entity type <typeparamref name="TEntity"/>.</returns>
        /// <exception cref="NoMappersProvidedException">
        /// Thrown when an attempt has been made to map the <see cref="IHalResource"/> to a custom reference type and no <see cref="IEntityMapperFactory"/> 
        /// has been provided to access the required mapper.
        /// </exception>
        /// <exception cref="TypeHasNoMapperException">
        /// Thrown when an attempt to map to a type that has no mapper registered is made.
        /// </exception>
        /// <exception cref="ProblemWithMapperException">
        /// Thrown when a mapper was found for the casting type, but there was a problem executing it. 
        /// This hints at a configuration problem within the mapper.
        /// </exception>
        /// <exception cref="ResourceHasNoPropertiesException">
        /// Thrown when the resource to map has no properties in it. This suggests there is a potential configuration issue.
        /// </exception>
        TEntity CastResourceAs<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Accesses the (singular) embedded item found against the specified key and casts it to the type <typeparamref name="TEmbedded"/>.
        /// </summary>
        /// <typeparam name="TEmbedded">The entity type to cast the embedded resource properties into.</typeparam>
        /// <param name="embeddedItemKey">The key from which to access the embedded resource.</param>
        /// <returns>The embedded item found against the provided key and cast as type <typeparamref name="TEmbedded"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <see cref="null"/> has been provided as the embedded item key name.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when an empty <see cref="string"/> has been provided as the embedded item key name.
        /// </exception>
        /// <exception cref="NoEmbeddedItemWithKeyException">
        /// Thrown when there are no embedded items with the key as provided (does not include where the key exists but the content is empty).
        /// </exception>
        /// <exception cref="EmbeddedItemIsCollectionException">
        /// Thrown when the key provided is for an embedded item collection rather than for a singular embedded item.
        /// </exception>
        TEmbedded CastEmbeddedItemAs<TEmbedded>(string embeddedItemKey) where TEmbedded : class, new();

        /// <summary>
        /// Accesses the embedded item collection found against the specified key and casts it to a collection of type <typeparamref name="TEmbedded"/>.
        /// </summary>
        /// <typeparam name="TEmbedded">The entity type to cast the embedded resource properties into.</typeparam>
        /// <param name="embeddedItemKey">The key from which to access the embedded resource.</param>
        /// <returns>The collection of embedded items found against the provided key and cast as a collection of type <typeparamref name="TEmbedded"/>.</returns>
        IEnumerable<TEmbedded> CastEmbeddedItemSetAs<TEmbedded>(string embeddedItemKey) where TEmbedded : class, new();
    }
}