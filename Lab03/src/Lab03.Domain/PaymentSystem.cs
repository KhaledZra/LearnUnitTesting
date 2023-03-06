using System;

namespace Lab03.Domain
{
    public interface IPaymentSystem
    {
        public void Pay(int amount);

        public int Balance();

        public int Vat();
    }
    public abstract class PaymentSystem : IPaymentSystem
    {
        public abstract void Pay(int amount);
        public abstract int Balance();
        public abstract int Vat();
    }
}