namespace Meetup.ApplicationCore.Interfaces.IRepository
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        public Task<IEnumerable<T>> GetAllByAsync(Func<IQueryable<T>, 
          IIncludableQueryable<T, object>>? include = null,
          Expression<Func<T, bool>>? expression = null);

        public Task<T> GetOneByAsync(Func<IQueryable<T>, 
           IIncludableQueryable<T, object>>? include = null,
           Expression<Func<T, bool>>? expression = null);

        public Task<T> GetOneManyToManyAsync(Func<IQueryable<T>,
          IIncludableQueryable<T, object>>? include = null,
          Expression<Func<T, bool>>? expression = null);

        public Task CreateAsync(T entity);

        public Task UpdateAsync(T entity);

        public Task DeleteAsync(T entity);

        public Task<bool> SaveChangesAsync();
    }
}
