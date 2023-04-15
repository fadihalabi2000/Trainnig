using NewsApiData;
using NewsApiDomin.Models;
using Repositories.Interfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsApiRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NewsApiRepositories
{
    public class CategoryReository:BaseRepository<Category>,ICategoryRepository
    {
        private readonly NewsApiDbContext dbContext;

        public CategoryReository(NewsApiDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Category> CheckCategoryName(string categoryName)
        {
           
                var dbSet = await this.dbContext.Set<Category>().FirstOrDefaultAsync(a => a.CategoryName == categoryName);
                return dbSet!;
            
        }

        public new async Task<List<Category>> GetAllAsync()
        {
            var dbSet = this.dbContext.Set<Category>();


            return await Task.Run(() => dbSet.Where(c => c.IsDeleted == false).ToListAsync());
        }
        public new async Task<Category> GetByIdAsync(int id)
        {

            var dbSet = await this.dbContext.Set<Category>().Include(a =>a.Articles.OrderByDescending(a => a.PublishDate)).ToListAsync();

            Category? entity =  dbSet.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            return entity!;


        }



  

    }
}
