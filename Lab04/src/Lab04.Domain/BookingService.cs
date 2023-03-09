using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Lab04.Domain.Interface;

namespace Lab04.Domain
{
    public class BookingService
    {
        private readonly MongoDbHandler _dataManager;

        public BookingService(MongoDbHandler dataManager)
        {
            _dataManager = dataManager;
        }
    }
}