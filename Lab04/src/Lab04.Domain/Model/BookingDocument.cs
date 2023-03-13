using System;
using System.Runtime.InteropServices.JavaScript;
using Lab04.Domain.Interface;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Lab04.Domain.Model;


public class BookingDocument
{
    [BsonId]
    public int Id { get; private set; }
    public User User { get; private set; }
    public float Price { get; private set; }
    public string Location { get; private set; }
    public DateOnly DateRequested { get; private set; }

    public BookingDocument(int id, User user, IPaymentCalculator paymentCalculator, string location,
        DateOnly dateRequested)
    {
        Id = id;
        User = user;
        Location = location;
        DateRequested = dateRequested;
        
        Price = paymentCalculator.GetPrice(this, Location, DateRequested);
    }
}