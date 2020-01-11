using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF.Query;

namespace ProcessingServer.Models
{
    public class Triple
    {
        public string Property { get; set; }
        public string HasValue { get; set; }
        public string IsValueOf { get; set; }

        public Triple(SparqlResult result)
        {
            try
            {
                Property = result["property"].ToString();
            }
            catch
            { Property = ""; }
            try
            {
                HasValue = result["hasValue"].ToString();
            }
            catch
            { HasValue = ""; }
            try
            {
                IsValueOf = result["isValueOf"].ToString();
            }
            catch
            { IsValueOf = ""; }
        }
    }
}
