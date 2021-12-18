namespace StoreApp.Logic
{
    public class Location
    {
        public int LocationID { get; }
        public string StoreLocation { get; }
        public Location(int id, string location)
        {
            LocationID = id;
            StoreLocation = location;
        }
    }
}