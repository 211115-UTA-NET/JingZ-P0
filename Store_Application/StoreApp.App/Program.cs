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
                int CustomerID = CustomerLogin(store, out exitShop);
                if (exitShop) break;
                // -----------------------------------------
                int LocationID = StoreLocation(store, out exitShop);
                if (exitShop) break;
                /*
                 * Tasks:
                 * 1. add a loop for ordering
                 * 2. create a logic class to store user information: 
                 *      CustomerID, 
                 *      List<string> ProductName (data take from productList), 
                 *      List<int> ProductQty,
                 *      int LocationID
                 */
                Ordering:
                    Console.Write("Choose the product you want to order.\nEnter Product ID#: ");
                    string? productID =  Console.ReadLine();
                    if(productID == null || productID.Length <= 0)
                    {
                        EmptyInputMessage();
                        goto Ordering;
                    }
                    Console.Write("Enter Product Quantity: ");
                    string? productAmount = Console.ReadLine();
                    if (productAmount == null || productAmount.Length <= 0)
                    {
                        EmptyInputMessage();
                        goto Ordering;
                    }
                
            }
        }

        public static int CustomerLogin(Store store, out bool exit)
        {
            int CustomerID = -1;    // initialize
        NewCustomer:
            Console.Write("Before you start shopping, are you a new customer? (y/n) ");
            string? input = Console.ReadLine();
            // check if input is null/empty
            if (input == null || input.Length <= 0)
            {
                EmptyInputMessage();
                goto NewCustomer; // jump to line name NewCustomer
            }
            exit = ExitShop(input); // if user want exit shop
            if (exit) return CustomerID;
            // if not a new customer, login account
            if (input.ToLower() == "n")
            {
            Login:
                // Login User
                Console.Write("Please Enter Your Customer ID#: ");
                string? customerID = Console.ReadLine();
                if(customerID == null || customerID.Length <= 0)
                {
                    EmptyInputMessage();
                    goto Login;
                }
                bool customerExist = store.SearchCustomer(customerID);
                if (!customerExist) goto Login;
                CustomerID = int.Parse(customerID);  // store customer ID
            }
            else if (input.ToLower() == "y")    // new customer, add to database
            {
            CreateAccount:
                Console.WriteLine("To Create An New Acount, Please Enter Your Name.");
                Console.Write("Enter Your First Name: ");
                string? firstName = Console.ReadLine();
                // check if firstname is null/empty
                if (firstName == null || firstName.Length <= 0)
                {
                    EmptyInputMessage();
                    goto CreateAccount; // jump to line name CreateAccount
                }
                Console.Write("Enter Your Last Name: ");
                string? lastName = Console.ReadLine();
                // check if lastname is null/empty
                if (lastName == null || lastName.Length <= 0)
                {
                    EmptyInputMessage();
                    goto CreateAccount; // jump to line name CreateAccount
                }
                // create account
                CustomerID = store.CreateAccount(firstName, lastName);
                if (CustomerID < 0)
                {
                    Console.WriteLine("Something When Wrong... Please Try Again.\n");
                    goto CreateAccount;  // jump to line name CreateAccount
                }
                Console.Write($"\nYour Account is Created Successfully!\n\n" +
                    $"Welcome to Stationery Shop, {firstName} {lastName}.\n" +
                    $"-------------------------------------------------------" +
                    $"[ Your Customer ID#: {CustomerID} ]\n" +
                    $"[ Please Remember Your CustomerID For Later Login. ]\n\n");
            }
            else
            {
                EmptyInputMessage();
                goto NewCustomer;
            }
            return CustomerID;
        }

        public static int StoreLocation(Store store, out bool exit)
        {
        StoreLocations:
            Console.WriteLine(store.GetLocations());    // print store locations
            Console.WriteLine("Which store location do you want to shop today? ");
            Console.Write("Enter an ID number or Enter EXIT for Exit this Shop: ");
            string? locationID = Console.ReadLine();
            Console.WriteLine();
            if (locationID == null || locationID.Length <= 0)
            {
                EmptyInputMessage();
                goto StoreLocations;
            }
            exit = ExitShop(locationID); // if user want exit shop
            if (exit) return -1;
            // Print store products and get product list
            Console.WriteLine(store.GetStoreProducts(locationID, out bool validID, out List<string> productList));
            // invalidID go back to the top and try again
            if (!validID) goto StoreLocations;
            return int.Parse(locationID);
        }
        public static void EmptyInputMessage()
        {
            Console.WriteLine("--- Your Input is invalid, please try again. ---\n");
        }

        public static bool ExitShop(string userInput)
        {
            if (userInput.ToLower() == "exit")
            {
                Console.WriteLine("--- Thank you and bye. Looking forward your next visit. ---");
                return true;
            }
            return false;
        }
    }
}