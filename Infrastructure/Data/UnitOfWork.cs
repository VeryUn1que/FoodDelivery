﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace Infrastructure.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext;
		
		public UnitOfWork(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		private IGenericRepository<Category> _Category;

		public IGenericRepository<Category> Category
		{
			get
			{
				if (_Category == null)
				{
					_Category = new GenericRepository<Category>(_dbContext);
				}
				return _Category;
			}
		}

		public IGenericRepository<Category> CategoryRepository => throw new NotImplementedException();

		public int Commit()
		{
			return _dbContext.SaveChanges();
		}

		public async Task<int> CommitAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}
	}
}
