using System;
using System.Runtime.InteropServices.JavaScript;
using NodaTime;

namespace Lab02.Domain;

public enum BookingDiscount
{
    None,
    Companies,
    Pensioners,
    Teenagers,
    Children
}

public class Booking
{
    public string Name { get; private set; }
    public LocalDate StartTime { get; private set; }
    public LocalTime Duration { get; private set; }
    public float Price { get; private set; }
    public string Location { get; private set; }
    public bool IsActive { get; set; }
    
    private readonly BookingDiscount _bookingDiscount;
    private readonly float basePrice = 50.0f;
    private readonly float vatBasePrice = 20.0f;
    

    public Booking(string userName, string location, BookingDiscount bookingDiscount = BookingDiscount.None)
    {
        Location = location;
        Name = userName + location;
        _bookingDiscount = bookingDiscount;
        IsActive = true;
    }

    // Public methods
    public void SetName(string userName)
    {
        Name = Location + userName;
    }
    public void SetLocation(string location)
    {
        Location = location;
    }
    public void SetStartTime(int daysFromNow = 0)
    {
        StartTime = LocalDate.FromDateTime(DateTime.Now.AddDays(daysFromNow)); 
    }
    
    public bool SetDuration(int durationLengthInMinutes)
    {
        if (durationLengthInMinutes > 60) return false;
        if (durationLengthInMinutes < 15) return false;

        Duration = LocalTime
            .FromTimeOnly(
                TimeOnly.FromDateTime(
                    DateTime.Now.AddMinutes(durationLengthInMinutes)));
        return true;
    }

    public void GeneratePrice(float customDiscountModifier = 0)
    {
        Price = basePrice;
        ApplyDiscount(customDiscountModifier);
        if (_bookingDiscount != BookingDiscount.Companies)
        {
            Price += vatBasePrice;
        }
    }

    // Private methods
    private void ApplyDiscount(float customDiscountModifier = 0)
    {
        if (_bookingDiscount == BookingDiscount.None) ;
        else if (_bookingDiscount == BookingDiscount.Companies) CalculateDiscount(customDiscountModifier);
        else if (_bookingDiscount == BookingDiscount.Pensioners) CalculateDiscount(0);
        else if (_bookingDiscount == BookingDiscount.Teenagers) CalculateDiscount(80);
        else if (_bookingDiscount == BookingDiscount.Children) CalculateDiscount(50);
    }

    private void CalculateDiscount(float discountValueProcentage)
    {
        discountValueProcentage /= 100;
        Price *= discountValueProcentage;
    }
}