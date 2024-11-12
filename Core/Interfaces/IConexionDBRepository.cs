using Microsoft.Data.SqlClient;

namespace Core.Interfaces
{
    public interface IConexionDBRepository
    {
        Task Execute(Func<SqlCommand, Task> action);
        Task<T> Execute<T>(Func<SqlCommand, Task<T>> action);
    }
}
