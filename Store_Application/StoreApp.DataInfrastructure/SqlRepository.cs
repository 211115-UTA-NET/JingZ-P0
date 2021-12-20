using StoreApp.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.DataInfrastructure
{
    public class SqlRepository : IRepository
    {
        private readonly string _connectionString;

        public SqlRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        ///     A method takes a query parameters and returns a DataSet type result. 
        ///     Only for read only database connection. (ex. SELECT)
        ///     Notes: colnames parameter 
        /// </summary>
        /// <param name="query">Database query command</param>
        /// <param name="paramName">parameter name in your query (ex. "...WHERE ID = @id" paramName = "id")</param>
        /// <param name="inputVal">the value for the query search condition</param>
        /// <returns>DataSet type</returns>
        private DataSet DBReadOnly(string? query, string[]? paramName = null, object[]? inputVal = null)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();
            using SqlCommand command = new(query, connection);
            if (paramName != null && inputVal != null)
            {
                for (int i = 0; i < inputVal.Length; i++)
                {
                    command.Parameters.AddWithValue($"@{paramName[i]}", inputVal[i]);
                }
            }
            using SqlDataAdapter adapter = new(command);
            DataSet dataSet = new();
            adapter.Fill(dataSet);
            connection.Close();
            return dataSet;
        }

        private int DBWriteOnly(string? query, string[]? paramName = null, object[]? inputVal = null)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();
            using SqlCommand command = new(query, connection);
            if (paramName != null && inputVal != null)
            {
                for (int i = 0; i < inputVal.Length; i++)
                {
                    command.Parameters.AddWithValue($"@{paramName[i]}", inputVal[i]);
                }
            }
            int rowsAffected = command.ExecuteNonQuery();
            connection.Close();
            return rowsAffected;
        }

        public IEnumerable<Location> GetLocationList()
        {
            List<Location> result = new();
            DataSet dataSet = DBReadOnly("SELECT * FROM Location");
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                result.Add(new((int)row["ID"], (string)row["StoreLocation"]));
            }
            return result;
        }
        public IEnumerable<Product> GetStoreProducts(string locationID)
        {
            List<Product> result = new();
            bool isInt = int.TryParse(locationID, out int locId);
            if (isInt)
            {
                DataSet dataSet = DBReadOnly("SELECT ProductName, Price FROM StoreInventory WHERE LocationID = @locID ORDER BY Price",
                    new string[] { "locID" },
                    new object[] { locId });
                foreach(DataRow row in dataSet.Tables[0].Rows)
                {
                    result.Add(new((string)row["ProductName"], (decimal)row["Price"]));
                }

            }
            // if locationID is not int then return empty List
            return result;
        }

        public int AddNewCustomer(string firstName, string lastName)
        {
            int result = DBWriteOnly("INSERT Customer VALUES (@firstName, @lastName);",
                new string[] { "firstName", "lastName" },
                new object[] { firstName, lastName });
            if (result > 0)
            {
                DataSet customerID = DBReadOnly("SELECT ID FROM Customer WHERE FirstName=@firstName AND LastName=@lastName",
                    new string[] { "firstName", "lastName" },
                    new object[] { firstName, lastName });
                return (int)customerID.Tables[0].Rows[0]["ID"];
            }
            else return -1;
        }
        public IEnumerable<Customer> FindCustomer(string customerID)
        {
            List<Customer> customer = new();
            bool isInt = int.TryParse(customerID, out int CustomerID);
            if (isInt) {
                DataSet dataSet = DBReadOnly("SELECT * FROM Customer WHERE ID = @CustomerID;",
                    new string[] { "CustomerID" },
                    new object[] { CustomerID });
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    customer.Add(new((int)row["ID"], (string)row["FirstName"], (string)row["LastName"]));
                }
            }
            return customer;
        }
        /// <summary>
        ///     Find the product amount based on the location id and product name.
        /// </summary>
        /// <param name="productName">Valid product name</param>
        /// <param name="locationID">Valid location ID</param>
        /// <returns>The product amount in the local inventory.</returns>
        public int InventoryAmount(string productName, int locationID)
        {
            DataSet dataSet = DBReadOnly(
                "SELECT ProductAmount From StoreInventory WHERE LocationID = @locationID AND ProductName = @productName;",
                new string[] { "locationID", "productName" },
                new object[] { locationID, productName });
            int amount = (int)dataSet.Tables[0].Rows[0]["ProductAmount"];
            return amount;
        }

        public int GetOrderNumber(int customerID)
        {
            int result = DBWriteOnly("INSERT CustomerOrder VALUES (@customerID)",
                new string[] { "customerID" },
                new object[] { customerID });
            if (result > 0)
            {
                DataSet dataSet = DBReadOnly("SELECT MAX(OrderNum) AS OrderNum From CustomerOrder WHERE CustomerID = @customerID;",
                    new string[] { "customerID" },
                    new object[] { customerID });
                return (int)dataSet.Tables[0].Rows[0]["OrderNum"];
            }
            return -1;
        }
        /// <summary>
        ///     Iterate the Order List data to the database, and update inventory amount.
        ///     Return summary of the insertion as receipt.
        /// </summary>
        /// <param name="order">Order type class</param>
        /// <returns>Summary of the insertion as receipt.</returns>
        public IEnumerable<Order> AddOrder(List<Order> order)
        {
            List<Order> receipt = new();
            foreach (Order orderProduct in order)
            {
                // insert order product & update inventory amount
                if (!InsertOrderProduct(orderProduct) || !UpdateInventoryAmount(orderProduct))
                    break;
            }
            // all success
            DataSet dataSet = DBReadOnly("SELECT * FROM OrderProduct WHERE OrderNum = @orderNum;",
                    new string[] { "orderNum" },
                    new object[] { order[0].OrderNum });
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                receipt.Add(new((int)row["OrderNum"], (string)row["ProductName"], (int)row["Amount"], (int)row["LocationID"], (DateTimeOffset)row["OrderTime"]));
            }
            return receipt;
        }

        /// <summary>
        ///     Insert order product to 
        /// </summary>
        /// <param name="order"></param>
        /// <returns>true if insert success, false otherwise.</returns>
        private bool InsertOrderProduct(Order order)
        {
            int result = DBWriteOnly("INSERT OrderProduct (OrderNum, ProductName, Amount, LocationID) " +
                "VALUES (@orderNum, @productName, @amount, @locationID);",
                new string[] { "orderNum", "productName", "amount", "locationID" },
                new object[] { order.OrderNum, order.ProductName, order.ProductQty, order.LocationID });
            if (result > 0) return true;
            return false;
        }
        /// <summary>
        ///     Update inventory table column. 
        ///     Subtract the inventory amount by the customer order amount.
        /// </summary>
        /// <param name="order"></param>
        /// <returns>true if update success, false otherwise.</returns>
        private bool UpdateInventoryAmount(Order order)
        {
            string query = "UPDATE StoreInventory " +
                "SET ProductAmount = ProductAmount - @orderAmount " +
                "WHERE LocationID = @locationID AND ProductName = @productName;";

            int result = DBWriteOnly(query,
                new string[] { "orderAmount", "locationID", "productName" },
                new object[] { order.ProductQty, order.LocationID, order.ProductName });
            if (result > 0) return true;
            return false;
        }

        public List<decimal> GetPrice(List<Order> order)
        {
            List<decimal> price = new();
            foreach (Order item in order)
            {
                DataSet dataSet = DBReadOnly(
                    "SELECT Price FROM StoreInventory WHERE LocationID = @locationID AND ProductName=@productName",
                    new string[] { "locationID", "productName" },
                    new object[] { item.LocationID, item.ProductName });
                price.Add((decimal)dataSet.Tables[0].Rows[0]["Price"]);
            }
            return price;
        }
    }
}
