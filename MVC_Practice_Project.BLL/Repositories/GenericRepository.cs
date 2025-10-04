﻿using Microsoft.EntityFrameworkCore;
using MVC_Practice_Project.BLL.Interfaces;
using MVC_Practice_Project.DAL.Data.Contexts;
using MVC_Practice_Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Practice_Project.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>)await _context.Employees.Include(E => E.WorkFor).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.WorkFor).FirstOrDefaultAsync(E => E.Id == id) as T;
            }
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T model)
        {
            await _context.Set<T>().AddAsync(model);
        }

        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
        }


        public void Update(T model)
        {
            _context.Set<T>().Update(model);
        }
    }
}
