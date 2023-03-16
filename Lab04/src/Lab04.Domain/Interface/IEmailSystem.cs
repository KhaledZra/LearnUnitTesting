using Lab04.Domain.Model;

namespace Lab04.Domain.Interface;

public interface IEmailSystem
{
    public void SendBookingInformationEmail(BookingDocument bookingDocument);
    public void SendFailedPaymentEmail(BookingDocument bookingDocument);
    public void SendSuccessfulBookingCancellationEmail(BookingDocument bookingDocument);
    public void SendFailedBookingCancellationEmail(BookingDocument bookingDocument, string reasonMessage);
}