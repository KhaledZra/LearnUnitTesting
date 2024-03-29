﻿using System;
using Autofac;
using Autofac.Core;
using Autofac.Extras.FakeItEasy;
using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using FluentAssertions;
using Xunit;
using FakeItEasy;
using Lab04.Domain.Interface;
using Lab04.Domain.Model;
using Lab04.Domain.Service;
using MongoDB.Driver;

namespace Lab04.Domain.Tests
{
    
    //private readonly Fixture _fixture;
    //private readonly IContainer _container;
    
    // creates a new autofac container and register the dependencies
    // var builder = new ContainerBuilder(); // initialize the builder
    // builder.RegisterType<IPaymentGateway>().AsSelf(); // Dependencies
    // builder.RegisterType<IPaymentCalculator>().AsSelf(); // Dependencies
    // _container = builder.Build(); // new autofac container
            
    // create a new AutoFixture instance and configure it to use FakeItEasy for dependency injection
    // _fixture = (Fixture)new Fixture().Customize(new AutoFakeItEasyCustomization());
    // _fixture.Inject(this.mongoClient);
    // _fixture.Inject(new AutoFake().Resolve<IPaymentGateway>());
    // _fixture.Inject(new AutoFake().Resolve<IPaymentCalculator>());

    public class BookingServiceTests : MongoDbIntegrationTest
    {
        private readonly AutoFake _fake;

        public BookingServiceTests()
        {
            _fake = new AutoFake();
            _fake.Provide<IMongoClient>(this.mongoClient);
            // _fake.Provide<BookingDocument>(_fake.Resolve<BookingService>().CreateNewBooking(
            //     userId:1,
            //     userName: "khaled",
            //     bookingId: 1,
            //     bookingLocation: "skene",
            //     bookingStartDate: DateOnly.FromDateTime(DateTime.Today)));
        }
        
        [Fact]
        public void If_booking_is_successful_add_to_mongo_db()
        {
            // Arrange
            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = sut.CreateNewBooking(
                userId:1,
                userName: "khaled",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today));

                // Act
            sut.AddBookingToDb(bookingDocument);

            // Assert
            sut.GetBookingFromDb(1).Should().BeEquivalentTo(bookingDocument);
        }
        
        // [Fact]
        // public void If_booking_is_successful_add_to_mongo_db()
        // {
        //     // Arrange
        //     IPaymentGateway fakePaymentGateway = A.Fake<IPaymentGateway>();
        //     IPaymentCalculator fakePaymentCalculator = A.Fake<IPaymentCalculator>();
        //
        //     DateOnly dateTimeStamp = DateOnly.FromDateTime(DateTime.Today);
        //     
        //     BookingService sut = new BookingService(new MongoDbHandler(this.mongoClient), fakePaymentCalculator);
        //     BookingDocument bookingDocument = new BookingDocument(
        //         1, new User(1,"Khaled", fakePaymentGateway),
        //         fakePaymentCalculator, "skene", dateTimeStamp);
        //
        //     // Act
        //     sut.AddBookingToDb(bookingDocument);
        //
        //     // Assert
        //     sut.GetBookingFromDb(1).Should().BeEquivalentTo(
        //         new BookingDocument(
        //             id: 1,
        //             user: new User(1, "Khaled", fakePaymentGateway),
        //             paymentCalculator: fakePaymentCalculator,
        //             location: "skene",
        //             dateRequested: dateTimeStamp)); // named params example
        // }
        
        
        [Fact]
        public void Booking_payment_from_user_needs_to_be_captured() 
        {
            // Arrange
            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = sut.CreateNewBooking(
                userId:1,
                userName: "khaled",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today));
            
            A.CallTo(() => _fake.Resolve<IPaymentCalculator>()
                    .GetPrice(A<BookingDocument>.Ignored)).Returns(50);
            
            // Act
            sut.StartPaymentProcess(50);

