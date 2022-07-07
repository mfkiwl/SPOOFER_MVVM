using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Services.User.Repository
{
    public interface IRepository<T>
    {
        void AddOrUpdate(T entity);
        void Remove(params string[] id);
        void Update(params T [] entity);
        IEnumerable<T> GetAll();
        T GetByViewModel(string id);
        void Save();
    }
}
