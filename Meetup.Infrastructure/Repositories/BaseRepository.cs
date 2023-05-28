namespace Meetup.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        private readonly MeetupContext _dbContext;
        private readonly DbSet<T> _table;
             
        public BaseRepository(MeetupContext dbContext)
        {
            _dbContext = dbContext;
            _table = _dbContext.Set<T>();
        }

        /// <summary>
        /// Gets entities collection based on some criteria.
        /// </summary>
        /// <param name="include">Allows to iclude related entities.</param>
        /// <param name="expression">Allows to filter entities.</param>
        /// <returns>The collection of entities.</returns>
        public async Task<IEnumerable<T>> GetAllByAsync(Func<IQueryable<T>, 
            IIncludableQueryable<T, object>>? include = null, 
            Expression<Func<T, bool>>? expression = null)
        {
            IQueryable<T> query = _table;

            if (expression is not null)
            {
                query = query.Where(expression);
            }
            if (include is not null)
            {
                query = include(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Gets a single entity based on some criteria.
        /// </summary>
        /// <param name="include">Allows to iclude related entities.</param>
        /// <param name="expression">Allows to filter entities.</param>
        /// <returns>A certain entity.</returns>
        public async Task<T> GetOneByAsync(Func<IQueryable<T>, 
            IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, bool>>? expression = null)
        {
            IQueryable<T> query = _table;

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            if (include is not null)
            {
                query = include(query);
            }

            var model = await query.AsNoTracking().FirstOrDefaultAsync();

            return model!;
        }

        /// <summary>
        /// Gets a single entity based on some criteria with EF Tracking.
        /// </summary>
        /// <param name="include">Allows to iclude related entities.</param>
        /// <param name="expression">Allows to filter entities.</param>
        /// <returns>A certain entity.</returns>
        public async Task<T> GetOneManyToManyAsync(Func<IQueryable<T>, 
            IIncludableQueryable<T, object>>? include = null, 
            Expression<Func<T, bool>>? expression = null)
        {
            IQueryable<T> query = _table;

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            if (include is not null)
            {
                query = include(query);
            }

            var model = await query.FirstOrDefaultAsync();

            return model!;
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity">The entity to be created.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateAsync(T entity)
        {
            await _table.AddAsync(entity);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing entity. 
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(T entity)
        {
            _table.Update(entity);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Removes an existing entity.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAsync(T entity)
        {
            _table.Remove(entity);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Saves changes.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            var saved = await _dbContext.SaveChangesAsync();
            return saved > 0;
        }
    }
}
