using System;
using System.Collections.Generic;
using Lab04.Domain.Model;
using MongoDB.Driver;

namespace Lab04.Domain.Repository;

public class BookingRepository // middleman between service and database
{
    // Not used
    private List<BookingDocument> _fakeDbList = new List<BookingDocument>();
    
    //private readonly MongoClientSettings _settings;
    //private readonly IMongoClient _client;
    private readonly IMongoCollection<BookingDocument> _collection;

    // IMongoClient is injected instead
    public BookingRepository(IMongoClient mongoClient)
    {
        // Better to do this elsewhere when injecting mongoClient with preset settings
        //this._settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017");
        //this._client = mongoClient;
        
        this._collection = mongoClient
            .GetDatabase("Lab4")
            .GetCollection<BookingDocument>("BookingCollection");
    }

    public void SaveToDatabase(BookingDocument booking)
    {
        try
        {
            _collection.InsertOne(booking);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public BookingDocument GetFromDatabase(int id)
    {
        return _collection.Find<BookingDocument>(bd => bd.Id == id).FirstOrDefault();
    }

    public bool UpdateToDatabase(BookingDocument booking)
    {
        return _collection.ReplaceOne<BookingDocument>(bd => bd.Id == booking.Id, booking)
            .IsAcknowledged;
    }

    public bool RemoveFromDatabase(int id)
    {
        throw new System.NotImplementedException();
    }
}