﻿using EchoNest;
using EchoNest.Playlist;
using EchoNest.Song;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TopTastic.tests
{
    [TestClass]
    [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP0100:AdvancedNamingRules", Justification = "OK.")]
    public class EchoNestPlaylistTests : EchoNestTests
    {

        [DataRow("Jimi Hendrix")]
        [DataRow("Amy Winehouse")]
        [DataRow("Miles Davis")]
        [DataRow("Alison Krauss")]
        [DataRow("Led Zeppelin")]
        [TestMethod]
        public void GetBasicPlaylist_WhereArtistName_HasSongsByArtist(string artistName)
        {
            BasicArgument basicArgument = new BasicArgument
            {
                Results = 10,
                Dmca = true
            };

            TermList artistTerms = new TermList { artistName };
            basicArgument.Artist.AddRange(artistTerms);

            using (var session = new EchoNestSession(ApiKey))
            {
                PlaylistResponse searchResponse = session.Query<Basic>().Execute(basicArgument);

                Assert.IsNotNull(searchResponse);

                System.Diagnostics.Debug.WriteLine("Songs for : {0}", artistName);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    System.Diagnostics.Debug.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }

            }
        }

        [DataRow("Apocalypse Now Phyc Rock", "60s,guitar,psychadelic,rock,sountrack^0.5", "eeirie^0.5,dark^0.5,disturbing^0.5,groovy^0.5,melancholia^0.5,ominous^0.5")]
        [DataRow("Apocalypse Now", "60s,psychadelic,rock^0.5,sountrack^0.5", "eeirie,dark,disturbing,groovy,melancholia,ominous^0.5")]
        [TestMethod]
        public void GetStaticPlaylist_WhereMoodAndStyle_HasVarietyOfArtists(string title, string styles, string moods)
        {
            TermList styleTerms = new TermList();
            foreach (string s in styles.Split(','))
            {
                styleTerms.Add(s);
            }

            TermList moodTerms = new TermList();
            foreach (string s in moods.Split(','))
            {
                moodTerms.Add(s);
            }

            StaticArgument staticArgument = new StaticArgument
            {
                Results = 25,
                Adventurousness = 0.4,
                Type = "artist-description",
                Variety = 0.4 /* variety of artists */
            };

            staticArgument.Styles.AddRange(styleTerms);

            staticArgument.Moods.AddRange(moodTerms);

            using (var session = new EchoNestSession(ApiKey))
            {
                PlaylistResponse searchResponse = session.Query<Static>().Execute(staticArgument);

                Assert.IsNotNull(searchResponse);
                Assert.IsNotNull(searchResponse.Songs);
                Assert.IsTrue(searchResponse.Songs.Any());

                System.Diagnostics.Debug.WriteLine("Songs for : {0}", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    System.Diagnostics.Debug.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }
            }
        }
    }

}
