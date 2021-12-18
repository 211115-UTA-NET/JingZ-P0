using StoreApp.DataInfrastructure;

namespace StoreApp.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("[ Welcome to Stationery Shop ]");

            // Connection to database
            string connectionString = File.ReadAllText("D:/Study_Documents/Revature/Training/DBConnectionStrings/StoreDB.txt");
            IRepository repository = new SqlRepository(connectionString);
        }
    }
}