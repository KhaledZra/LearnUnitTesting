using System;
using Autofac;
using Autofac.Extras.FakeItEasy;
using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using FluentAssertions;
using Xunit;
using FakeItEasy;
using Lab04.Domain.Interface;
using Lab04.Domain.Model;
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
            _fake.Provide<BookingDocument>(_fake.Resolve<BookingService>().CreateNewBooking(
                userId:1,
                userName: "khaled",
                bookingId: 1,
                bookingLocation: "skene",
                bookingStartDate: DateOnly.FromDateTime(DateTime.Today)));
        }
        
        [Fact]
        public void If_booking_is_successful_add_to_mongo_db()
        {
            // Arrange
            BookingService sut = _fake.Resolve<BookingService>();
            BookingDocument bookingDocument = _fake.Resolve<BookingDocument>();

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
        
        
        // TODO IN ALL TESTS BELOW, apply changes from autofac and new BookingService constructor
        [Fact]
        public void Booking_payment_from_user_needs_to_be_captured() 
        {
            // Arrange
            IPaymentGateway fakePaymentGateway = A.Fake<IPaymentGateway>();
            IPaymentCalculator fakePaymentCalculator = A.Fake<IPaymentCalculator>();
            A.CallTo(() => fakePaymentCalculator
                    .GetPrice(A<BookingDocument>.Ignored, A<string>.Ignored, A<DateOnly>.Ignored))
                    .Returns(50);
            
            User user = new User(1, "Khaled", fakePaymentGateway);
            BookingService sut = new BookingService(
                new MongoDbHandler(this.mongoClient), fakePaymentCalculator);
            
            // Act
            sut.AddBookingToDb(new BookingDocument(1, user, fakePaymentCalculator, "skene",
                DateOnly.FromDateTime(DateTime.Today)));

            // Assert
            A.CallTo(() => fakePaymentGateway.SendPayment(50)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Booking_payment_should_use_external_service_to_calculate_price()
        {
            // Arrange
            IPaymentGateway fakePaymentGateway = A.Fake<IPaymentGateway>();
            IPaymentCalculator fakePaymentCalculator = A.Fake<IPaymentCalculator>();
            A.CallTo(() => fakePaymentCalculator
                .GetPrice(A<BookingDocument>.Ignored, A<string>.Ignored, A<DateOnly>.Ignored))
                .Returns(50);
            
            User user = new User(1, "Khaled", fakePaymentGateway);
            BookingService sut = new BookingService(
                new MongoDbHandler(this.mongoClient), fakePaymentCalculator);
            
            // Act
            sut.AddBookingToDb(new BookingDocument(1, user, fakePaymentCalculator,
                "skene", DateOnly.FromDateTime(DateTime.Today)));

            // Assert
            A.CallTo(() => fakePaymentGateway.SendPayment(50)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Booking_should_not_be_persisted_if_payment_fails()
        {
            // Arrange
            IPaymentGateway fakePaymentGateway = A.Fake<IPaymentGateway>();
            IPaymentCalculator fakePaymentCalculator = A.Fake<IPaymentCalculator>();
            A.CallTo(() => fakePaymentGateway
                .SendPayment(A<float>.Ignored))
                .Throws(new Exception("Something went wrong"));

            User user = new User(1, "Khaled", fakePaymentGateway);
            BookingService sut = new BookingService(
                new MongoDbHandler(this.mongoClient), fakePaymentCalculator);
            
            // Act
            Action action = () => sut.AddBookingToDb(new BookingDocument(1, user, fakePaymentCalculator,
                "skene", DateOnly.FromDateTime(DateTime.Today)));

            // Assert
            action.Should().Throw<Exception>("Something went wrong");
            sut.GetBookingFromDb(1).Should().BeNull();
        }
        
        [Fact]
        public void Booking_should_not_be_persisted_if_payment_fails0()
        {
            // Arrange
            IPaymentGateway fakePaymentGateway = A.Fake<IPaymentGateway>();
            IPaymentCalculator fakePaymentCalculator = A.Fake<IPaymentCalculator>();
            A.CallTo(() => fakePaymentGateway
                    .SendPayment(A<float>.Ignored))
                .Throws(new Exception("Something went wrong"));

            User user = new User(1, "Khaled", fakePaymentGateway);
            BookingService sut = new BookingService(
                new MongoDbHandler(this.mongoClient), fakePaymentCalculator);
            
            // Act
            Action action = () => sut.AddBookingToDb(new BookingDocument(1, user, fakePaymentCalculator,
                "skene", DateOnly.FromDateTime(DateTime.Today)));

            // Assert
            action.Should().Throw<Exception>("Something went wrong");
            sut.GetBookingFromDb(1).Should().BeNull();
        }
    }
}