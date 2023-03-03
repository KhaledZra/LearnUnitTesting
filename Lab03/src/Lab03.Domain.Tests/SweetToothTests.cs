using FluentAssertions;

namespace Lab03.Domain.Tests
{
    using FakeItEasy; // Named paramteters
    using Xunit; // any test framework will do

    public class SweetToothTests
    {
        [Fact]
        public void BuyTastiestCandy_should_buy_top_selling_candy_from_shop()
        {
            // make some fakes for the test
            var lollipop = A.Fake<ICandy>();
            var shop = A.Fake<ICandyShop>(); 
            //A.<datatype>.Ignored // use this to handle method input

            // set up a call to return a value // stubs should be part of arrange
            A.CallTo(() => shop.GetTopSellingCandy()).Returns(lollipop); // Stub because returns
            A.CallTo(() => shop.Address).Returns("123 Fake Street");

            // use the fake as an actual instance of the faked type
            var developer = new SweetTooth();
            developer.BuyTastiestCandy(shop);

            // asserting uses the exact same syntax as when configuring calls—
            // no need to learn another syntax
            A.CallTo(() => shop.BuyCandy(lollipop)).MustHaveHappened(); // mock because does something
            shop.Address.Should().Be("123 Fake Street");
        }

        public class SweetTooth
        {
            public void BuyTastiestCandy(ICandyShop shop)
            {
                shop.BuyCandy(shop.GetTopSellingCandy());
            }
        }

        public interface ICandyShop
        {
            public ICandy GetTopSellingCandy();
            public void BuyCandy(ICandy candy);
            string Address { get; set; }
        }

        public interface ICandy
        {
        }
    }
}
