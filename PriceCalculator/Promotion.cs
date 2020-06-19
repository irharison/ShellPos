using System;

namespace PriceCalculator
{
    public class Promotion
    {
        public string Name { get; set; }
        public PromotionTypeEnum PromotionTypeEnum;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public Product RequiredPromotionProduct { get; set; }
        public int PromotionRequiredQuantity { get; set; }
        public Product PromotionAppliedProduct { get; set; } 
    }
}