using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend.Data.Repositories.Implementation
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private GPMSDbContext _dbContext;
        public GenericRepository(GPMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Entity entity)
        {
            _dbContext.Set<Entity>().Add(entity);
        }

        public void AddRange(List<Entity> entities)
        {
            _dbContext.Set<Entity>().AddRange(entities);
        }

        public void Delete(Entity entity)
        {
            _dbContext.Set<Entity>().Remove(entity);
        }

        public Entity Details(Guid id)
        {
            return _dbContext.Set<Entity>().Find(id);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Entity> Search()
        {
            return _dbContext.Set<Entity>();
        }

        public void Update(Entity entity)
        {
            _dbContext.Set<Entity>().Update(entity);
        }

        public void UpdateRange(Entity entities)
        {
            _dbContext.Set<Entity>().UpdateRange(entities);
        }
    }
}