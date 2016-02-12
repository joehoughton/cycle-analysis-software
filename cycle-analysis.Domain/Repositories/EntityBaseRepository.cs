/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using cycle_analysis.Domain.Context;
    using cycle_analysis.Domain.Generic;
    using cycle_analysis.Domain.Infrastructure;

    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private CycleAnalysisContext dataContext;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected CycleAnalysisContext DbContext
        {
            get { return this.dataContext ?? (this.dataContext = this.DbFactory.Init()); }
        }
        public EntityBaseRepository(IDbFactory dbFactory)
        {
            this.DbFactory = dbFactory;
        }

        public virtual IQueryable<T> GetAll()
        {
            return this.DbContext.Set<T>();
        }
        public virtual IQueryable<T> All
        {
            get
            {
                return this.GetAll();
            }
        }
        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = this.DbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
        public T GetSingle(int id)
        {
            return this.GetAll().FirstOrDefault(x => x.Id == id);
        }
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return this.DbContext.Set<T>().Where(predicate);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = this.DbContext.Entry(entity);
            this.DbContext.Set<T>().Add(entity); 
        }
        public virtual void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = this.DbContext.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = this.DbContext.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }
    }
}
