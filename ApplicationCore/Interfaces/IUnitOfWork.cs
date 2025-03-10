using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces
{
	public interface IUnitOfWork
	{
		IGenericRepository<Category> CategoryRepository { get; }

		int Commit();

		Task<int> CommitAsync();
	}
}
