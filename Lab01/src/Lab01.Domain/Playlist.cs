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
      if (!song.Artist.Contains("abba", StringComparison.CurrentCultureIgnoreCase))
      {
        if (Songs.FirstOrDefault(s => s.Artist == song.Artist) == null)
        {
          Songs.Add(song);
          return true;
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
  }
}
