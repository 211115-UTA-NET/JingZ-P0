using StoreApp.DataInfrastructure;
using StoreApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.App
{
    public class Store : OrderProcess
    {
        private readonly IRepository _repository;
        private List<string> ProductList;
        public Store(IRepository repository) : base(repository)
        {
            ProductList = new();
            _repository = repository;
        }
        public string GetLocations()
        {
            IEnumerable<Location> allRecords = _repository.GetLocationList();
            var locations = new StringBuilder();
            locations.AppendLine($"ID\tStore Location");
            locations.AppendLine("---------------------------------------------------------------");
            foreach (var record in allRecords)
            {
                locations.AppendLine($"{record.LocationID}\t[{record.StoreLocation}]");
            }
            locations.AppendLine("---------------------------------------------------------------");
            // get location always call before get store product.
            // So product list will initialize each time user want to switch a location
            ProductList = new(); 
            return locations.ToString();
        }
        public string GetStoreProducts(string locationID, out bool validID)
        {
            IEnumerable<Product> allRecords = _repository.GetStoreProducts(locationID);
            var products = new StringBuilder();
            if (allRecords == null || !allRecords.Any())
            {
                products.AppendLine("--- Your Input is invalid, please try again. ---");
                validID = false;
            }
            else
            {
                validID = true;
                products.AppendLine($"ID\t\tProduct Name\t\t\tPrice");
                products.AppendLine("---------------------------------------------------------------");
                int i = 1;
                foreach (var record in allRecords)
                {
                    // store ProductName
                    ProductList.Add(record.ProductName);
                    products.AppendLine(string.Format("{0,5} | {1,30} | {2,10}", i, record.ProductName, record.Price));
                    i++;
                }
                products.AppendLine("---------------------------------------------------------------");
            }
            
            return products.ToString();
        }

        public int CreateAccount(string firstName, string lastName)
        {
            int CustomerID = _repository.AddNewCustomer(firstName, lastName);
            return CustomerID;
        }

        public bool SearchCustomer(string customerID, out int CustomerID, string firstName ="", string lastName="")
        {
            IEnumerable<Customer> customer = _repository.FindCustomer(customerID, firstName, lastName);
            CustomerID = -1;
            if (customer == null || !customer.Any())
            {
                Console.WriteLine("--- Account Not Found. Please Try Again. ---");
                return false;
            }
            foreach (var existCustomer in customer)
            {
                Console.WriteLine($"\nWelcome Back! {existCustomer.FirstName} {existCustomer.LastName}.\n");
                CustomerID = existCustomer.CustomerID;
            }
            return true;
        }

        public bool ValidProductID(string productID, out string ProductName)
        {
            if (int.TryParse(productID, out int productId))
            {
                productId -= 1; // used as List index
                if (productId >= 0 && ProductList.Count > productId)
                {
                    ProductName = ProductList[productId];
                    return true;
                }
            }
            ProductName = "";
            return false;
        }

        /// <summary>
        ///     Check if user select product quantity is valid or not.
        /// </summary>
        /// <param name="productName">User selected product (valid)</param>
        /// <param name="amount">User input amount</param>
        /// <param name="locationID">Valid location ID</param>
        /// <param name="orderAmount">return the valid order amount</param>
        /// <returns>true if amount is valid, false otherwise.</returns>
        public bool validAmount(string productName, string amount, int locationID, out int orderAmount)
        {
            // amount <= inventory amount
            if (int.TryParse(amount, out orderAmount))
            {
                if (orderAmount >= 100 || orderAmount == 0) {
                    Console.WriteLine("\n--- Quantity cannot be 0 and cannot exceed the Max limit. ---");
                    return false;
                } // cannot order more than 99
                int inventoryAmount = _repository.InventoryAmount(productName, locationID);
                // Console.WriteLine("inventory amount: " + inventoryAmount);
                if (orderAmount <= inventoryAmount)
                {
                    return true;
                }
                else Console.WriteLine("\n--- Sorry, this Product is OUT of STOCK... Please select another product. ---");
            }
            return false;
        }
    }
}
