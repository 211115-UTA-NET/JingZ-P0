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
        public int CustomerID { get; }
        List<string> ProductName { get; }
        List<int> ProductQty { get; }
        public Order(int orderNum, int customerID, List<string> productName, List<int> productQty)
        {
            OrderNum = orderNum;
            CustomerID = customerID;
            ProductName = productName;
            ProductQty = productQty;
        }
    }
}
