using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab01.Domain
{
  public class Playlist
  {
    public string Title { get; private set;  }
    public bool IsActive { get; private set; }
    
    public List<Song> Songs { get; private set; }

    public Playlist()
    {
      Title = "Unassigned";
      IsActive = true;
      Songs = new List<Song>();
    }

    public void SetTitle(string title)
    {
      Title = title + ", " + DateTime.Today.Year;
    }

    public bool TryAddNewSong(Song song)
    {
      if (!song.Artist.Contains("abba", StringComparison.CurrentCultureIgnoreCase)) // banned artist check
      {
        if (Songs.FirstOrDefault(s => s.Artist == song.Artist && s.Name == song.Name) == null) // dupe check
        {
          if (song.DurationInMinutes < 8.0f) // duration check
          {
            Songs.Add(song);
            if (Songs.Count >= 2)
            {
              Songs = Songs
                .OrderBy(s => s.ReleaseDate)
                .ThenBy(s => s.Artist)
                .ThenBy(s => s.Name)
                .ToList();
            }
            return true;
          }
        }
      }
      
      return false;
    }

    public void RemoveFaultySongs()
    {
      if (Songs.FirstOrDefault(s => s.Artist == s.Name) != null)
      {
        Songs.Remove(Songs.FirstOrDefault(s => s.Artist == s.Name));
      }
    }
    
    public List<string> RetrieveUniqueArtistList()
    {
      List<string> uniqueArtistList = new List<string>();
      
      Songs.ForEach(song =>
      {
        if (!uniqueArtistList.Contains(song.Artist))
        {
          uniqueArtistList.Add(song.Artist);
        }
      });

      return uniqueArtistList;
    }
  }
}
