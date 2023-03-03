using System;
using Xunit;
using FluentAssertions;
using Lab03.Domain;
using FakeItEasy;

namespace Lab03.Domain.Tests
{
    public class BookingTests
    {
        [Fact]
        public void Booking_should_take_payment()
        {
            // Arrange
            PaymentSystem fakePaymentSystemClass = A.Fake<PaymentSystem>(); // Fake initialization
            BookingService sut = new BookingService(fakePaymentSystemClass); // user services instead
            //A.CallTo(() => fakePaymentSystemClass.Pay()).DoesNothing(); // already done by Fake

            // Act
            sut.Confirm1();

            // Assert
            A.CallTo(() => fakePaymentSystemClass.Pay()).MustHaveHappenedOnceExactly(); // mock
        }
        
        [Fact]
        public void Booking_should_take_two_payments_worth_50_each()
        {
            // Arrange
            PaymentSystem fakePaymentSystemClass = A.Fake<PaymentSystem>(); // Fake 
            A.CallTo(() => fakePaymentSystemClass.Pay()).DoesNothing();
            A.CallTo(() => fakePaymentSystemClass.Balance()).Returns(50);
            BookingService sut = new BookingService(fakePaymentSystemClass);

            // Act
            sut.Confirm2();

            // Assert
            A.CallTo(() => fakePaymentSystemClass.Pay()).MustHaveHappenedTwiceExactly(); // mock
        }
        
        [Fact]
        public void Booking_should_take_one_payment_with_any_vat()
        {
            // Arrange
            PaymentSystem fakePaymentSystemClass = A.Fake<PaymentSystem>(); // Fake 
            A.CallTo(() => fakePaymentSystemClass.Pay()).DoesNothing();
            A.CallTo(() => fakePaymentSystemClass.Balance()).Returns(50);
            A.CallTo(() => fakePaymentSystemClass.Vat()).Returns(5);
            BookingService sut = new BookingService(fakePaymentSystemClass);

            // Act
            sut.Confirm3();

            // Assert
            A.CallTo(() => fakePaymentSystemClass.Pay()).MustHaveHappenedOnceExactly(); // mock
        }
    }
}