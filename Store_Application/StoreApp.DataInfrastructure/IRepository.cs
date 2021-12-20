using StoreApp.Logic;

namespace StoreApp.DataInfrastructure
{
    public interface IRepository
    {
        IEnumerable<Location> GetLocationList();
        IEnumerable<Product> GetStoreProducts(string locationID);
        int AddNewCustomer(string firstName, string lastName);
        IEnumerable<Customer> FindCustomer(string customerID);
        int InventoryAmount(string productName, int locationID);
        int GetOrderNumber(int customerID);
        IEnumerable<Order> AddOrder(List<Order> order);
        List<decimal> GetPrice(List<Order> order);
    }
}