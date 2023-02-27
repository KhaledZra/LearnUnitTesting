using System.Collections.Generic;

namespace Lab02.Domain;

public interface IBooker
{
    public string GetName();
    public void AddBooking(Booking booking, int delayByDays = 0);

    public bool CancelBooking(int index);

    public bool GetBookingActivityStatus(int index);

    public int GetBookingListCount();

    public void LogBooking(Booking booking);

    public IReadOnlyCollection<Booking> GetBookingLog();
}