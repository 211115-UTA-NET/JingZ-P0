using StoreApp.Logic;

namespace StoreApp.DataInfrastructure
{
    public interface IRepository
    {
        IEnumerable<Location> GetLocationList();
    }
}