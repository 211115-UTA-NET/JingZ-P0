using StoreApp.Logic;

namespace StoreApp.DataInfrastructure
{
    public interface IRepository
    {
        IEnumerable<Location> GetLocationList();
        IEnumerable<Product> GetStoreProducts(string locationID);
        int AddNewCustomer(string firstName, string lastName);
        IEnumerable<Customer> FindCustomer(string customerID);
    }
}