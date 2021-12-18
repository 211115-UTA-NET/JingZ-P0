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
    }
}
