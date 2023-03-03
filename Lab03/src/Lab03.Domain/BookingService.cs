
namespace Lab03.Domain
{
   public class BookingService
   {
      // payment should be injected instead
      private readonly IPaymentSystem _paymentSystem;
      public BookingService(IPaymentSystem paymentSystem)
      {
         _paymentSystem = paymentSystem;
      }
      
      public void Confirm1()
      {
         // does something with bank, maybe gets from db?
         _paymentSystem.Pay();
         // sends signal to save?
      }
      
      public void Confirm2()
      {
         // does something with bank
         if (_paymentSystem.Balance() == 50) // not important for test
         {
            _paymentSystem.Pay();
            _paymentSystem.Pay();
         }
      }
      
      public void Confirm3()
      {
         _paymentSystem.Pay();
      }
   }
}