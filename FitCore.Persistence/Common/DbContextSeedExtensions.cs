using FitCore.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Persistence.Common
{
    public static class DbContextSeedExtensions
    {
        /// <summary>
        /// جهت اضافه کردن مغادیر اولیه در جداول دیتابیس
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entities"></param>
        /// <param name="predicateFactory"></param>
        /// <returns></returns>
        public static async Task SeedIfNotExists<TEntity>(
            this DataBaseContext context,
            IEnumerable<TEntity> entities,
            Func<TEntity, Expression<Func<TEntity, bool>>> predicateFactory)
            where TEntity : class
        {
            var dbSet = context.Set<TEntity>();

            foreach (var entity in entities)
            {
                var predicate = predicateFactory(entity);
                var exists = await dbSet.AnyAsync(predicate);

                if (!exists)
                {
                    await dbSet.AddAsync(entity);
                }
            }

            await context.SaveChangesAsync(default);
        }
    }
}
