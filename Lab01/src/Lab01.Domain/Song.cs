using System;

namespace Lab01.Domain;

public class Song
{
    public string Name { get; set; }
    public string Artist { get; set; }
    public float DurationInMinutes { get; set; }
    public DateTime ReleaseDate { get; set; }
}