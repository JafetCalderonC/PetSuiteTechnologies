using DataAccess.DAOs;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class SqlDao
    {
        private string _connectionString;
        private static SqlDao? _instance;

        private SqlDao()
        {
            _connectionString = "Data Source=jcalderon-ucenfotec2023server.database.windows.net;" +
                "Initial Catalog=PetSuiteTechnologies;Persist Security Info=True;" +
                "User ID=sysman;Password=Cenfotec123!";
        }

        public static SqlDao GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SqlDao();
            }
            return _instance;
        }

        public int ExecuteProcedure(SqlOperation sqlOperation)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sqlOperation.ProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (var param in sqlOperation.Parameters)
                {
                    command.Parameters.Add(param);
                }

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int ExecuteProcedure(SqlOperation sqlOperation, out int identity)
        {
            identity = -1;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sqlOperation.ProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (var param in sqlOperation.Parameters)
                {
                    command.Parameters.Add(param);
                }

                // Add an output parameter for the identity value
                SqlParameter outputIdParam = new SqlParameter("@P_ID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                var rowsAffected = command.ExecuteNonQuery();

                // Retrieve the identity value from the output parameter
                if (rowsAffected > 0)
                {
                    identity = (int)outputIdParam.Value;
                }

                return rowsAffected;
            }
        }

        public List<Dictionary<string, object>> ExecuteQueryProcedure(SqlOperation sqlOperation)
        {
            var lstResults = new List<Dictionary<string, object>>();

            using (var conn = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sqlOperation.ProcedureName, conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (var param in sqlOperation.Parameters)
                {
                    command.Parameters.Add(param);
                }

                conn.Open();
                var reader = command.ExecuteReader();

                // Check if the reader has rows
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();

                        // Get the columns
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var key = reader.GetName(i);
                            var value = reader.GetValue(i);
                            // Add the column to the dictionary
                            row.Add(key, value);
                        }

                        // Add the row to the list
                        lstResults.Add(row);
                    }
                }
            }

            return lstResults;
        }
    }
}