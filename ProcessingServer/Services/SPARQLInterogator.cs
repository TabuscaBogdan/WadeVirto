using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using VDS.RDF.Query;

namespace ProcessingServer.Services
{
    public class SPARQLInterogator
    {
        private SparqlRemoteEndpoint endpoint;

        private void AddUsualQueryNamespaces(SparqlParameterizedString queryString)
        {
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            queryString.Namespaces.AddNamespace("foaf",new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("vocab",new Uri("http://dbtune.org/musicbrainz/resource/vocab/"));
            queryString.Namespaces.AddNamespace("rel",new Uri("http://purl.org/vocab/relationship/"));
        }
        public SPARQLInterogator()
        {
            endpoint = new SparqlRemoteEndpoint(new Uri("http://dbtune.org/musicbrainz/sparql"));
            SparqlParameterizedString queryString = new SparqlParameterizedString();

        }

        public List<string> GetArtistInfo(List<string> artists)
        {
            var ids = new List<string>();
            SparqlParameterizedString queryString = new SparqlParameterizedString();
            AddUsualQueryNamespaces(queryString);
            queryString.CommandText =
                "SELECT DISTINCT ?item WHERE { ?item rdfs:label @value1 . ?item rdf:type <http://purl.org/ontology/mo/MusicArtist>}";
            foreach (var artist in artists)
            {
                queryString.CommandText =
                    $"SELECT DISTINCT ?item WHERE {{ ?item rdfs:label \"{artist}\" . ?item rdf:type <http://purl.org/ontology/mo/MusicArtist>}}";
                SparqlResultSet results = endpoint.QueryWithResultSet(queryString.ToString());
                foreach (var result in results)
                {
                    ids.Add(result.ToString());
                }
            }

            return ids;
        }
        
    }
}