            // Assert
            A.CallTo(() => _fake.Resolve<IPaymentGateway>().SendPayment(50)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Booking_payment_should_use_external_service_to_calculate_price()
        {
            // Arrange
            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = sut.CreateNewBooking(
                userId:1,
                userName: "khaled",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today));
            
            A.CallTo(() => _fake.Resolve<IPaymentCalculator>()
                    .GetPrice(A<BookingDocument>.Ignored))
                .Returns(50);
            
            // Act
            sut.AddBookingToDb(bookingDocument);

            // Assert
            A.CallTo(() => _fake.Resolve<IPaymentGateway>().SendPayment(A<float>.Ignored)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Booking_should_not_be_persisted_if_payment_fails()
        {
            // Arrange
            A.CallTo(() => _fake.Resolve<IPaymentGateway>()
                .SendPayment(A<float>.Ignored))
                .Throws(new Exception("Something went wrong"));

            BookingService sut = _fake.Resolve<BookingService>();
            
            // Act
            Action action = () => sut.AddBookingToDb(sut.CreateNewBooking(
                userId:1,
                userName: "khaled",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today)));
        
            // Assert
            action.Should().Throw<Exception>("Something went wrong");
            sut.GetBookingFromDb(1).Should().BeNull();
        }
        
        [Theory]
        [InlineData(61, 75.0f, 75.0f)]
        [InlineData(31, 75.0f, 56.25f)]
        [InlineData(8, 75.0f, 37.5f)]
        public void Booking_should_refund_based_on_days_left_before_start_date(int daysLeft, float pricePaid,
            float expectedRefundValue)
        {
            // Arrange
            A.CallTo(() => _fake.Resolve<IPaymentCalculator>()
                .GetPrice(A<BookingDocument>.Ignored))
                .Returns(pricePaid);

            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = sut.CreateNewBooking(
                userId: 1,
                userName: "khaled",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(daysLeft));
            
            sut.AddBookingToDb(bookingDocument);
            
            // Act
            sut.StartBookingCancelProcess(bookingDocument);

            // Assert
            A.CallTo(() => _fake.Resolve<IPaymentGateway>()
                .RequestPaymentRefund(1, expectedRefundValue)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Booking_should_send_email_if_given_by_user()
        {
            // Arrange
            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = sut.CreateNewBooking(
                userId: 1,
                userName: "khaled",
                userEmail: "khaled@gmail.com",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(100));
            
            // Act
            sut.AddBookingToDb(bookingDocument);
        
            // Assert
            A.CallTo(() => _fake.Resolve<IEmailSystem>()
                .SendBookingInformationEmail(bookingDocument)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Booking_should_send_email_if_payment_fails()
        {
            // Arrange
            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = sut.CreateNewBooking(
                userId: 1,
                userName: "khaled",
                userEmail: "khaled@gmail.com",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(100));

            A.CallTo(() => _fake
                    .Resolve<IPaymentGateway>()
                    .SendPayment(A<float>.Ignored))
                    .Throws(new Exception("Error from server side"));
            
            // Act
            Action act = () => sut.AddBookingToDb(bookingDocument);
        
            // Assert
            act.Should().Throw<Exception>("Error from server side");
            A.CallTo(() => _fake.Resolve<IEmailSystem>()
                .SendFailedPaymentEmail(bookingDocument)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Booking_should_send_email_on_cancellation_failure()
        {
            // Arrange
            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = sut.CreateNewBooking(
                userId: 1,
                userName: "khaled",
                userEmail: "khaled@gmail.com",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(100));

            A.CallTo(() => _fake
                    .Resolve<IPaymentGateway>()
                    .SendPayment(A<float>.Ignored))
                    .Throws(new Exception("Error from server side"));
            
            // Act
            Action act = () => sut.AddBookingToDb(bookingDocument);
        
            // Assert
            act.Should().Throw<Exception>("Error from server side");
            A.CallTo(() => _fake.Resolve<IEmailSystem>()
                .SendFailedPaymentEmail(bookingDocument)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Booking_should_send_email_on_cancellation_success()
        {
            // Arrange
            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = sut.CreateNewBooking(
                userId: 1,
                userName: "khaled",
                userEmail: "khaled@gmail.com",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(100));
            
            sut.AddBookingToDb(bookingDocument);

            // Act
            sut.StartBookingCancelProcess(bookingDocument);
        
            // Assert
            A.CallTo(() => _fake.Resolve<IEmailSystem>()
                .SendSuccessfulBookingCancellationEmail(bookingDocument)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Booking_should_send_email_on_cancellation_failure_with_reason()
        {
            // Arrange
            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = sut.CreateNewBooking(
                userId: 1,
                userName: "khaled",
                userEmail: "khaled@gmail.com",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(-5));
            
            sut.AddBookingToDb(bookingDocument);

            // Act
            try
            {
                sut.StartBookingCancelProcess(bookingDocument);
            }
            catch { }
        
            // Assert
            A.CallTo(() => _fake.Resolve<IEmailSystem>()
                .SendFailedBookingCancellationEmail(bookingDocument, A<string>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Theory]
        [InlineData(20, 100, 100.0f, 20.0f)]
        [InlineData(10, 100, 100.0f, 30.0f)]
        [InlineData(2, 100, 100.0f, 40.0f)]
        public void Booking_should_charge_additional_fee_on_date_change(
            int daysFromToday, int newDaysFromToday, float price, float expectedAdditionalFee)
        {
            // Arrange
            A.CallTo(() => _fake.Resolve<IPaymentCalculator>()
                .GetPrice(A<BookingDocument>.Ignored))
                .Returns(price);
            
            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = sut.CreateNewBooking(
                userId: 1,
                userName: "khaled",
                userEmail: "khaled@gmail.com",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today).AddDays(daysFromToday));

            sut.AddBookingToDb(bookingDocument);

            // Act
            sut.ChangeBookingDate(bookingDocument, newDaysFromToday);
            
            // Assert
            A.CallTo(() => _fake.Resolve<IPaymentGateway>()
                .SendPayment(price))
                .MustHaveHappenedOnceExactly();
            
            A.CallTo(() => _fake.Resolve<IPaymentGateway>()
                .SendPayment(expectedAdditionalFee))
                .MustHaveHappenedOnceExactly();
        }
    }
}