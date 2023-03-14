using Lab04.Domain.Model;

namespace Lab04.Domain.Interface;

public interface IEmailSystem
{
    public void SendSuccessfulEmail(BookingDocument bookingDocument);
}