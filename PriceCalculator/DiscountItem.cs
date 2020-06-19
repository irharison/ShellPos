namespace PriceCalculator
{
    public class DiscountItem
    {
        public string Description
        {
            get { return Promotion.Name; }
        }
        public decimal Amount { get; set; }

        //Would normally put this in the view model,but a simple solution.
        
        private Promotion Promotion { get; set; }

        public DiscountItem(Promotion promotion, decimal amount)
        {
            Promotion = promotion;
            Amount = amount;
        }
    }
}