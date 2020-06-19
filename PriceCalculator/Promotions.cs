using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator
{
    public class Promotions
    {
        private Products Products;
        private List<Promotion> PromotionList = new List<Promotion>();
        
        public Promotions(Products products)
        {
            Products = products;
            InitialisePromotions();
        }

        private void InitialisePromotions()
        {
            AddPromotion(new Promotion
            {
                Name = "Apples 10% off",
                StartDate = new DateTime(2020, 06, 16),
                EndDate = new DateTime(2020, 06, 21),
                Amount = new decimal(10),
                PromotionAppliedProduct = Products.GetProduct("Apples"),
                RequiredPromotionProduct = Products.GetProduct("Apples"),
                PromotionRequiredQuantity = 1,
                PromotionTypeEnum = PromotionTypeEnum.DiscountPercentage
            });
            
            AddPromotion(new Promotion
            {
                Name = "Buy 2 cans of Beans and get a loaf of bread for half price",
                StartDate = new DateTime(2020, 06, 16),
                EndDate = new DateTime(9999, 06, 21),
                Amount = new decimal(50),
                PromotionAppliedProduct = Products.GetProduct("Bread"),
                PromotionRequiredQuantity = 2,
                RequiredPromotionProduct = Products.GetProduct("Beans"),
                PromotionTypeEnum = PromotionTypeEnum.MultipleDiscount
            });

        }

        private void AddPromotion(Promotion promotion)
        {
            PromotionList.Add(promotion);
        }

        public List<Promotion> GetValidPromotions(DateTime promotionEffectiveDate)
        {
            return PromotionList
                .Where(x => x.StartDate <= promotionEffectiveDate && x.EndDate >= promotionEffectiveDate).ToList();
        }
    }
}