using FluentAssertions;
using Xunit;
using FakeItEasy;
using Lab04.Domain.Interface;
using Lab04.Domain.Model;

namespace Lab04.Domain.Tests
{
    public class BookingServiceTests : MongoDbIntegrationTest
    {
        [Fact]
        public void If_booking_is_successful_add_to_mongo_db()
        {
            // Arrange
            //User fakeUser = A.Fake<User>();
            IPaymentGateway fakePaymentGateway = A.Fake<IPaymentGateway>();
            IPaymentCalculator fakePaymentCalculator = A.Fake<IPaymentCalculator>();
            //A.CallTo(() => fakeUser.Name).Returns("Khaled");
            
            // Use autoFac
            // use autoFicture
            
            BookingService sut = new BookingService(new MongoDbHandler(this.mongoClient), fakePaymentCalculator);
            BookingDocument bookingDocument = new BookingDocument(
                1, new User(1,"Khaled", fakePaymentGateway), fakePaymentCalculator);

            // Act
            sut.AddBookingToDb(bookingDocument);

            // Assert
            sut.GetBookingFromDb(1).Should().BeEquivalentTo(
                new BookingDocument(
                    1, new User(1, "Khaled", fakePaymentGateway), fakePaymentCalculator));
        }
        
        [Fact]
        public void Booking_payment_from_user_needs_to_be_captured()
        {
            // Arrange
            IPaymentGateway fakePaymentGateway = A.Fake<IPaymentGateway>();
            IPaymentCalculator fakePaymentCalculator = A.Fake<IPaymentCalculator>();
            A.CallTo(() => fakePaymentCalculator.GetPrice()).Returns(50);
            
            User user = new User(1, "Khaled", fakePaymentGateway);
            BookingService sut = new BookingService(
                new MongoDbHandler(this.mongoClient), fakePaymentCalculator);
            
            // Act
            sut.AddBookingToDb(new BookingDocument(1, user, fakePaymentCalculator));

            // Assert
            A.CallTo(() => fakePaymentGateway.SendPayment(50)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void Booking_payment_should_use_external_service_to_calculate_price()
        {
            // Arrange
            IPaymentGateway fakePaymentGateway = A.Fake<IPaymentGateway>();
            IPaymentCalculator fakePaymentCalculator = A.Fake<IPaymentCalculator>();
            A.CallTo(() => fakePaymentCalculator.GetPrice()).Returns(50);
            
            User user = new User(1, "Khaled", fakePaymentGateway);
            BookingService sut = new BookingService(
                new MongoDbHandler(this.mongoClient), fakePaymentCalculator);
            
            // Act
            sut.AddBookingToDb(new BookingDocument(1, user, fakePaymentCalculator));

            // Assert
            A.CallTo(() => fakePaymentGateway.SendPayment(50)).MustHaveHappenedOnceExactly();
        }
    }
}