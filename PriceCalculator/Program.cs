using System;

namespace PriceCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var products = new Products();
            var promotions = new Promotions(products);
            var basket = new Basket();

            bool invalidProduct = false;
            
            foreach (var arg in args)
            {
                var product = products.GetProduct(arg);
                
                if (product == null)
                {
                    Console.WriteLine($"Unknown product {arg}");
                    invalidProduct = true;
                }
                else
                {
                    basket.AddBasketItem(product);
                }
            }

            if (!invalidProduct)
            {
                var basketCalculator = new BasketCalculator(products, promotions, basket);
                //The effective date woudl normally be rolling.  However thi won't work if 
                //you check this code next week, hence hard coded here.
                basketCalculator.CalculateBasket(new DateTime(2020,06,18));
                Console.WriteLine($"Subtotal: £{basketCalculator.SubTotal}");
                if (basketCalculator.DiscountsItems.Count == 0)
                {
                    Console.WriteLine("(No Offers Available)");
                }
                else
                {
                    foreach (var discountItem in basketCalculator.DiscountsItems)
                    {
                        Console.WriteLine($"{discountItem.Description}: " + AmountWithPoundsAndPence(discountItem.Amount));
                    }
                }

                Console.WriteLine($"Total Price: " + AmountWithPoundsAndPence(basketCalculator.Total));
            }
        }

        public static string AmountWithPoundsAndPence(decimal amount)
        {
            if (amount > 1)
            {
                return $"£{amount}";
            }
            else
            {
                return $"{amount*100:G29}p";
            }
        }
    }
}