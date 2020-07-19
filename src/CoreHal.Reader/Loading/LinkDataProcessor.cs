using CoreHal.Graph;
using CoreHal.Reader.Loading.Exceptions;
using System.Collections;
using System.Collections.Generic;

namespace CoreHal.Reader.Loading
{
    internal class LinkDataProcessor
    {
        private const string LinksKey = "_links";
        private const string LinkHRefKey = "href";
        private const string LinkTitleKey = "title";

        IDictionary<string, IEnumerable<Link>> linksCollection;

        internal void Process(IDictionary<string, object> loadedData, IDictionary<string, IEnumerable<Link>> links)
        {
            linksCollection = links;

            if (loadedData.ContainsKey(LinksKey))
            {
                if (loadedData[LinksKey] is IDictionary<string, object> linkRelSetDictionary)
                {
                    foreach (var linkRelSetKeyValuePair in linkRelSetDictionary)
                    {
                        if (linkRelSetKeyValuePair.Value is IDictionary)
                        {
                            ProcessSingularLink(linkRelSetKeyValuePair);
                        }
                        else
                        {
                            ProcessLinkCollection(linkRelSetKeyValuePair);
                        }
                    }
                }
                else
                {
                    throw new LinksDataInWrongFormatException();
                }

                loadedData.Remove(LinksKey);
            }
        }

        private void ProcessLinkCollection(KeyValuePair<string, object> keyValuePair)
        {
            var linkPropertySet = (IEnumerable<Dictionary<string, object>>)keyValuePair.Value;

            var thisSetOfLink = new List<Link>();

            foreach (var linkPropertyDictionary in linkPropertySet)
            {
                thisSetOfLink.Add(GetLink(linkPropertyDictionary));
            }

            linksCollection.Add(keyValuePair.Key, new List<Link>(thisSetOfLink));
        }

        private void ProcessSingularLink(KeyValuePair<string, object> keyValuePair)
        {
            var linksList =
                new List<Link>
                {
                    GetLink((IDictionary<string, object>)keyValuePair.Value)
                };

            linksCollection.Add(keyValuePair.Key, linksList);
        }

        private static Link GetLink(IDictionary<string, object> linkProperties)
        {
            var href = linkProperties[LinkHRefKey].ToString();

            var title = linkProperties.ContainsKey(LinkTitleKey)
                        ? linkProperties[LinkTitleKey].ToString()
                        : null;

            var link = new Link(href, title);

            return link;
        }
    }
}
