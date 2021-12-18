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

            using SqlConnection connection = new(_connectionString);
            connection.Open();
            using SqlCommand command = new("SELECT * FROM Location", connection);
            using SqlDataAdapter adapter = new(command);
            DataSet dataSet = new();
            adapter.Fill(dataSet);
            connection.Close();
            foreach(DataRow row in dataSet.Tables[0].Rows)
            {
                result.Add(new((int)row["ID"], (string)row["StoreLocation"]));
            }
            return result;
        }
    }
}
