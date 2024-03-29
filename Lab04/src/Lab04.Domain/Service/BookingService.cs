using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Lab04.Domain.Interface;
using Lab04.Domain.Model;
using Lab04.Domain.Repository;
using Ardalis.GuardClauses;

namespace Lab04.Domain.Service
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly IPaymentCalculator _paymentCalculator;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IEmailSystem _emailSystem;

        public BookingService(BookingRepository bookingRepository, IPaymentCalculator paymentCalculator,
            IPaymentGateway paymentGateway, IEmailSystem emailSystem)
        {
            _bookingRepository = bookingRepository;
            _paymentCalculator = paymentCalculator;
            _paymentGateway = paymentGateway;
            _emailSystem = emailSystem;
        }

        public BookingDocument CreateNewBooking(int userId, string userName, 
            int bookingId, string bookingLocation, DateOnly bookingStartDate, string userEmail = "")
        {
            var newBooking = new BookingDocument(
                paymentCalculator: _paymentCalculator,
                userDocument: new UserDocument(userId, userName, userEmail),
                id: bookingId,
                location: bookingLocation,
                dateRequested: bookingStartDate);
            
            // If user gives email send email with successful booking information
            CheckAndSendEmail(newBooking, () => 
                _emailSystem.SendBookingInformationEmail(newBooking));

            return newBooking;
        }

        public void AddBookingToDb(BookingDocument bookingDocument)
        {
            try
            {
                // On success start payment process from user
                StartPaymentProcess(bookingDocument.Price);
            }
            catch (Exception e)
            {
                CheckAndSendEmail(bookingDocument, () => 
                    _emailSystem.SendFailedPaymentEmail(bookingDocument));
                
                Console.WriteLine(e);
                throw;
            }
            
            // If everything works save to DB
            _bookingRepository.SaveToDatabase(bookingDocument);
        }

        public BookingDocument GetBookingFromDb(int bookingId)
        {
            return _bookingRepository.GetFromDatabase(bookingId);
            //  ?? throw new Exception("Not found")
        }

        public void StartBookingCancelProcess(BookingDocument bookingDocument)
        {
            // var bookingDocument = _dataManager.GetFromDatabase(bookingId);
            DateOnly dateNow = DateOnly.FromDateTime(DateTime.Today);
            // Guard.Against.Null(bookingDocument, nameof(bookingDocument));

            if (dateNow.AddDays(60) < bookingDocument.DateRequested) // more than 60 days
            {
                _paymentGateway.RequestPaymentRefund(1, bookingDocument.Price);
            }
            else if (dateNow.AddDays(30) < bookingDocument.DateRequested) // more than 30 days
            {
                _paymentGateway.RequestPaymentRefund(1, bookingDocument.Price * 0.75f);
            }
            else if (dateNow.AddDays(7) < bookingDocument.DateRequested) // more than 7 days
            {
                _paymentGateway.RequestPaymentRefund(1, bookingDocument.Price * 0.50f);
            }
            else if (dateNow >= bookingDocument.DateRequested) // user trying to cancel after booking start date
            {
                CheckAndSendEmail(bookingDocument, () => 
                    _emailSystem.SendFailedBookingCancellationEmail(bookingDocument,
                        $"Trying to cancel booking past start date, Today: {dateNow}," +
                        $" booking start date: {bookingDocument.DateRequested}"));
                
                throw new Exception($"Trying to cancel booking past start date, Today: {dateNow}," +
                                    $" booking start date: {bookingDocument.DateRequested}");
            }
            else
            {
                CheckAndSendEmail(bookingDocument, () => 
                    _emailSystem.SendFailedBookingCancellationEmail(bookingDocument,
                        $"Something went wrong, Error with server."));
                throw new NotImplementedException("Something went wrong, unhandled condition.");
            }

            CheckAndSendEmail(bookingDocument, () => 
                _emailSystem.SendSuccessfulBookingCancellationEmail(bookingDocument));
            bookingDocument.DisableBooking();
        }

        public void CheckAndSendEmail(BookingDocument bookingDocument, Action emailAction)
        {
            if (!string.IsNullOrWhiteSpace(bookingDocument.UserDocument.Email))
            {
                emailAction.Invoke();
            }
        }
        
        public void ChangeBookingDate(BookingDocument bookingDocument, int newDaysFromToday)
        {
            DateOnly newDate = DateOnly.FromDateTime(DateTime.Today.AddDays(newDaysFromToday));
            int differenceBetweenDates = 
                bookingDocument.DateRequested.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber;
            float fee = 0.0f;
            
            if (differenceBetweenDates <= 0) throw new Exception("Too late to change date!");

            // if (differenceBetweenDates <= 21)
            // {
            //     bookingDocument.ChangeDate(newDate);
            //     _bookingRepository.UpdateToDatabase(bookingDocument);
            //     return;
            // }
            
            if (differenceBetweenDates <= 2) fee = bookingDocument.Price * 0.4f;
            else if (differenceBetweenDates <= 10) fee = bookingDocument.Price * 0.3f;
            else if (differenceBetweenDates <= 20) fee = bookingDocument.Price * 0.2f;

            StartPaymentProcess(float.Round(fee));
            bookingDocument.AddFeeToPrice(fee);
            bookingDocument.ChangeDate(newDate);
            _bookingRepository.UpdateToDatabase(bookingDocument);
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
}