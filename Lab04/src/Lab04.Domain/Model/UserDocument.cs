using System;
using Lab04.Domain.Interface;
using MongoDB.Bson.Serialization.Attributes;

namespace Lab04.Domain.Model;

public class UserDocument : IUser
{
    [BsonId] 
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    private IPaymentGateway _paymentGateway;
    public UserDocument(int id, string name, IPaymentGateway paymentGateway, string email)
    {
        Id = id;
        Name = name;
        _paymentGateway = paymentGateway;
        Email = email;
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