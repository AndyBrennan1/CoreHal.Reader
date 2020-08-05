using System.Collections.Generic;

namespace CoreHal.Reader
{
    public class Problem
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int? Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
        public IDictionary<string, object> Extensions { get; }
    }
}