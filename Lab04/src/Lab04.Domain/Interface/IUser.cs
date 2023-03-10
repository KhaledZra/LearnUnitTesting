namespace Lab04.Domain.Interface;

public interface IUser
{
    public string Name { get; set; }
    public void StartPaymentProcess(float payment);
}