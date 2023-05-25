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

        public async Task CreateAsync(T entity)
        {
            await _table.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _table.Update(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _table.Remove(entity);
            await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var saved = await _dbContext.SaveChangesAsync();

            return saved > 0 ? true : false;
        }
    }
}
