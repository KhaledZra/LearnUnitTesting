using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Lab04.Domain.Model;


public class Booking
{
    [BsonId]
    public string Name { get; set; }
    public string Place { get; set; }

    public Booking(string name, string place)
    {
        Name = name;
        Place = place;
    }
}