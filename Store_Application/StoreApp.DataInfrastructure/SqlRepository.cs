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
                DataSet dataSet = DBReadOnly("SELECT ProductName, Price FROM StoreInventory WHERE LocationID = @locID;",
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

        /// <summary>
        ///     A method takes a query parameters and returns a DataSet type result. 
        ///     Only for read only database connection. (ex. SELECT)
        ///     Notes: colnames parameter 
        /// </summary>
        /// <param name="query">Database query command</param>
        /// <param name="paramName">parameter name in your query (ex. "...WHERE ID = @id" paramName = "id")</param>
        /// <param name="inputVal">the value for the query search condition</param>
        /// <returns>DataSet type</returns>
        public DataSet DBReadOnly(string? query, string[]? paramName = null, object[]? inputVal = null)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();
            using SqlCommand command = new(query, connection);
            if (paramName!= null && inputVal != null)
            {
                for (int i = 0; i < inputVal.Length; i++) {
                    command.Parameters.AddWithValue($"@{paramName[i]}", inputVal[i]);
                }
            }
            using SqlDataAdapter adapter = new(command);
            DataSet dataSet = new();
            adapter.Fill(dataSet);
            connection.Close();
            return dataSet;
        }

    }
}
