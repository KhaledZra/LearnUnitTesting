using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using NodaTime;

namespace Lab02.Domain.Tests
{
    public class BookingTests
    {
        [Fact]
        public void Booking_must_have_start_time()
        {
            // Arrange
            Booking sut = new Booking("Khaled", "boras");
            
            // Act
            sut.SetStartTime(1);
            
            // Assert
            sut.StartTime.Should().NotBeNull();
        }
        
        [Fact]
        public void Booking_should_have_a_duration()
        {
            // Arrange
            Booking sut = new Booking("Khaled", "boras");
            
            // Act 
            sut.SetDuration(60); // Input: Minutes from now
            
            // Assert
            sut.Duration.Should().NotBeNull();
        }
        
        [Fact]
        public void Booking_duration_should_not_be_over_60_minutes()
        {
            // Arrange
            Booking sut = new Booking("Khaled", "boras");
            
            // Act 
            bool result = sut.SetDuration(61); // Input: Minutes from now
            
            // Assert
            result.Should().BeFalse();
        }
        
        [Fact]
        public void Booking_duration_should_be_at_least_15_minutes()
        {
            // Arrange
            Booking sut = new Booking("Khaled", "boras");
            
            // Act 
            bool result = sut.SetDuration(14); // Input: Minutes from now
            
            // Assert
            result.Should().BeFalse();
        }
        
        [Fact]
        public void Booking_should_have_price()
        {
            // Arrange
            Booking sut = new Booking("Khaled", "boras");
            
            // Act 
            sut.GeneratePrice();
            
            // Assert
            sut.Price.Should().NotBe(0);
        }
        
        [Fact]
        public void Booking_should_apply_custom_discount_for_companies()
        {
            // Arrange
            Booking sut = new Booking("Khaled", "boras", BookingDiscount.Companies);
            
            // Act 
            sut.GeneratePrice(50); // Start price is 50
            
            // Assert
            sut.Price.Should().Be(25);
        }
        
        [Fact]
        public void Booking_should_apply_discount_for_pensioners()
        {
            // Arrange
            Booking sut = new Booking("Khaled", "boras", BookingDiscount.Pensioners);
            
            // Act 
            sut.GeneratePrice(); // Start price is 50 + 20 tax
            
            // Assert
            sut.Price.Should().Be(20);
        }
        
        [Fact]
        public void Booking_should_apply_discount_for_teenagers()
        {
            // Arrange
            Booking sut = new Booking("Khaled", "boras", BookingDiscount.Teenagers);
            
            // Act 
            sut.GeneratePrice(); // Start price is 50 + 20 tax
            
            // Assert
            sut.Price.Should().Be(60);
        }
        
        [Fact]
        public void Booking_should_apply_discount_for_children()
        {
            // Arrange
            Booking sut = new Booking("Khaled", "boras", BookingDiscount.Children);
            
            // Act 
            sut.GeneratePrice(); // Start price is 50 + 20 tax
            
            // Assert
            sut.Price.Should().Be(45);
        }
        
        [Fact]
        public void Booking_should_be_associated_with_a_location()
        {
            // Arrange
            Booking sut = new Booking("Khaled", "boras");
            
            // Act 
            sut.SetLocation("Boras");
            
            // Assert
            sut.Location.Should().Be("Boras");
        }

        [Fact]
        public void User_must_be_able_to_cancel_booking()
        {
            // Arrange
            User sut = new User("Khaled",new List<Booking>());
            sut.AddBooking(new Booking(sut.GetName(), "boras"), 1);

            // Act
            sut.CancelBooking(0);

            // Assert
            sut.GetBookingActivityStatus(0).Should().BeFalse();
        }
        
        [Fact]
        public void Booking_name_should_be_user_and_location()
        {
            // Arrange
            User sut = new User("Khaled",new List<Booking>());
            
        
            // Act
            sut.AddBooking(new Booking(sut.GetName(), "boras"));

            // Assert
            sut.GetName().Should().Be(sut.GetName()+"");
        }
        
        [Fact]
        public void User_booking_if_successful_should_be_added_to_list()
        {
            // Arrange
            User sut = new User("Khaled",new List<Booking>());
        
            // Act
            sut.AddBooking(new Booking(sut.GetName(), "Boras"));
            sut.AddBooking(new Booking(sut.GetName(), "Boras"));
        
            // Assert
            sut.GetBookingListCount().Should().Be(2);
        }
        
        [Fact]
        public void User_should_not_be_able_to_cancel_one_hour_before_start()
        {
            // Arrange
            User sut = new User("Khaled",new List<Booking>());
            sut.AddBooking(new Booking(sut.GetName(), "Boras"));
            // Act
            var result = sut.CancelBooking(0);

            // Assert
            result.Should().BeFalse();
        }
        
        [Fact]
        public void Company_booking_name_should_be_name_and_reg_no_and_location()
        {
            // Arrange
            Company sut = new Company("KhaledCo", "101",new List<Booking>());
        
            // Act
            var result = new Booking(sut.GetName(), "Boras");

            // Assert
            result.Name.Should().BeEquivalentTo("KhaledCo101Boras");
        }
        
        [Fact]
        public void User_limit_recent_booking_to_two()
        {
            // Arrange
            User sut = new User("Khaled",new List<Booking>());
            sut.AddBooking(new Booking(sut.GetName(), "Boras"));
            sut.AddBooking(new Booking(sut.GetName(), "Boras"));
            sut.AddBooking(new Booking(sut.GetName(), "Boras"));
            sut.AddBooking(new Booking(sut.GetName(), "Boras"));
            sut.AddBooking(new Booking(sut.GetName(), "Boras"));
        
            // Act
            var recentLog = sut.GetBookingLog();

            // Assert
            recentLog.Count.Should().Be(2);
        }
    }
}