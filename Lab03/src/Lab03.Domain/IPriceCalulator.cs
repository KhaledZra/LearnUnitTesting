namespace Lab03.Domain
{
    public interface IPriceCalculator
    {
        public int GeneratePrice(int price, int vat);
    }

    public abstract class PriceCalculator : IPriceCalculator
    {
        public abstract int GeneratePrice(int price, int vat);
    }
}