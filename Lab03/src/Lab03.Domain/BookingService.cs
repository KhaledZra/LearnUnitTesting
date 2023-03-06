
namespace Lab03.Domain
{
   public class BookingService
   {
      // payment should be injected instead
      private readonly IPaymentSystem _paymentSystem;
      private readonly IPriceCalculator _priceCalculatorSystem;
      public BookingService(IPaymentSystem paymentSystem, IPriceCalculator priceCalculatorSystem)
      {
         _paymentSystem = paymentSystem;
         _priceCalculatorSystem = priceCalculatorSystem;
      }
      
      public void Confirm1()
      {
         // does something with bank, maybe gets from db?
         _paymentSystem.Pay(_paymentSystem.Balance());
         // sends signal to save?
      }
      
      public void Confirm2()
      {
         // does something with bank
         _paymentSystem.Pay(_paymentSystem.Balance());
         _paymentSystem.Pay(_paymentSystem.Balance());
      }
      
      public void Confirm3()
      {
         _paymentSystem.Pay(_paymentSystem.Balance() + _paymentSystem.Vat());
         
      }

      public void Confirm4()
      {
         _paymentSystem.Pay(_paymentSystem.Balance() + _paymentSystem.Vat());
         _paymentSystem.Pay(_paymentSystem.Balance() + _paymentSystem.Vat());
      }

      public void Confirm5()
      {
         _paymentSystem.Pay(
            _priceCalculatorSystem.GeneratePrice(
                     _paymentSystem.Balance(), 0));
      }

      public void Confirm6()
      {
         _paymentSystem.Pay(
            _priceCalculatorSystem.GeneratePrice(
               _paymentSystem.Balance(), 
               _paymentSystem.Vat()));
      }

      public void Confirm7()
      {
         int generatedPrice = _priceCalculatorSystem.GeneratePrice(
            _paymentSystem.Balance(),
            _paymentSystem.Vat());
         
         // First installment
         _paymentSystem.Pay(generatedPrice / 2);
         
         // Second installment
         _paymentSystem.Pay(generatedPrice / 2);

      }

      public void Confirm8()
      {
         int firstGeneratedPrice = _priceCalculatorSystem.GeneratePrice(
            _paymentSystem.Balance(),
            _paymentSystem.Vat());
         
         int secondGeneratedPrice = _priceCalculatorSystem.GeneratePrice(
            _paymentSystem.Balance(),
            _paymentSystem.Vat());
         
         // First installment
         _paymentSystem.Pay(firstGeneratedPrice);
         
         // Second installment
         _paymentSystem.Pay(secondGeneratedPrice);
      }
   }
}