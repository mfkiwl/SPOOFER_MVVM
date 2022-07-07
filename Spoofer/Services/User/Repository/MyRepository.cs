using Microsoft.EntityFrameworkCore;
using Spoofer.Data;
using Spoofer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Services.User.Repository
{
    public class MyRepository<T> : IRepository<T> where T : class, IEntityWithId
    {
        private readonly CoordinatesContext _context;
        private readonly DbSet<T> _table;
        public MyRepository(CoordinatesContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }
        public void AddOrUpdate(T entity)
        {
            _table.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            var list = _table.ToList();
            return list;
        }

        public T GetByViewModel(string id)
        {
            T existing = _table.Find(id);
            return existing;
        }
        public void Remove(params string[] id)
        {
            foreach (var item in id)
            {
                T existing = _table.Find(item);
                _table.Remove(existing);
            }
        }
        public void Update(params T[] entity)
        {
            foreach (var item in entity)
            {
                var currentEntity = _table.Find((item as IEntityWithId).Id);
                _context.Entry(currentEntity).CurrentValues.SetValues(item);
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
