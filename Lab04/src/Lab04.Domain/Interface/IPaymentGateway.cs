namespace Lab04.Domain.Interface;

public interface IPaymentGateway
{
    public void SendPayment(float payment);

    public void RequestPaymentRefund(int bookingId, float amount);
}