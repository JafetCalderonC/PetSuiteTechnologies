using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                "Initial Catalog=jafetCalderonAprendiendo;Persist Security Info=True;" +
                "User ID=sysman;Password=Cenfotec123!"; //** ESTO TENEMOS QEU ACTUALIZARLO**
        }

        public static SqlDao GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SqlDao();
            }
            return _instance;
        }

        public void ExecuteProcedure(SqlOperation sqlOperation)
        {

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(sqlOperation.ProcedureName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {

                    foreach (var param in sqlOperation.Parameters)
                    {
                        command.Parameters.Add(param);

                    }
                    conn.Open();
                    command.ExecuteNonQuery();
                }

            }


        }

        // Manda el query a la base de datos y lee lo que devuelve y lo retorna en una lista.
        public List<Dictionary<string, object>> ExecuteQueryProcedure(SqlOperation sqlOperation) // tiene mucho mas sentido que sea un objeto tipo llave, valor. La lista de diccionarios va a ser la tabla y 
                                                                                                 // como el diccionario es una lista de varios objetos llave valor, va a ser cada row, cada objeto llave valor va a ser una columna. 
        {

            var lstResults = new List<Dictionary<string, object>>();


            //Aqui indicamos con cual BD trabajamos
            using (var conn = new SqlConnection(_connectionString))
            {
                //Aqui indicamos cual SP voy a utilizar
                using (var command = new SqlCommand(sqlOperation.ProcedureName, conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    //Recorremos la lista de parametros y los agregamos a la ejecucion
                    foreach (var param in sqlOperation.Parameters)
                    {
                        command.Parameters.Add(param);

                    }

                    //Ejecutamos "contra" la base datos
                    conn.Open();

                    //Levantar el proceso de extraccion de data
                    var reader = command.ExecuteReader(); // a partir de aqui cambia porque lo que necesito es leer el result

                    //Validar que tenga registros
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            //Un diccionario por cada fila
                            var row = new Dictionary<string, object>();

                            for (var index = 0; index < reader.FieldCount; index++) // va iterando en los diferentes elementos del diccionario
                            {
                                var key = reader.GetName(index);
                                var value = reader.GetValue(index);

                                row[key] = value; // esta es la forma de asignarle valores a un diccionario
                            }
                            lstResults.Add(row);
                        }
                    }

                }

            }

            return lstResults;
        }
    }
}
