using StoreApp.DataInfrastructure;

namespace StoreApp.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("[ Welcome to Stationery Shop ]\n");

            // Connection to database
            string connectionString = File.ReadAllText("D:/Study_Documents/Revature/Training/DBConnectionStrings/StoreDB.txt");
            IRepository repository = new SqlRepository(connectionString);
            Store store = new(repository);
            bool exitShop = false;
            while (!exitShop)
            {
                Console.WriteLine(store.GetLocations());
                Console.WriteLine("Which store location do you want to shop today? " +
                    "\n(Enter an ID number or Enter EXIT for Exit this Shop)");
                string? locationID = Console.ReadLine();
                if (locationID == null || locationID.Length <= 0)
                {
                    Console.WriteLine("--- Please Choose A Option! ---\n");
                    continue;
                }
                if (locationID.ToUpper() == "EXIT") break;
                Console.WriteLine(store.GetStoreProducts(locationID, out bool validID));
                if (!validID) continue;
                else
                {
                    Console.WriteLine("Are you a new customer?");
                    break;
                }
            }
        }


    }
}