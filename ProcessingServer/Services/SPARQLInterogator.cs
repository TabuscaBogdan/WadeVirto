using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using ProcessingServer.Models;
using VDS.RDF.Query;

namespace ProcessingServer.Services
{
    public class SPARQLInterogator
    {
        private SparqlRemoteEndpoint endpoint;
        private readonly string DescriptionProperty = "http://purl.org/dc/elements/1.1/description";
        private readonly string TitleProperty = "http://purl.org/dc/elements/1.1/title";
        private readonly string LabelProperty = "http://www.w3.org/2000/01/rdf-schema#label";
        private readonly string IdProperty = "http://purl.org/ontology/mo/musicbrainz";
        private readonly string TypeProperty = "http://www.w3.org/1999/02/22-rdf-syntax-ns#type";
        private readonly string LengthProperty = "http://purl.org/ontology/mo/length";
        private readonly string TrackIdProperty = "http://purl.org/ontology/mo/musicbrainz";
        private readonly string MakerProperty = "http://xmlns.com/foaf/0.1/maker";
        private readonly string TagProperty = "http://www.holygoat.co.uk/owl/redwood/0.1/tags/Tag";

        private void AddUsualQueryNamespaces(SparqlParameterizedString queryString)
        {
            queryString.Namespaces.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
            queryString.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            queryString.Namespaces.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
            queryString.Namespaces.AddNamespace("vocab", new Uri("http://dbtune.org/musicbrainz/resource/vocab/"));
            queryString.Namespaces.AddNamespace("rel", new Uri("http://purl.org/vocab/relationship/"));
        }
        public SPARQLInterogator()
        {
            endpoint = new SparqlRemoteEndpoint(new Uri("http://dbtune.org/musicbrainz/sparql"));
            SparqlParameterizedString queryString = new SparqlParameterizedString();

        }

        public List<string> GetTagLinks(List<string> generes)
        {
            var tagLinks = new List<string>();

            SparqlParameterizedString queryString = new SparqlParameterizedString();
            AddUsualQueryNamespaces(queryString);
            foreach (var genere in generes)
            {
                queryString.CommandText =
                    $"SELECT DISTINCT ?item WHERE {{ ?item rdfs:label \"{genere}\" . ?item rdf:type <http://www.holygoat.co.uk/owl/redwood/0.1/tags/Tag>}}";
                SparqlResultSet results = endpoint.QueryWithResultSet(queryString.ToString());
                foreach (var result in results)
                {
                    try
                    {
                        tagLinks.Add(result["item"].ToString());
                    }
                    catch
                    { }
                }
            }
            //SELECT DISTINCT ?item WHERE { ?item rdfs:label "rock" . ?item rdf:type <http://www.holygoat.co.uk/owl/redwood/0.1/tags/Tag>}
            return tagLinks;
        }

        public List<Triple> GetTagInformation(string tagLink)
        {
            var tagInformation = new List<Triple>();

            SparqlParameterizedString queryString = new SparqlParameterizedString();
            AddUsualQueryNamespaces(queryString);

            queryString.CommandText =
                $"SELECT DISTINCT ?property ?hasValue ?isValueOf " +
                $"WHERE {{ {{<{tagLink}> ?property ?hasValue }} UNION" +
                $"{{ ?isValueOf ?property <{tagLink}> }} }}" +
                $"ORDER BY (!BOUND(?hasValue)) ?property ?hasValue ?isValueOf LIMIT 200";

            SparqlResultSet results = endpoint.QueryWithResultSet(queryString.ToString());

            foreach(var result in results)
            {
                tagInformation.Add(new Triple(result));
            }
            return tagInformation;
        }


        public List<string> GetArtistLinks(List<string> artists)
        {
            var artistLinks = new List<string>();

            SparqlParameterizedString queryString = new SparqlParameterizedString();
            AddUsualQueryNamespaces(queryString);
            foreach (var artist in artists)
            {
                queryString.CommandText =
                    $"SELECT DISTINCT ?item WHERE {{ ?item rdfs:label \"{artist}\" . ?item rdf:type <http://purl.org/ontology/mo/MusicArtist>}}";
                SparqlResultSet results = endpoint.QueryWithResultSet(queryString.ToString());
                foreach (var result in results)
                {
                    var link = result.ToString();
                    link = link.Split("=")[1];
                    link = link.Trim();
                    artistLinks.Add(link);
                }
            }

            return artistLinks;
        }


