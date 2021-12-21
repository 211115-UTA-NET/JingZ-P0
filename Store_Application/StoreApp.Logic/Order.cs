namespace StoreApp.Logic
{
    public class Order
    {
        /*
         *      OrderNum ?
         *      CustomerID, 
         *      List<string> ProductName (data take from productList), 
         *      List<int> ProductQty,
         *      int LocationID
         */
        public int OrderNum { get; }
        public string ProductName { get; }
        public int ProductQty { get; }
        public int LocationID { get; }
        public DateTimeOffset? OrderDate { get; }

        public string Location {get;}
        public Order(int orderNum, string productName, int productQty, int locationID, DateTimeOffset? date, string loaction = "")
        {
            OrderNum = orderNum;
            ProductName = productName;
            ProductQty = productQty;
            LocationID = locationID;
            OrderDate = date;
            Location = loaction;
        }
    }
}
