using CoreHal.Reader.Loading.Exceptions;
using System.Collections;
using System.Collections.Generic;

namespace CoreHal.Reader.Loading
{
    internal class EmbeddedItemDataProcessor
    {
        private const string EmbeddedItemsKey = "_embedded";

        private IDictionary<string, IEnumerable<HalResource>> embedded;

        internal void Process(
            IDictionary<string, object> loadedData, 
            IDictionary<string, IEnumerable<HalResource>> embeddedItems)
        {
            embedded = embeddedItems;

            if (loadedData.ContainsKey(EmbeddedItemsKey))
            {
                if (loadedData[EmbeddedItemsKey] is IDictionary<string, object>)
                {
                    var embeddedItemDictionary = (Dictionary<string, object>)loadedData[EmbeddedItemsKey];

                    foreach (var keyValuePair in embeddedItemDictionary)
                    {
                        if (keyValuePair.Value is IDictionary)
                        {
                            ProcessSingularEmbededItem(keyValuePair);
                        }
                        else
                        {
                            ProcessEmbeddedItemSet(keyValuePair);
                        }
                    }
                }
                else
                {
                    throw new EmbeddedItemDataInWrongFormatException();
                }

                loadedData.Remove(EmbeddedItemsKey);
            }
        }

        private void ProcessSingularEmbededItem(
            KeyValuePair<string, object> keyValuePair)
        {
            var embeddedItemHalReader = new HalResource((Dictionary<string, object>)keyValuePair.Value);

            embedded.Add(
                keyValuePair.Key,
                new List<HalResource> { embeddedItemHalReader }
                );
        }

        private void ProcessEmbeddedItemSet(KeyValuePair<string, object> keyValuePair)
        {
            var embededItemSetItems = (IEnumerable<Dictionary<string, object>>)keyValuePair.Value;

            var embeddedItemSet = new List<HalResource>();

            foreach (var embeddedItem in embededItemSetItems)
            {
                var embeddedItemHalReader = new HalResource(embeddedItem);

                embeddedItemSet.Add(embeddedItemHalReader);
            }

            embedded.Add(
                keyValuePair.Key,
                new List<HalResource>(embeddedItemSet));
        }
    }
}