using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWorksHandler.WEB.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        void Insert(T item);

        void Update(T item);

        void Delete(T item);

        void Select(int id);
    }
}
