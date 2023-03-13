using System;
using Lab04.Domain.Interface;
using MongoDB.Bson.Serialization.Attributes;

namespace Lab04.Domain.Model;

public class User : IUser
{
    [BsonId] 
    public int Id { get; set; }
    public string Name { get; set; }
    private IPaymentGateway _paymentGateway;
    public User(int id, string name, IPaymentGateway paymentGateway)
    {
        Id = id;
        Name = name;
        _paymentGateway = paymentGateway;
    }
    
    public void StartPaymentProcess(float payment)
    {
        try
        {
            _paymentGateway.SendPayment(payment);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}