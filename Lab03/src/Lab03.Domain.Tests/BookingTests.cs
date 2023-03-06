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
            PriceCalculator fakePriceCalculator = A.Dummy<PriceCalculator>(); // Dummy
            BookingService sut = new BookingService(fakePaymentSystemClass, fakePriceCalculator); // user services instead

            // Act
            sut.Confirm1();

            // Assert
            A.CallTo(() => fakePaymentSystemClass.Pay(A<int>.Ignored)).MustHaveHappenedOnceExactly(); // mock
        }
        
        [Fact]
        public void Booking_should_take_two_payments_worth_50_each()
        {
            // Arrange
            PaymentSystem fakePaymentSystemClass = A.Fake<PaymentSystem>(); // Fake 
            int paymentValue = 50;
            A.CallTo(() => fakePaymentSystemClass.Balance()).Returns(paymentValue);
            PriceCalculator fakePriceCalculator = A.Dummy<PriceCalculator>(); // Dummy
            BookingService sut = new BookingService(fakePaymentSystemClass, fakePriceCalculator); // user services instead

            // Act
            sut.Confirm2();

            // Assert
            A.CallTo(() => fakePaymentSystemClass.Pay(paymentValue)).MustHaveHappenedTwiceExactly(); // mock
            A.CallTo(() => fakePaymentSystemClass.Balance()).MustHaveHappenedTwiceExactly(); // mock
        }
        
        [Fact]
        public void Booking_should_take_one_payment_with_any_vat()
        {
            // Arrange
            PaymentSystem fakePaymentSystemClass = A.Fake<PaymentSystem>(); // Fake 
            int paymentValue = 100;
            int vatValue = 5;
            A.CallTo(() => fakePaymentSystemClass.Balance()).Returns(paymentValue);
            A.CallTo(() => fakePaymentSystemClass.Vat()).Returns(vatValue);
            PriceCalculator fakePriceCalculator = A.Dummy<PriceCalculator>(); // Dummy
            BookingService sut = new BookingService(fakePaymentSystemClass, fakePriceCalculator); // user services instead

            // Act
            sut.Confirm3();

            // Assert
            A.CallTo(() => fakePaymentSystemClass.Pay(paymentValue + vatValue)).MustHaveHappenedOnceExactly(); // mock
            A.CallTo(() => fakePaymentSystemClass.Vat()).MustHaveHappenedOnceExactly(); // mock
            A.CallTo(() => fakePaymentSystemClass.Balance()).MustHaveHappenedOnceExactly(); // mock
        }
        
        [Fact]
        public void Booking_should_take_two_payments_with_diffrent_values()
        {
            // Arrange
            PaymentSystem fakePaymentSystemClass = A.Fake<PaymentSystem>(); // Fake 
            int paymentValue1 = 50;
            int vatValue1 = 3;
            int paymentValue2 = 65;
            int vatValue2 = 4;
            
            A.CallTo(() => fakePaymentSystemClass.Balance()).ReturnsNextFromSequence(paymentValue1, paymentValue2);
            A.CallTo(() => fakePaymentSystemClass.Vat()).ReturnsNextFromSequence(vatValue1, vatValue2);
            
            PriceCalculator fakePriceCalculator = A.Dummy<PriceCalculator>(); // Dummy
            BookingService sut = new BookingService(fakePaymentSystemClass, fakePriceCalculator); // user services instead
        
            // Act
            sut.Confirm4();
        
            // Assert
            A.CallTo(() => fakePaymentSystemClass.Pay(paymentValue1 + vatValue1)).MustHaveHappenedOnceExactly(); // mock
            A.CallTo(() => fakePaymentSystemClass.Pay(paymentValue2 + vatValue2)).MustHaveHappenedOnceExactly(); // mock
            A.CallTo(() => fakePaymentSystemClass.Vat()).MustHaveHappenedTwiceExactly(); // mock
            A.CallTo(() => fakePaymentSystemClass.Balance()).MustHaveHappenedTwiceExactly(); // mock
        }
        
        [Fact]
        public void Booking_should_use_the_amount_returned_by_the_price_calculator()
        {
            // Arrange
            PaymentSystem fakePaymentSystem = A.Fake<PaymentSystem>(); // Fake 
            PriceCalculator fakePriceCalculator = A.Fake<PriceCalculator>(); // Fake 
            
            A.CallTo(() => fakePaymentSystem.Balance()).Returns(50);
            A.CallTo(() => fakePriceCalculator.GeneratePrice(A<int>.Ignored, A<int>.Ignored))
                .Returns(50);
        
            BookingService sut = new BookingService(fakePaymentSystem, fakePriceCalculator);
        
            // Act
            sut.Confirm5();
        
            // Assert
            A.CallTo(() => fakePaymentSystem.Pay(50)).MustHaveHappenedOnceExactly(); // mock
            A.CallTo(() => fakePaymentSystem.Balance()).MustHaveHappenedOnceExactly(); // mock
        }
        
        [Fact]
        public void Booking_should_use_the_amount_returned_by_the_price_calculator_and_vat()
        {
            // Arrange
            PaymentSystem fakePaymentSystem = A.Fake<PaymentSystem>(); // Fake 
            PriceCalculator fakePriceCalculator = A.Fake<PriceCalculator>(); // Fake 
            
            A.CallTo(() => fakePaymentSystem.Balance()).Returns(50);
            A.CallTo(() => fakePaymentSystem.Vat()).Returns(5);
            int paymentTotal = 50 + 5;
            A.CallTo(() => fakePriceCalculator.GeneratePrice(A<int>.Ignored, A<int>.Ignored))
                .Returns(paymentTotal);
        
            BookingService sut = new BookingService(fakePaymentSystem, fakePriceCalculator);
        
            // Act
            sut.Confirm6();
        
            // Assert
            A.CallTo(() => fakePaymentSystem.Pay(paymentTotal)).MustHaveHappenedOnceExactly(); // mock
            A.CallTo(() => fakePaymentSystem.Vat()).MustHaveHappenedOnceExactly(); // mock
            A.CallTo(() => fakePaymentSystem.Balance()).MustHaveHappenedOnceExactly(); // mock
        }
        
        [Fact]
        public void Booking_should_use_the_amount_returned_by_the_price_calculator_twice_in_any_order()
        {
            // Arrange
            PaymentSystem fakePaymentSystem = A.Fake<PaymentSystem>(); // Fake 
            PriceCalculator fakePriceCalculator = A.Fake<PriceCalculator>(); // Fake 
            
            A.CallTo(() => fakePaymentSystem.Balance()).Returns(45);
            A.CallTo(() => fakePaymentSystem.Vat()).Returns(5);
            int paymentTotal = 45 + 5;
            A.CallTo(() => fakePriceCalculator.GeneratePrice(A<int>.Ignored, A<int>.Ignored))
                .Returns(paymentTotal);
        
            BookingService sut = new BookingService(fakePaymentSystem, fakePriceCalculator);
        
            // Act
            sut.Confirm7();
        
            // Assert
            A.CallTo(() => fakePaymentSystem.Pay(paymentTotal/2)).MustHaveHappenedTwiceExactly(); // mock
        }
        
        [Fact]
        public void Booking_should_use_both_amount_returned_by_the_price_calculator_twice_in_order()
        {
            // Arrange
            PaymentSystem fakePaymentSystem = A.Fake<PaymentSystem>(); // Fake 
            PriceCalculator fakePriceCalculator = A.Fake<PriceCalculator>(); // Fake 
            
            A.CallTo(() => fakePaymentSystem.Balance()).ReturnsNextFromSequence(50, 25);
            A.CallTo(() => fakePaymentSystem.Vat()).ReturnsNextFromSequence(2, 1);
            int firstPaymentTotal = 50 + 2;
            int secondPaymentTotal = 25 + 1;
            
            A.CallTo(() => fakePriceCalculator.GeneratePrice(A<int>.Ignored, A<int>.Ignored))
                .ReturnsNextFromSequence(firstPaymentTotal, secondPaymentTotal);

            BookingService sut = new BookingService(fakePaymentSystem, fakePriceCalculator);
        
            // Act
            sut.Confirm8();
        
            // Assert
            A.CallTo(() => fakePaymentSystem.Pay(firstPaymentTotal)).MustHaveHappenedOnceExactly().Then( // mock
            A.CallTo(() => fakePaymentSystem.Pay(secondPaymentTotal)).MustHaveHappenedOnceExactly()); // mock
        }
    }
}