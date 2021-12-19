namespace StoreApp.Logic
{
    public class Customer
    {
        public int CustomerID { get; }
        public string LastName { get; }
        public string FirstName { get; }
        public Customer(int customerID, string firstName, string lastName)
        {
            CustomerID = customerID;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
