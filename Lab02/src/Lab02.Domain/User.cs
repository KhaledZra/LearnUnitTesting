using System.Collections.Generic;

namespace Lab02.Domain;

public class User
{
    public string Name { get; private set; }
    private readonly List<Booking> _bookings;

    public User(string name, List<Booking> booking)
    {
        this.Name = name;
        this._bookings = booking;
    }

    public void AddBooking(Booking booking)
    {
        _bookings.Add(booking);
    }

    public void CancelBooking(int index)
    {
        _bookings[index].IsActive = false;
    }

    public bool GetBookingActivityStatus(int index)
    {
        return _bookings[index].IsActive;
    }

    public int GetBookingListCount()
    {
        return _bookings.Count;
    }
}