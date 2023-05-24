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

        public async Task<IEnumerable<T>> GetAllByAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _table
                            .Where(_ => _.Id == id)
                            .FirstOrDefaultAsync();
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

        public async Task<bool> EntityExists(int id)
        {
            return await _table
                            .AnyAsync(_ => _.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            var saved = await _dbContext.SaveChangesAsync();

            return saved > 0 ? true : false;
        }
    }
}
