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
            //while (false)
            //{
            Console.WriteLine(store.GetLocations());
                Console.WriteLine("Which store location do you want to shop today?");
            //}
        }
    }
}