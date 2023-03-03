using System;

namespace Lab03.Domain
{
    public interface IPaymentSystem
    {
        public void Pay();

        public int Balance();

        public int Vat();
    }
    public abstract class PaymentSystem : IPaymentSystem
    {
        public abstract void Pay();
        public abstract int Balance();
        public abstract int Vat();
    }
}