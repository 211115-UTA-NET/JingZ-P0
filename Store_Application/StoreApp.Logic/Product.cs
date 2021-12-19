namespace StoreApp.Logic
{
    public class Product
    {
        public string ProductName { get; }
        public decimal Price { get; }
        public Product(string productName, decimal price)
        {
            ProductName = productName;
            Price = price;
        }
    }
}
