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
    public UserDocument UserDocument { get; private set; }
    public float Price { get; private set; }
    public string Location { get; private set; }
    public DateOnly DateRequested { get; private set; }
    public bool IsActive { get; private set; }

    public BookingDocument(int id, UserDocument userDocument, IPaymentCalculator paymentCalculator, string location,
        DateOnly dateRequested)
    {
        Id = id;
        UserDocument = userDocument;
        Location = location;
        DateRequested = dateRequested;
        Price = paymentCalculator.GetPrice(this);
        IsActive = true;
    }

    public void DisableBooking()
    {
        IsActive = false;
    }
    
    public void ChangeDate(DateOnly dateOnly)
    {
        DateRequested = dateOnly;
    }

    public void AddFeeToPrice(float fee)
    {
        Price += fee;
    }
}