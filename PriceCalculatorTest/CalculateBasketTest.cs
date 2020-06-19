using System;
using NUnit.Framework;
using PriceCalculator;
using Shouldly;

namespace ShellPosTest
{
    [TestFixture]
    public class CalculateBasketTest
    {
        private Products Products;
        private Promotions Promotions;
        
        [OneTimeSetUp]
        public void SetUp()
        {
            Products = new Products();
            Promotions = new Promotions(Products);
        }
        [Test]
        public void CalculateBasketWithAppleDiscount()
        {
            var basket = new Basket();
            basket.AddBasketItem(Products.GetProduct("Apples"));
            basket.AddBasketItem(Products.GetProduct("Milk"));
            basket.AddBasketItem(Products.GetProduct("Bread"));
            
            var calculator = new BasketCalculator(Products, Promotions, basket);
            calculator.CalculateBasket(new DateTime(2020,6,18));
            
            calculator.SubTotal.ShouldBe(new decimal(3.10));
            calculator.DiscountsItems.Count.ShouldBe(1);
            calculator.DiscountsItems[0].Amount.ShouldBe(new decimal(.10));
            calculator.Total.ShouldBe(new decimal(3.00));
        }
        
        [Test]
        public void CalculateBasketWithOutAppleDiscountAsPromotionExpired()
        {
            var basket = new Basket();
            basket.AddBasketItem(Products.GetProduct("Apples"));
            basket.AddBasketItem(Products.GetProduct("Milk"));
            basket.AddBasketItem(Products.GetProduct("Bread"));
            
            var calculator = new BasketCalculator(Products, Promotions, basket);
            //Use an date in the future after promotion ended
            calculator.CalculateBasket(new DateTime(2021,6,18));
            
            calculator.SubTotal.ShouldBe(new decimal(3.10));
            calculator.DiscountsItems.Count.ShouldBe(0);
            calculator.Total.ShouldBe(new decimal(3.10));
        }

        [Test]
        public void CalculateBasketWithBeansAndBreadDiscount()
        {
            var basket = new Basket();
            basket.AddBasketItem(Products.GetProduct("Beans"));
            basket.AddBasketItem(Products.GetProduct("Beans"));
            basket.AddBasketItem(Products.GetProduct("Bread"));
            
            var calculator = new BasketCalculator(Products, Promotions, basket);
            calculator.CalculateBasket(new DateTime(2020,6,18));
            
            calculator.SubTotal.ShouldBe(new decimal(2.10));
            calculator.DiscountsItems.Count.ShouldBe(1);
            calculator.DiscountsItems[0].Amount.ShouldBe(new decimal(.40));
            calculator.Total.ShouldBe(new decimal(1.70));
        }
        
        [Test]
        public void CalculateBasketWithBeansAndBreadDiscountTwice()
        {
            var basket = new Basket();
            basket.AddBasketItem(Products.GetProduct("Beans"));
            basket.AddBasketItem(Products.GetProduct("Beans"));
            basket.AddBasketItem(Products.GetProduct("Bread"));
            basket.AddBasketItem(Products.GetProduct("Beans"));
            basket.AddBasketItem(Products.GetProduct("Beans"));
            basket.AddBasketItem(Products.GetProduct("Bread"));

            
            var calculator = new BasketCalculator(Products, Promotions, basket);
            calculator.CalculateBasket(new DateTime(2020,6,18));
            
            calculator.SubTotal.ShouldBe(new decimal(4.20));
            calculator.DiscountsItems.Count.ShouldBe(2);
            calculator.DiscountsItems[0].Amount.ShouldBe(new decimal(.40));
            calculator.DiscountsItems[0].Amount.ShouldBe(new decimal(.40));
            calculator.Total.ShouldBe(new decimal(3.40));
        }
        
        [Test]
        public void CalculateBasketWithBeansAndBreadDiscountOnyOnceWithFourBeans()
        {
            var basket = new Basket();
            basket.AddBasketItem(Products.GetProduct("Beans"));
            basket.AddBasketItem(Products.GetProduct("Beans"));
            basket.AddBasketItem(Products.GetProduct("Bread"));
            basket.AddBasketItem(Products.GetProduct("Beans"));
            basket.AddBasketItem(Products.GetProduct("Beans"));
         
            var calculator = new BasketCalculator(Products, Promotions, basket);
            calculator.CalculateBasket(new DateTime(2020,6,18));
            
            calculator.SubTotal.ShouldBe(new decimal(3.40));
            calculator.DiscountsItems.Count.ShouldBe(1);
            calculator.DiscountsItems[0].Amount.ShouldBe(new decimal(.40));
            calculator.Total.ShouldBe(new decimal(3.00));
        }
        
    }
}