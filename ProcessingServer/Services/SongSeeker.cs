using ProcessingServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessingServer.Services
{
    public class SongSeeker
    {
        private Random random;
        public SongSeeker()
        {
            random = new Random();
        }
        private List<T> Pick<T>(List<T> elements, int numberOfChoices=1)
        {
            int attempts = 100;
            if (elements.Count <= numberOfChoices)
                return elements;

            int index = random.Next(elements.Count);
            var pickedElements = new List<T>();
            var pickedIndexes = new List<int>();
            pickedIndexes.Add(index);
            pickedElements.Add(elements[index]);

            while(pickedIndexes.Count<numberOfChoices && attempts>0)
            {
                index = random.Next(elements.Count);
                if(pickedIndexes.Contains(index))
                {
                    attempts--;
                }
                else
                {
                    pickedIndexes.Add(index);
                    pickedElements.Add(elements[index]);
                }
            }

            return pickedElements;
        }
        private List<JsonLDSong> GetArtistSongs(SPARQLInterogator sparqlInterogator,List<string> artists,int songsPerCategory=3)
        {
            var chosenSongs = new List<JsonLDSong>();
            foreach (var artistLink in artists)
            {
                var artistInfo = sparqlInterogator.GetArtistInformation(artistLink);

                var makerInfo = sparqlInterogator.ExtractArtistMakerInformation(artistInfo);
                var trackLinks = sparqlInterogator.ExtractTrackLinks(artistInfo);

                var chosenTracks = Pick(trackLinks, songsPerCategory);

                foreach (var trackLink in chosenTracks)
                {
                    var trackInfo = sparqlInterogator.GetTrackInformation(trackLink);
                    var songLD = sparqlInterogator.ExtractSongInformation(trackInfo, makerInfo);
                    chosenSongs.Add(songLD);
                }

            }
            return chosenSongs;
        }

        public SongList SeekSongsBasedOnPreferences(Dictionary<string,List<string>> preferences, int songsPerCategory=3)
        {
            var chosenSongs = new List<JsonLDSong>();

            var sparqlInterogator = new SPARQLInterogator();

            var artists = sparqlInterogator.GetArtistLinks(preferences[NaturalLanguageProcessor.likeArtist]);
            var badArtists = sparqlInterogator.GetArtistLinks(preferences[NaturalLanguageProcessor.notLikeArtist]);

            var tagLinks = sparqlInterogator.GetTagLinks(preferences[NaturalLanguageProcessor.likeGenere]);
            var badTagLinks = sparqlInterogator.GetTagLinks(preferences[NaturalLanguageProcessor.notLikeGenere]);

            chosenSongs.AddRange(GetArtistSongs(sparqlInterogator, artists, songsPerCategory));
            

            foreach(var tagLink in tagLinks)
            {
                var tagInfo = sparqlInterogator.GetTagInformation(tagLink);
                var taggedArtistLinks = sparqlInterogator.ExtractArtistLinks(tagInfo);
                var chosenArtists = Pick(taggedArtistLinks, songsPerCategory);

                var filteredArtistsLinks = new List<string>();
                foreach(var chosenArtist in chosenArtists)
                {
                    if (!badArtists.Contains(chosenArtist))
                        if (!sparqlInterogator.CheckArtistIsTagged(chosenArtist, badTagLinks))
                            filteredArtistsLinks.Add(chosenArtist);
                }

                chosenSongs.AddRange(GetArtistSongs(sparqlInterogator, filteredArtistsLinks, songsPerCategory));
            }

            SongList songList = new SongList();
            songList.Songs = chosenSongs;
            songList.ListName = DateTime.Now.ToString() + " Querry List";

            return songList;
        }








    }
}
