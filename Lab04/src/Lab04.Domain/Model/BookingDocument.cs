using Lab04.Domain.Interface;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Lab04.Domain.Model;


public class BookingDocument
{
    [BsonId]
    public int Id { get; set; }
    public User User { get; set; }
    public float Price { get; set; }

    public BookingDocument(int id, User user, IPaymentCalculator paymentCalculator)
    {
        Id = id;
        User = user;
        Price = paymentCalculator.GetPrice();
        
        // On success start payment process from user
        user.StartPaymentProcess(Price);
    }
}