        public List<Triple> GetTrackInformation(string trackLink)
        {
            var trackInformation = new List<Triple>();

            SparqlParameterizedString queryString = new SparqlParameterizedString();
            AddUsualQueryNamespaces(queryString);

            queryString.CommandText =
                $"SELECT DISTINCT ?property ?hasValue ?isValueOf WHERE {{" +
                $"{{<{trackLink}> ?property ?hasValue}} UNION " +
                $"{{?isValueOf ?property <{trackLink}>}}" +
                $"}}ORDER BY (!BOUND(?hasValue)) ?property ?hasValue ?isValueOf LIMIT 100";

            SparqlResultSet results = endpoint.QueryWithResultSet(queryString.ToString());

            foreach (var result in results)
            {
                trackInformation.Add(new Triple(result));
            }

            return trackInformation;
        }

        public List<Triple> GetArtistInformation(string artistLink)
        {
            var artistInfromation = new List<Triple>();

            SparqlParameterizedString queryString = new SparqlParameterizedString();
            AddUsualQueryNamespaces(queryString);
            queryString.CommandText =
                $"SELECT DISTINCT ?property ?hasValue ?isValueOf WHERE {{ " +
                $"{{ <{artistLink}> ?property ?hasValue }} UNION " +
                $"{{ ?isValueOf ?property <{artistLink}> }}" +
                $"}}ORDER BY(!BOUND(?hasValue)) ?property ?hasValue ?isValueOf LIMIT 400";

            SparqlResultSet results = endpoint.QueryWithResultSet(queryString.ToString());

            foreach(var result in results)
            {
                artistInfromation.Add(new Triple(result));
            }

            return artistInfromation;
        }

        public List<string> ExtractTrackLinks(List<Triple> informationLines)
        {
            var artistLinks = new List<string>();

            foreach(var infoTriple in informationLines)
            {
                if (infoTriple.IsValueOf != null)
                    if (infoTriple.IsValueOf.Contains("/track/"))
                        artistLinks.Add(infoTriple.IsValueOf);
            }
            return artistLinks;
        }

        public JsonLDMaker ExtractArtistMakerInformation(List<Triple> artistInformation)
        {
            var makerInfo = new JsonLDMaker();
            foreach(var infoTriple in artistInformation)
            {
                if (infoTriple.Property.Equals(LabelProperty))
                    makerInfo.foafName = infoTriple.HasValue;
                if (infoTriple.Property.Equals(IdProperty))
                    makerInfo.id = infoTriple.HasValue;
                if (infoTriple.Property.Equals(TypeProperty))
                    makerInfo.type = infoTriple.HasValue;
                if (infoTriple.Property.Equals(DescriptionProperty))
                    makerInfo.description = infoTriple.HasValue;
            }

            return makerInfo;
        }
        public JsonLDSong ExtractSongInformation(List<Triple> informationLines)
        {
            var songInfo = new JsonLDSong();
            foreach(var infoTriple in informationLines)
            {
                if (infoTriple.Property.Equals(TitleProperty))
                    songInfo.dcTitle = infoTriple.HasValue;
                if (infoTriple.Property.Equals(TrackIdProperty))
                    songInfo.id = infoTriple.HasValue;
                if (infoTriple.Property.Equals(TypeProperty))
                    songInfo.type = infoTriple.HasValue;
                if(infoTriple.Property.Equals(LengthProperty))
                {
                    int len = 0;
                    int.TryParse(infoTriple.HasValue.Split("^")[0], out len);
                    songInfo.length = len;
                }
                if(infoTriple.Property.Equals(MakerProperty))
                {
                    var artistInfo = GetArtistInformation(infoTriple.HasValue);
                    var maker = ExtractArtistMakerInformation(artistInfo);
                    songInfo.foafMaker = maker;
                }
            }
            return songInfo;
        }

        public JsonLDSong ExtractSongInformation(List<Triple> informationLines, JsonLDMaker parentArtist)
        {
            var songInfo = new JsonLDSong();
            songInfo.foafMaker = parentArtist;
            foreach (var infoTriple in informationLines)
            {
                if (infoTriple.Property.Equals(TitleProperty))
                    songInfo.dcTitle = infoTriple.HasValue;
                if (infoTriple.Property.Equals(TrackIdProperty))
                    songInfo.id = infoTriple.HasValue;
                if (infoTriple.Property.Equals(TypeProperty))
                    songInfo.type = infoTriple.HasValue;
                if (infoTriple.Property.Equals(LengthProperty))
                {
                    int len = 0;
                    int.TryParse(infoTriple.HasValue.Split("^")[0], out len);
                    songInfo.length = len;
                }
                
            }
            return songInfo;
        }




    }
}
