using System.Collections.Generic;
using System.Linq;

namespace PriceCalculator
{
    public class Products
    {
        private List<Product> ProductList = new List<Product>();

        public Products()
        {
            InitialiseProducts();
        }
        //This would normally be read from a database, but hard coding for the 
        //purposes of this exercise
        private void InitialiseProducts()
        {
            AddProduct(new Product{Id = 1, Name = "Beans", Cost = new decimal(0.65)});
            AddProduct(new Product{Id = 2, Name = "Bread", Cost = new decimal(0.80)});
            AddProduct(new Product{Id = 3, Name = "Milk", Cost = new decimal(1.30)});
            AddProduct(new Product{Id = 4, Name = "Apples", Cost = new decimal(1.00)});
        }
        public void AddProduct(Product product)
        {
            ProductList.Add(product);
        }

        public Product GetProduct(string name)
        {
            return ProductList.FirstOrDefault(x => x.Name == name);
        }

        public List<Product> GetProductListFromIds(List<int> ids)
        {
            return ProductList.Where(x => ids.Contains(x.Id)).ToList();
        }

    }
}