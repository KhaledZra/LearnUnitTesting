using System;
using System.Collections.Generic;
using NodaTime;

namespace Lab02.Domain;

public class Company : IBooker
{
    private readonly string _name;
    private readonly string _companyRegNo;
    private readonly List<Booking> _bookings;

    public Company(string name, string companyRegNo, List<Booking> booking)
    {
        this._name = name;
        this._companyRegNo = companyRegNo;
        this._bookings = booking;
    }

    public string GetName()
    {
        return _name + _companyRegNo;
    }

    public void AddBooking(Booking booking, int delayByDays = 0)
    {
        booking.SetStartTime(delayByDays);
        _bookings.Add(booking);
    }

    public bool CancelBooking(int index)
    {
        LocalDate hourCheck = LocalDate.FromDateTime(DateTime.Now.AddHours(1));
        
        if (_bookings[index].StartTime > hourCheck)
        {
            _bookings[index].IsActive = false;
            return true;
        }

        return false;
    }

    public bool GetBookingActivityStatus(int index)
    {
        return _bookings[index].IsActive;
    }

    public int GetBookingListCount()
    {
        return _bookings.Count;
    }
    
    public void LogBooking(Booking booking)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<Booking> GetBookingLog()
    {
        throw new NotImplementedException();
    }
}