using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseWorksHandler.WEB.Repositories
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task InsertAsync(T item);

        Task UpdateAsync(T item);

        Task DeleteAsync(int id);

        Task<T> SelectAsync(int id);
    }
}
