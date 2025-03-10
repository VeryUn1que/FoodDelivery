using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
	public interface IGenericRepository<T> where T : class
	{
		T GetById(int id);

		T Get(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null);

		Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false, string includes = null);

		IEnumerable<T> List();

		//Returns enumerable list of results to iterate through.
		IEnumerable<T> List(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null);

		//Same as above, just an asynchronous action
		Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> orderBy = null, string includes = null);

		void Add(T entity);

		void Delete(T entity);

		void Delete(IEnumerable<T> entities);

		void Update(T entity);

	}
}
