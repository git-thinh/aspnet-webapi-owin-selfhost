﻿namespace People.Domain.Context
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Microsoft.AspNet.Identity.EntityFramework;

    public abstract class BaseDbContext : IdentityDbContext<IdentityUser>
    {
        protected BaseDbContext() : base("DefaultConnection", throwIfV1Schema: false) { }

        protected BaseDbContext(string connectionString, bool throwIfV1Schema) : base(connectionString, throwIfV1Schema) { }

        public virtual IDbSet<T> GetDbSet<T>() where T : class
        {
            return base.Set<T>();
        }

        public virtual void Entry<T>(T entity, Action<DbEntityEntry<T>> stateAction) where T : class
        {
            stateAction(base.Entry(entity));
        }

        #region Obsolete methods, hides superclass implementation
        [Obsolete("Use overload for unit tests.")]
        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            throw new ApplicationException("Use overload for unit tests.");
        }

        [Obsolete("Use overload for unit tests.")]
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            throw new ApplicationException("Use overload for unit tests.");
        }

        [Obsolete("Use overload for unit tests.")]
        public new DbSet Set(Type entity)
        {
            throw new ApplicationException("Use overload for unit tests.");
        }
        #endregion
    }
}