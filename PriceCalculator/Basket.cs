using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator
{
    public class Basket
    {
        private List<BasketItem> BasketItems = new List<BasketItem>();
        
        public void AddBasketItem(Product product)
        {
            BasketItems.Add(new BasketItem{Product = product});
        }

        public List<decimal> PriceList
        {
            get { return BasketItems.Select(x => x.Product.Cost).ToList(); }
        }

        public List<int> ProductIdList
        {
            get { return BasketItems.Select(x => x.Product.Id).ToList(); }
        }

        public List<Product> ProductWithIds(List<int> ids)
        {
            return BasketItems.Where(x => ids.Contains(x.Product.Id)).Select(x => x.Product).ToList();
        }
    }
}