using System.Collections.Generic;
using Lab04.Domain.Interface;
using Lab04.Domain.Model;
using MongoDB.Driver;

namespace Lab04.Domain;

public class MongoDbHandler
{
    private List<Booking> _fakeDbList = new List<Booking>();

    private MongoClientSettings _settings;
    private MongoClient _client;
    private IMongoDatabase _test;

    public MongoDbHandler()
    {
        this._settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017");
        this._client = new MongoClient(_settings);
        IMongoDatabase test = _client.GetDatabase("");
    }

    public bool SaveToDatabase(Booking booking)
    {
        throw new System.NotImplementedException();
    }

    public Booking GetFromDatabase(int id)
    {
        throw new System.NotImplementedException();
    }

    public bool UpdateToDatabase(Booking booking)
    {
        throw new System.NotImplementedException();
    }

    public bool RemoveFromDatabase(int id)
    {
        throw new System.NotImplementedException();
    }
}