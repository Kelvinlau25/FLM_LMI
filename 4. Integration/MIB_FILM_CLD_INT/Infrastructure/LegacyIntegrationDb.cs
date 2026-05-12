using System.Data;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;

namespace MIB_FILM_CLD_INT.Infrastructure
{
    public sealed class LegacyIntegrationDb(ConnectionStringFileProvider connectionStringProvider)
    {
        private readonly ConnectionStringFileProvider _connectionStringProvider = connectionStringProvider;

        public DataTable ExecuteSqlDataTable(string connectionFileName, string storedProcedureName, params SqlParameter[] parameters)
        {
            using SqlConnection connection = CreateSqlConnection(connectionFileName);
            using SqlCommand command = CreateSqlCommand(connection, storedProcedureName, parameters);
            DataTable table = new();

            connection.Open();
            table.Load(command.ExecuteReader());

            return table;
        }

        public void ExecuteSqlNonQuery(string connectionFileName, string storedProcedureName, params SqlParameter[] parameters)
        {
            using SqlConnection connection = CreateSqlConnection(connectionFileName);
            using SqlCommand command = CreateSqlCommand(connection, storedProcedureName, parameters);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void ExecuteOracleStoredProcedureReader(string connectionFileName, string storedProcedureName, Action<OracleDataReader> readAction)
        {
            using OracleConnection connection = CreateOracleConnection(connectionFileName);
            using OracleCommand command = new(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0
            };

            OracleParameter cursor = command.Parameters.Add("SREFDATA", OracleDbType.RefCursor);
            cursor.Direction = ParameterDirection.Output;

            connection.Open();
            using OracleDataReader reader = command.ExecuteReader();
            readAction(reader);
        }

        public void ExecuteOracleTextReader(string connectionFileName, string sql, Action<OracleDataReader> readAction)
        {
            using OracleConnection connection = CreateOracleConnection(connectionFileName);
            using OracleCommand command = new(sql, connection)
            {
                CommandType = CommandType.Text,
                CommandTimeout = 0
            };

            connection.Open();
            using OracleDataReader reader = command.ExecuteReader();
            readAction(reader);
        }

        private SqlConnection CreateSqlConnection(string connectionFileName)
        {
            return new SqlConnection(_connectionStringProvider.GetRequired(connectionFileName));
        }

        private OracleConnection CreateOracleConnection(string connectionFileName)
        {
            return new OracleConnection(_connectionStringProvider.GetRequired(connectionFileName));
        }

        private static SqlCommand CreateSqlCommand(SqlConnection connection, string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {
            SqlCommand command = new(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0
            };

            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }
    }
}
