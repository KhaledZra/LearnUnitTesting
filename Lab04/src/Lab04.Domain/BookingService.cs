using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Lab04.Domain.Interface;
using Lab04.Domain.Model;

namespace Lab04.Domain
{
    public class BookingService
    {
        private readonly MongoDbHandler _dataManager;
        private readonly IPaymentCalculator _paymentCalculator;

        public BookingService(MongoDbHandler dataManager, IPaymentCalculator paymentCalculator)
        {
            _dataManager = dataManager;
            _paymentCalculator = paymentCalculator;
        }

        public void AddBookingToDb(BookingDocument bookingDocument)
        {
            try
            {
                _dataManager.SaveToDatabase(bookingDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public BookingDocument GetBookingFromDb(int id)
        {
            return _dataManager.GetFromDatabase(id);
            //  ?? throw new Exception("Not found")
            
        }
    }
}