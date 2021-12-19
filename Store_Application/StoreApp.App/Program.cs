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
                Console.WriteLine(store.GetLocations());    // print store locations
                Console.WriteLine("Which store location do you want to shop today? ");
                Console.Write("Enter an ID number or Enter EXIT for Exit this Shop: ");
                string? locationID = Console.ReadLine();
                Console.WriteLine();
                if (locationID == null || locationID.Length <= 0)
                {
                    EmptyInputMessage();
                    continue;
                }
                if (locationID.ToLower() == "exit") break;
                // Print store products and get product list
                Console.WriteLine(store.GetStoreProducts(locationID, out bool validID, out List<string> productList));
                if (!validID) continue;
                else
                {
                NewCustomer:
                    Console.Write("Before you start shopping, are you a new customer? (y/n) ");
                    string? input = Console.ReadLine();
                    if (input == null || input.Length <= 0) { 
                        EmptyInputMessage(); 
                        goto NewCustomer; // jump to line name NewCustomer
                    }
                    if (input.ToLower() == "n")
                    {
                        Console.Write("Please Enter Your Customer ID#: ");
                        break;
                    }
                    else if (input.ToLower() == "y")
                    {
                    CreateAccount:
                        Console.WriteLine("To Create An New Acount, Please Enter Your Name.");
                        Console.Write("Enter Your First Name: ");
                        string? firstName = Console.ReadLine();
                        if (firstName == null || firstName.Length <= 0)
                        {
                            EmptyInputMessage();
                            goto CreateAccount; // jump to line name CreateAccount
                        }
                        Console.Write("Enter Your Last Name: ");
                        string? lastName = Console.ReadLine();
                        if (lastName == null || lastName.Length <= 0)
                        {
                            EmptyInputMessage();
                            goto CreateAccount; // jump to line name CreateAccount
                        }
                        int CustomerID = store.CreateAccount(firstName, lastName);
                        if (CustomerID < 0)
                        {
                            Console.WriteLine("Something When Wrong... Please Try Again.\n");
                            goto CreateAccount;  // jump to line name CreateAccount
                        }
                        Console.Write($"\nYour Account is Created Successfully!\n\n" +
                            $"Welcome to Stationery Shop, {firstName} {lastName}.\n" +
                            $"[ Your Customer ID#: {CustomerID} ]\n" +
                            $"[ Please Remember Your CustomerID For Later Login. ]\n");
                    }
                    else
                    {
                        EmptyInputMessage();
                        goto NewCustomer;
                    }
                PlaceOrder:
                    Console.Write("Choose the product you want to order.\nEnter Product ID#: ");
                    string? productID =  Console.ReadLine();
                    if(productID == null || productID.Length <= 0)
                    {
                        EmptyInputMessage();
                        goto PlaceOrder;
                    }
                    Console.WriteLine("Enter Product Quantity: ");
                    string? productAmount = Console.ReadLine();
                    if (productAmount == null || productAmount.Length <= 0)
                    {
                        EmptyInputMessage();
                        goto PlaceOrder;
                    }
                }
            }
        }

        public static void EmptyInputMessage()
        {
            Console.WriteLine("--- Your Input is invalid, please try again. ---\n");
        }
    }
}