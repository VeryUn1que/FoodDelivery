using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Interfaces; 

     
namespace Infrastructure.Data
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		// By using ReadOnly ApplicationDbContext, you can have access to only
		// querying capabilities of DbContext. UnitOfWork writes
		// (commits) to the PHYSICAL tables (not internal object).
		private readonly ApplicationDbContext _dbContext;
		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

		}
		public void Add(T entity)
		{
			_dbContext.Set<T>().Add(entity);
			_dbContext.SaveChanges();
		}
		public void Delete(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
			_dbContext.SaveChanges();
		}
		public void Delete(IEnumerable<T> entities)
		{
			_dbContext.Set<T>().RemoveRange(entities);
			_dbContext.SaveChanges();
		}
		public virtual T Get(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string? includes = null)
		{
			if(includes == null)
			{
				if (asNoTracking)
				{
					return _dbContext.Set<T>().AsNoTracking().Where(predicate).FirstOrDefault();
				}
				else
				{
					return _dbContext.Set<T>().Where(predicate).FirstOrDefault();
				}

			}
			else
			{
				IQueryable<T> queryable = _dbContext.Set<T>();
				foreach (var includeProperty in includes.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);   
				}
				if (asNoTracking)
				{
					return queryable.AsNoTracking().Where(predicate).FirstOrDefault();
				}
				else
				{
					return queryable.Where(predicate).FirstOrDefault();
				}
			}
		}
		public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string? includes = null)
		{
			if (includes == null)
			{
				if (asNoTracking)
				{
					return await _dbContext.Set<T>().AsNoTracking().Where(predicate).FirstOrDefaultAsync();
				}
				else
				{
					return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
				}

			}
			else
			{
				IQueryable<T> queryable = _dbContext.Set<T>();
				foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}
				if (asNoTracking)
				{
					return await queryable.AsNoTracking().Where(predicate).FirstOrDefaultAsync();
				}
				else
				{
					return await queryable.Where(predicate).FirstOrDefaultAsync();
				}
			}
		}
		// The virtual keyword is used to modify a method, property, indexer, or
		// and allows for it to be overridden in a derived class.
		public virtual T GetById(int id)
		{
			return _dbContext.Set<T>().Find(id);
		}

		public virtual IEnumerable<T> List()
		{
			return _dbContext.Set<T>().ToList().AsEnumerable();
		}

		public virtual IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null)
		{
			IQueryable<T> queryable = _dbContext.Set<T>();
			if(predicate != null && includes == null)
			{
				return _dbContext.Set<T>().Where(predicate).AsEnumerable();
			} else if (includes != null)
			{
				foreach(var includeProperty in includes.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}
			}
			if (predicate == null)
			{
				if (orderBy == null)
				{
					return queryable.AsEnumerable();
				}
				else
				{
					return queryable.OrderBy(orderBy).ToList().AsEnumerable();
				}
			}
			else
			{
				if (orderBy == null)
				{
					return queryable.Where(predicate).ToList().AsEnumerable();
				}
				else
				{
					return queryable.Where(predicate).OrderBy(orderBy).ToList().AsEnumerable();
				}
			}
		}

		public virtual async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null)
		{
			IQueryable<T> queryable = _dbContext.Set<T>();
			if (predicate != null && includes == null)
			{
				return await _dbContext.Set<T>().Where(predicate).ToListAsync();
			}
			else if (includes != null)
			{
				foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}
			}
			if (predicate == null)
			{
				if (orderBy == null)
				{
					return await queryable.ToListAsync();
				}
				else
				{
					return await queryable.OrderBy(orderBy).ToListAsync();
				}
			}
			else
			{
				if (orderBy == null)
				{
					return await queryable.Where(predicate).ToListAsync();
				}
				else
				{
					return await queryable.Where(predicate).OrderBy(orderBy).ToListAsync();
				}
			}
		}
		public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, int>>? orderBy = null, string? includes = null)
		{
			IQueryable<T> queryable = _dbContext.Set<T>();
			if (!string.IsNullOrEmpty(includes))
			{
				foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}
			}
			if (predicate != null)
			{
				queryable = queryable.Where(predicate);
			}
			if (orderBy != null)
			{
				queryable = queryable.OrderBy(orderBy);
			}
			return await queryable.ToListAsync();
		}
		public void Update(T entity)
		{
			_dbContext.Entry(entity).State = EntityState.Modified;
			_dbContext.SaveChanges();
		}
		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, int>>? orderBy = null, string? includes = null)
		{
			IQueryable<T> queryable = _dbContext.Set<T>();
			if (!string.IsNullOrEmpty(includes))
			{
				foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					queryable = queryable.Include(includeProperty);
				}
			}
			if (predicate != null)
			{
				queryable = queryable.Where(predicate);
			}
			if (orderBy != null)
			{
				queryable = queryable.OrderBy(orderBy);
			}
			return queryable.ToList();
		}
	}

}
