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
                // USER LOGIN SECTION
                int CustomerID = CustomerLogin(store, out exitShop);
                if (exitShop) break;
                // STORE INFORMATION SECTION
                int LocationID = StoreLocation(store, out exitShop);
                if (exitShop) break;
                // MENU SECTION
            MenuSelection:
                Console.WriteLine("\nMenu Options:" +
                    "1: Make a order" +
                    "2: Display all order history of this store location" +
                    "3: Display all order histroy of Stationery Shop" +
                    "Exit: Exit The Store.");
                string? menuSelection = Console.ReadLine();
                if (CheckEmptyInput(menuSelection, out menuSelection)) goto MenuSelection;
                if(ExitShop(menuSelection)) break; // if user want exit shop
                if (menuSelection == "1")
                {
                    // ORDERING SECTION
                    Ordering();
                }
                else if(menuSelection == "2")
                {
                    // display all order history of this store location
                }
                else if(menuSelection == "3")
                {
                    // display all order history of Stationery Shop
                }
                else
                {
                    EmptyInputMessage();
                    goto MenuSelection;
                }

            }
        }
        /// <summary>
        ///     Customer login section. 
        ///     If user is new customer, then create a new account and print the customer ID to user. 
        ///     If user has an account, then ask user to login and store the customer ID.
        /// </summary>
        /// <param name="store">A Store type class</param>
        /// <param name="exit">A boolean that checks if user want to exit the shop</param>
        /// <returns>int Customer ID</returns>
        public static int CustomerLogin(Store store, out bool exit)
        {
            int CustomerID = -1;    // initialize
        NewCustomer:
            Console.Write("Before you start shopping, are you a new customer? (y/n) ");
            string? input = Console.ReadLine();
            // check if input is null/empty
            if (CheckEmptyInput(input, out input)) goto NewCustomer;

            exit = ExitShop(input); // if user want exit shop
            if (exit) return CustomerID;
            // if not a new customer, login account
            if (input.ToLower() == "n")
            {
            Login:
                // Login User
                Console.Write("Please Enter Your Customer ID#: ");
                string? customerID = Console.ReadLine();
                if (CheckEmptyInput(customerID, out customerID)) goto Login;

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
                if (CheckEmptyInput(firstName, out firstName)) goto CreateAccount;

                Console.Write("Enter Your Last Name: ");
                string? lastName = Console.ReadLine();
                // check if lastname is null/empty
                if (CheckEmptyInput(lastName, out lastName)) goto CreateAccount;

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
        /// <summary>
        ///     Store Information section.
        ///     Print all the store location and ask user to pick a choice.
        ///     After user select a store location, display all products in the store loaction.
        /// </summary>
        /// <param name="store">A Store type class</param>
        /// <param name="exit">A boolean that checks if user want to exit the shop</param>
        /// <returns>The location ID that User selected</returns>
        public static int StoreLocation(Store store, out bool exit)
        {
        StoreLocations:
            Console.WriteLine(store.GetLocations());    // print store locations
            Console.WriteLine("Which store location do you want to shop today? ");
            Console.Write("Enter an ID number or Enter EXIT for Exit this Shop: ");
            string? locationID = Console.ReadLine();
            Console.WriteLine();
            if (CheckEmptyInput(locationID, out locationID)) goto StoreLocations;

            exit = ExitShop(locationID); // if user want exit shop
            if (exit) return -1;
            // Print store products and get product list
            Console.WriteLine(store.GetStoreProducts(locationID, out bool validID, out List<string> productList));
            // invalidID go back to the top and try again
            if (!validID) goto StoreLocations;
            return int.Parse(locationID);
        }

        public static void Ordering()
        {
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
            string? productID = Console.ReadLine();
            if (CheckEmptyInput(productID, out productID)) goto Ordering;

            Console.Write("Enter Product Quantity: ");
            string? productAmount = Console.ReadLine();
            if (CheckEmptyInput(productAmount, out productAmount)) goto Ordering;
        }
        /// <summary>
        ///     Printing empty input error message.
        /// </summary>
        public static void EmptyInputMessage()
        {
            Console.WriteLine("--- Your Input is invalid, please try again. ---\n");
        }
        /// <summary>
        ///     Check if user input is null or empty
        /// </summary>
        /// <param name="userInput">User Input string, can be null</param>
        /// <param name="userNotNullInput">Not null User input string</param>
        /// <returns>true if input is null/empty, false otherwise.</returns>
        public static bool CheckEmptyInput(string? userInput, out string userNotNullInput)
        {
            if (userInput == null || userInput.Length <= 0)
            {
                EmptyInputMessage();
                userNotNullInput = "";
                return true;
            }
            userNotNullInput = userInput;
            return false;
        }
        /// <summary>
        ///     Check if user input is "exit".
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns>true if user want to exit store. false otherwise.</returns>
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