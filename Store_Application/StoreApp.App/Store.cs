using StoreApp.DataInfrastructure;
using StoreApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.App
{
    public class Store
    {
        private readonly IRepository _repository;
        public Store(IRepository repository)
        {
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
            return locations.ToString();
        }
        public string GetStoreProducts(string locationID, out bool validID, out List<string> productList)
        {
            IEnumerable<Product> allRecords = _repository.GetStoreProducts(locationID);
            var products = new StringBuilder();
            productList = new();
            if (allRecords == null || !allRecords.Any())
            {
                products.AppendLine("--- Your Input is invalid, please try again. ---");
                validID = false;
            }
            else
            {
                validID = true;
                products.AppendLine($"ID\t\tProductName\t\t\tPrice");
                products.AppendLine("---------------------------------------------------------------");
                int i = 1;
                foreach (var record in allRecords)
                {
                    productList.Add(record.ProductName);
                    products.AppendLine(string.Format("{0,10}|{1,30}|{2,10}", i, record.ProductName, record.Price));
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
    }
}
