using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator
{
    public class BasketCalculator
    {
        private Products Products;
        private Promotions Promotions;
        private Basket Basket;
        
        
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        
        public List<DiscountItem> DiscountsItems = new List<DiscountItem>();

        private List<Product> PromotionRequiredProducts;
        private List<Product> PromotionAppliedProducts;

        public BasketCalculator(Products products, Promotions promotions, Basket basket)
        {
            Products = products;
            Promotions = promotions;
            Basket = basket;
        }


        public void CalculateBasket(DateTime promotionEffectiveDate)
        {
            SubTotal = Basket.PriceList.Sum();
            ApplyPromotion(promotionEffectiveDate);
            Total = SubTotal - DiscountsItems.Sum(x => x.Amount);
        }

        private void ApplyPromotion(DateTime promotionEffectiveDate)
        {
            //Get a list of promotions that are valid for this week
            var validPromotions = Promotions.GetValidPromotions(promotionEffectiveDate).ToList();

            PromotionRequiredProducts = 
                Basket.ProductWithIds(validPromotions.Select(x => x.RequiredPromotionProduct.Id).ToList());

            PromotionAppliedProducts = 
                Basket.ProductWithIds(validPromotions.Select(x => x.PromotionAppliedProduct.Id).ToList());
            
            if (PromotionRequiredProducts.Count > 0)
            {
                ApplyDiscountPromotions(validPromotions);
                if (PromotionRequiredProducts.Count > 0)
                {
                    MultipleDiscount(validPromotions);
                }
            }
        }

        private void MultipleDiscount(List<Promotion> validPromotions)
        {
            var multipleDiscountPromotions = validPromotions.Where(
                x => x.PromotionTypeEnum == PromotionTypeEnum.MultipleDiscount
                     && PromotionRequiredProducts.Select(y=>y.Id).ToList().Contains(x.RequiredPromotionProduct.Id)).ToList();

            foreach (var promotion in multipleDiscountPromotions)
            {
                //Check if we have at least the required number of products
                var promotionProductCount = PromotionRequiredProducts.Count(x => x.Id == promotion.RequiredPromotionProduct.Id);
                
                while (promotionProductCount >= promotion.PromotionRequiredQuantity)
                {
                    //Remove the promotion count number of products from the promotion required product list
                    for (int count = 0; count < promotion.PromotionRequiredQuantity; count++)
                    {
                        PromotionRequiredProducts.Remove(promotion.RequiredPromotionProduct);
                    }
                    
                    var promotionAppliedProduct = PromotionAppliedProducts
                        .Where(x => x.Id == promotion.PromotionAppliedProduct.Id).FirstOrDefault();
                    
                    //If we have something to apply to promotion too, then add discount line and remove 
                    //the product to be applied too
                    if (promotionAppliedProduct != null)
                    {
                        PromotionAppliedProducts.Remove(promotionAppliedProduct);
                        DiscountsItems.Add(new DiscountItem(promotion, promotionAppliedProduct.Cost * promotion.Amount / 100));
                    }
                    //If we don't have any products to apply the promotion too then don't do anything and move onto the next
                    //promotion
                    else
                    {
                        break;
                    }
                    
                    //We may be able to apply the promotion multiple times, so check for further products
                    promotionProductCount = PromotionRequiredProducts.Count(x => x.Id == promotion.RequiredPromotionProduct.Id);
                }
            }
        }

        private void ApplyDiscountPromotions(List<Promotion> validPromotions)
        {
            var discountPromotions = validPromotions.Where(
                x => x.PromotionTypeEnum == PromotionTypeEnum.DiscountPercentage
                     && PromotionRequiredProducts.Select(y=>y.Id).ToList().Contains(x.RequiredPromotionProduct.Id)).ToList();

            foreach (var promotion in discountPromotions)
            {
                var products = PromotionRequiredProducts.Where(x => x.Id == promotion.RequiredPromotionProduct.Id).ToList();
                foreach (var product in products)
                {
                    //Discount is the cost of the product multipled by the discount amount
                    DiscountsItems.Add(new DiscountItem(promotion, product.Cost * promotion.Amount / 100));
                    PromotionRequiredProducts.Remove(product);
                }
            }
        }
    }
}