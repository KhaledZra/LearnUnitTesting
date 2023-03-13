using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Lab04.Domain.Interface;
using Lab04.Domain.Model;
using Ardalis.GuardClauses;

namespace Lab04.Domain
{
    public class BookingService
    {
        private readonly MongoDbHandler _dataManager;
        private readonly IPaymentCalculator _paymentCalculator;
        private readonly IPaymentGateway _paymentGateway;
        
        public BookingService(MongoDbHandler dataManager, IPaymentCalculator paymentCalculator,
            IPaymentGateway paymentGateway = null)
        {
            _dataManager = dataManager;
            _paymentCalculator = paymentCalculator;
            _paymentGateway = paymentGateway;
        }

        public BookingDocument CreateNewBooking(int userId, string userName, 
            int bookingId, string bookingLocation, DateOnly bookingStartDate)
        {
            return new BookingDocument(
                paymentCalculator: _paymentCalculator,
                user: new User(userId, userName, _paymentGateway),
                id: bookingId,
                location: bookingLocation,
                dateRequested: bookingStartDate);
        }

        public void AddBookingToDb(BookingDocument bookingDocument)
        {
            try
            {
                // On success start payment process from user
                bookingDocument.User.StartPaymentProcess(bookingDocument.Price);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            // If everything works save to DB
            _dataManager.SaveToDatabase(bookingDocument);
        }

        public BookingDocument GetBookingFromDb(int id)
        {
            return _dataManager.GetFromDatabase(id);
            //  ?? throw new Exception("Not found")
        }

        public void StartBookingCancelProcess(int id)
        {
            Guard.Against.Null(_dataManager.GetFromDatabase(id), nameof(id));

        }
    }
}