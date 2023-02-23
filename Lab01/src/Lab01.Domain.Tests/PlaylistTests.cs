using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Xunit;
using FluentAssertions;
namespace Lab01.Domain.Tests
{
    public class PlaylistTests : IDisposable
    {
        private readonly Playlist _sut;

        public PlaylistTests()
        {
            _sut = new Playlist();
        }
        
        [Fact]
        public void Active_when_created()
        {
            // Arrange 
            Playlist sut = new Playlist();

            // Act
            
            // Assert
            sut.IsActive.Should().BeTrue();
            
            // Teardown
            Dispose();
        }
        
        [Fact]
        public void Title_must_not_be_null()
        { 
            // Arrange 
            var sut = new Playlist();

            // Act
            
            // Assert
            sut.Title.Should().NotBeNull();
        }
        
        [Fact]
        public void Title_must_not_be_empty()
        {
            // Arrange 
            var sut = new Playlist();

            // Act
            
            // Assert
            sut.Title.Should().NotBeNullOrEmpty();
        }
        
        [Fact]
        public void Add_song_to_a_playlist()
        {
            // Arrange 
            var sut = new Playlist();
            Song song = new Song();

            // Act
            song.Name = "MySong";
            sut.Songs.Add(song);

            // Assert
            sut.Songs.Should().Contain(song);
        }
        
        [Fact]
        public void New_playlist_has_empty_song_list()
        {
            var sut = new Playlist();

            // only one assert
            sut.Songs.Should().BeEmpty();
        }
        
        [Fact]
        public void Song_added_save_instance()
        {
            // Arrange
            var sut = new Playlist();
            var song = new Song();
            
            // Act
            song.Name = "MySong";
            sut.Songs.Add(song);

            // Assert
            sut.Songs.Should().BeEquivalentTo(song);
        }
        
        [Fact]
        public void Song_ban_abba_from_playlist()
        {
            // Arrange
            var sut = new Playlist();
            var song = new Song();
            
            // Act
            song.Artist = "Abba";

            // Assert
            sut.TryAddNewSong(song).Should().BeFalse();
        }
        
        [Fact]
        public void Playlist_can_be_cleared()
        {
            // Arrange
            var sut = new Playlist();
            sut.Songs.Add(new Song());

            // Act
            sut.Songs.Clear();

            // Assert
            sut.Songs.Should().BeEmpty();
        }
        
        [Fact]
        public void Playlist_dupe_songs_are_ignored()
        {
            // Arrange
            var sut = new Playlist();
            Song song = new Song {Artist = "Khaled", Name = "Hej"};

            // Act
            sut.TryAddNewSong(song);
            sut.TryAddNewSong(song);
            sut.TryAddNewSong(song);

            // Assert
            sut.Songs.Should().HaveCount(1);
        }
        
        [Fact]
        public void Playlist_faulty_songs_are_removed()
        {
            // Arrange
            var sut = new Playlist();
            Song song = new Song {Artist = "Khaled", Name = "Khaled"};
            sut.Songs.Add(song);
            
            // Act
            sut.RemoveFaultySongs();

            // Assert
            sut.Songs.Should().HaveCount(0);
        }
        
        [Fact]
        public void Playlist_automaticly_adds_year_to_title()
        {
            // Arrange
            var sut = new Playlist();
            
            // Act
            sut.SetTitle("Khaled");
            
            // Assert
            sut.Title.Should().Contain(DateTime.Now.Year.ToString());
        }
        
        [Fact]
        public void Playlist_is_sorted_by_artist_then_title()
        {
            // Arrange
            var sut = new Playlist();
            var song1 = new Song() {Artist = "Carl", Name = "Banjo times", DurationInMinutes = 5.0f, ReleaseDate = DateTime.Now.AddDays(2)};
            var song2 = new Song() {Artist = "Carl", Name = "Adamanite", DurationInMinutes = 5.0f, ReleaseDate = DateTime.Now.AddDays(2)};
            var song3 = new Song() {Artist = "Adam", Name = "Hej", DurationInMinutes = 5.0f, ReleaseDate = DateTime.Now.AddDays(1)};

            // Act
            sut.TryAddNewSong(song3);
            sut.TryAddNewSong(song2);
            sut.TryAddNewSong(song1);

            // Assert
            sut.Songs[0].Should().BeEquivalentTo(song3);
        }
        
        [Fact]
        public void Songs_over_eight_minutes_are_rejected()
        {
            // Arrange
            var sut = new Playlist();

            // Act
            var song = new Song() {Artist = "Khaled", Name = "Hej", DurationInMinutes = 9.0f};

            // Assert
            sut.TryAddNewSong(song).Should().BeFalse();
        }
        
        [Fact]
        public void Playlist_can_retrieve_list_of_unique_artists()
        {
            // Arrange
            var sut = new Playlist();
            var song1 = new Song() {Artist = "Khaled", Name = "Hej", DurationInMinutes = 5.0f};
            var song2 = new Song() {Artist = "Khaled", Name = "Då", DurationInMinutes = 6.0f};
            sut.TryAddNewSong(song1);
            sut.TryAddNewSong(song2);

            // Act
            var uniqueList = sut.RetrieveUniqueArtistList();

            // Assert
            uniqueList.Should().HaveCount(1);
        }

        public void Dispose()
        {
            _sut.CleanUp();
        }
    }
}
