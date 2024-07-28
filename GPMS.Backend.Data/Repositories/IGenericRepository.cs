using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend.Data.Repositories
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        IQueryable<Entity> GetAll();
        IQueryable<Entity> Search(Expression<Func<Entity, bool>> expression);
        Entity Details (Guid id);
        void Add(Entity entity);
        void AddRange(List<Entity> entities);
        void Update (Entity entity);
        void UpdateRange (Entity entities);
        void Delete (Entity entity);
        Task Save ();
        List<Entity> GetUnAddedEntityList();
        Entity GetUnAddedEntity();
        Entity GetUnAddedEntityById(Guid id);
    }
}