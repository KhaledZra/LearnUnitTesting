using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Lab04.Domain.Model;

public class UserDocument
{
    [BsonId] 
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    
    public UserDocument(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
}