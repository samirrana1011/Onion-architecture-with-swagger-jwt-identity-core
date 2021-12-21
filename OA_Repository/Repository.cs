using Microsoft.EntityFrameworkCore;
using OA_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OA_Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return entities.AsEnumerable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> GetFilteredData(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable < T >> orderBy = null,
            string includeProperties = "",
            int first = 0, int offset = 0)
        {
            try
            {
                IQueryable<T> query = entities;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (offset > 0)
                {
                    query = query.Skip(offset);
                }
                if (first > 0)
                {
                    query = query.Take(first);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }
                else
                {
                    return await query.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T GetById(int Id)
        {
            try
            {
                return entities.FirstOrDefault(x => x.Id == Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public  void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entities.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                entities.Remove(entity);
                _context.SaveChanges();
            } 
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

       
    }
}
