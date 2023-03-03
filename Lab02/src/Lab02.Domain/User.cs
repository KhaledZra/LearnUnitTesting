using System;
using System.Collections.Generic;
using NodaTime;

namespace Lab02.Domain;

public class User : IBooker
{
    
    
    private readonly string _name;
    private readonly List<Booking> _bookings;
    private readonly List<Booking> _bookingsLog;

    public User(string name, List<Booking> booking)
    {
        this._name = name;
        this._bookings = booking;
        _bookingsLog = new List<Booking>();
        
        
    }

    public string GetName()
    {
        return _name;
    }

    public void AddBooking(Booking booking, int delayByDays = 0)
    {
        booking.SetStartTime(delayByDays);
        _bookings.Add(booking);
        LogBooking(booking);
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
        _bookingsLog.Add(booking);
        if (_bookings.Count == 3) // limit to recent to 2 bookings
        {
            _bookings.RemoveAt(0);
        }
    }

    public IReadOnlyCollection<Booking> GetBookingLog()
    {
        return _bookings.AsReadOnly();
    }
}