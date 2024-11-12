using Core.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_Pasteleria.Repositories
{
    public class ConexionDBRepository(IConfiguration configuration) : IConexionDBRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DBConnection") ?? string.Empty;

        public async Task Execute(Func<SqlCommand, Task> action)
        {
            SqlConnection cxn = new();
            SqlCommand cmd = new();

            try
            {
                cxn = new SqlConnection(_connectionString);
                await cxn.OpenAsync();

                cmd = new SqlCommand(String.Empty, cxn, cxn.BeginTransaction()) { CommandType = CommandType.StoredProcedure };

                await action(cmd);

                await cmd.Transaction.CommitAsync();
                await cxn.CloseAsync();
                cxn.Dispose();
                cxn = new();
            }
            catch
            {
                await cmd.Transaction.RollbackAsync();

                if (cxn.State != ConnectionState.Closed)
                {
                    await cxn.CloseAsync();
                    cxn?.Dispose();
                }

                throw;
            }
        }

        public async Task<T> Execute<T>(Func<SqlCommand, Task<T>> action)
        {
            SqlConnection cxn = new();
            SqlCommand cmd = new();

            try
            {
                cxn = new SqlConnection(_connectionString);
                await cxn.OpenAsync();

                cmd = new SqlCommand(String.Empty, cxn, cxn.BeginTransaction()) { CommandType = CommandType.StoredProcedure };

                T result = await action(cmd);

                await cmd.Transaction.CommitAsync();
                await cxn.CloseAsync();
                cxn.Dispose();
                cxn = new();

                return result;
            }
            catch
            {
                await cmd.Transaction.RollbackAsync();

                if (cxn.State != ConnectionState.Closed)
                {
                    await cxn.CloseAsync();
                    cxn.Dispose();
                }

                throw;
            }
        }
    }
}
