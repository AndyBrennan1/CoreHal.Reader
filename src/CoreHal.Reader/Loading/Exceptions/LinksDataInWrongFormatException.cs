using System;

namespace CoreHal.Reader.Loading.Exceptions
{
    /// <summary>
    /// Represents errors that occur due to link data not being supplied in the required format.
    /// All links information should be passed as IDictionary{string, object}
    /// </summary>
    public class LinksDataInWrongFormatException : Exception
    {
        private const string message = "All links information should be passed as IDictionary<string, object>. Copy and paste the following into your IDE (you'll need to format it) as a guide for what 'good' looks like.";

        private const string example =
            "IDictionary<string, object> dataBeingLoaded = new Dictionary<string, object>" +
            "{" +
                "{" +
                    "\"_links\"," +
                        "new Dictionary<string, object>" +
                            "{{" +
                                "\"self\"," +
                                "new Dictionary<string, object>" +
                                    "{" +
                                        "{ " +
                                            "\"href\", \"/api/categories\" }," +
                                            "{ \"title\", \"Some Title\" }" +
                                        "}" +
                                    "}," +
                                    "{" +
                                        "\"products\"," +
                                        "new List<Dictionary<string, object>>" +
                                        "{" +
                                            "{" +
                                                "new Dictionary<string, object>" +
                                                "{" +
                                                    "{ \"href\", \"/api/products/product1\" }," +
                                                    "{ \"title\", \"Product\" }" +
                                                "}" +
                                            "}," +
                                        "{" +
                                            "new Dictionary<string, object>" +
                                            "{" +
                                                "{ " +
                                                    "\"href\", \"/api/products/product2\" }," +
                                                    "{ \"title\", \"Product\" }" +
                                                "}" +
                                            "}" +
                                        "}" +
                                    "}" +
                                "}" +
                            "}" +
                        "};";

        public LinksDataInWrongFormatException()
            : base($"{message}{Environment.NewLine}{example}")
        {
        }
    }
}